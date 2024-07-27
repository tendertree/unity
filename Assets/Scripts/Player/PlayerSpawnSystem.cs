using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Logging;
using Unity.Burst;


[BurstCompile]
public partial struct PlayerSpawnSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        //debug용 중지 

        state.RequireForUpdate<PlayerSpawner>();
        state.RequireForUpdate<SystemActivationTag>();
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state) { }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var activationTag = SystemAPI.GetSingleton<SystemActivationTag>();
        if (!activationTag.IsActive)
        {
            return;
        }
        EntityCommandBuffer.ParallelWriter ecb = GetEntityCommandBuffer(ref state);
        new PlayerSpawnJob
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
    public partial struct PlayerSpawnJob : IJobEntity
    {
        public EntityCommandBuffer.ParallelWriter ECB;

        [BurstCompile]
        private void Execute(
            Entity entity,
            [ChunkIndexInQuery]
            int sortKey, ref PlayerSpawner player)
        {
            if (player.Prefab == Entity.Null)
            {
                Log.Error("Player prefab is null");
                return;
            }

            // Spawn the player entity
            Entity newEntity = ECB.Instantiate(sortKey, player.Prefab);
            // Set the position
            ECB.SetComponent(sortKey, newEntity, LocalTransform.FromPosition(player.SpawnPosition));
            ECB.AddComponent(sortKey, newEntity, new Player { grade = 1 });

        }
    }



}

