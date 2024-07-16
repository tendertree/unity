using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Logging;
[UpdateInGroup(typeof(SimulationSystemGroup))]
public partial struct RotationSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<Rotate>();
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state) { }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        float deltaTime = SystemAPI.Time.DeltaTime;

        new RotateJob
        {
            DeltaTime = deltaTime
        }.ScheduleParallel();
    }
}

[BurstCompile]
public partial struct RotateJob : IJobEntity
{
    public float DeltaTime;

    void Execute(ref LocalTransform transform, ref Rotate rotateComponent)
    {

        // 각도 업데이트
        rotateComponent.CurrentAngle += rotateComponent.RotationSpeed * DeltaTime;

        // 2π를 초과하면 0으로 리셋 (선택사항)
        rotateComponent.CurrentAngle %= 2 * math.PI;

        // 새로운 위치 계산
        float3 offset = new float3(
            math.cos(rotateComponent.CurrentAngle) * rotateComponent.Radius,
            0,
            math.sin(rotateComponent.CurrentAngle) * rotateComponent.Radius
        );

        transform.Position = rotateComponent.CenterPoint + offset;

        // 엔티티가 이동 방향을 바라보도록 회전 (선택사항)
        transform.Rotation = quaternion.LookRotation(offset, math.up());
    }
}