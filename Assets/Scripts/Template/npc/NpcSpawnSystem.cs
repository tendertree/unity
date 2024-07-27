using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

[BurstCompile]
public partial struct NpcSpawnSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<Npc>();
        state.Enabled = false;
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var ecb = GetEntityCommandBuffer(ref state);
        new NpcSpawnJob
        {
            ECB = ecb
        }.ScheduleParallel();
        state.Enabled = false;
    }

    private EntityCommandBuffer.ParallelWriter GetEntityCommandBuffer(ref SystemState state)
    {
        var ecbSingleton = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>();
        var ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);
        return ecb.AsParallelWriter();
    }

    [BurstCompile]
    public partial struct NpcSpawnJob : IJobEntity
    {
        public EntityCommandBuffer.ParallelWriter ECB;

        [BurstCompile]
        public void Execute(Entity entity, [ChunkIndexInQuery] int sortKey, ref Npc npc)
        {
            if (npc.prefab == Entity.Null) return;

            // Spawn the Npc entity
            var et = ECB.Instantiate(sortKey, npc.prefab);

            // Set the position using the authoring entity's position
            ECB.SetComponent(sortKey, et, LocalTransform.FromPosition(npc.SpawnPosition));
        }
    }
}