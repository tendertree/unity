using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

[BurstCompile]

public struct MoveTarget : IComponentData
{
    public Entity TargetEntity;
    public float Speed;
}


public partial struct MoveSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<MoveTarget>();
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        float deltaTime = SystemAPI.Time.DeltaTime;

        foreach (var (transform, moveTarget) in
            SystemAPI.Query<RefRW<LocalTransform>, RefRO<MoveTarget>>())
        {
            if (SystemAPI.Exists(moveTarget.ValueRO.TargetEntity))
            {
                float3 targetPosition = SystemAPI.GetComponent<LocalTransform>(moveTarget.ValueRO.TargetEntity).Position;
                float3 currentPosition = transform.ValueRO.Position;

                float3 direction = math.normalize(targetPosition - currentPosition);
                float3 movement = direction * moveTarget.ValueRO.Speed * deltaTime;

                // 목표 지점을 지나치지 않도록 체크
                if (math.lengthsq(targetPosition - currentPosition) > math.lengthsq(movement))
                {
                    transform.ValueRW.Position += movement;
                }
                else
                {
                    transform.ValueRW.Position = targetPosition;
                }
            }
        }
    }
}