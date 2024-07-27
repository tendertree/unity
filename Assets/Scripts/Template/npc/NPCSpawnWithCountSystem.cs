using Unity.Burst;
using Unity.Entities;
using Unity.Logging;
using Unity.Transforms;

[BurstCompile]
public partial struct NpcSpawnWithCountSystem : ISystem
{
    private EntityQuery m_NpcQuery;

    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<Npc>();
        m_NpcQuery = state.GetEntityQuery(ComponentType.ReadOnly<Npc>());
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var currentNpcCount = m_NpcQuery.CalculateEntityCount();
        Log.Info($"Current NPC count: {currentNpcCount}");

        if (currentNpcCount >= 2)
        {
            //2개 이상이면 비활성
            state.Enabled = false;
            return;
        }


        var ecb = GetEntityCommandBuffer(ref state);
        var spawnCount = 2 - currentNpcCount;

        Log.Info($"Attempting to spawn {spawnCount} NPCs");

        new NpcSpawnJob
        {
            ECB = ecb,
            SpawnCount = spawnCount
        }.ScheduleParallel();

        // ECB는 자동으로 실행되므로 명시적인 실행은 제거합니다.
    }

    private EntityCommandBuffer.ParallelWriter GetEntityCommandBuffer(ref SystemState state)
    {
        var ecbSingleton = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>();
        var ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);
        return ecb.AsParallelWriter();
    }
}

[BurstCompile]
public partial struct NpcSpawnJob : IJobEntity
{
    public EntityCommandBuffer.ParallelWriter ECB;
    public int SpawnCount;

    [BurstCompile]
    private void Execute([ChunkIndexInQuery] int chunkIndex, in Npc npc)
    {
        if (npc.prefab == Entity.Null || chunkIndex >= SpawnCount) return;

        var et = ECB.Instantiate(chunkIndex, npc.prefab);
        ECB.SetComponent(chunkIndex, et, LocalTransform.FromPosition(npc.SpawnPosition));

        // Npc 컴포넌트 추가 (프리팹에 없는 경우)
        ECB.AddComponent<Npc>(chunkIndex, et);
    }
}