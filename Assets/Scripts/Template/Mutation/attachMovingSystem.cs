using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Collections;
[DisableAutoCreation]
[UpdateInGroup(typeof(InitializationSystemGroup))]
public partial struct AddMovingUpDownSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<Player>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.Temp);

        foreach (var (_, entity) in SystemAPI.Query<RefRO<Player>>().WithEntityAccess())
        {
            if (!SystemAPI.HasComponent<MovingUpDown>(entity))
            {
                var transform = SystemAPI.GetComponent<LocalTransform>(entity);
                ecb.AddComponent(entity, new MovingUpDown
                {
                    Amplitude = 1f,
                    Frequency = 1f,
                    StartPosition = transform.Position,
                    TimeOffset = 0f
                });
            }
        }

        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }
}