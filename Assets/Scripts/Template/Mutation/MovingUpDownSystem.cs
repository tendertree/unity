using Unity.Entities;
using Unity.Mathematics;
using Unity.Burst;
using Unity.Transforms;

public struct MovingUpDown : IComponentData
{
    public float Amplitude;    // 움직임의 폭
    public float Frequency;    // 움직임의 빈도
    public float3 StartPosition; // 시작 위치
    public float TimeOffset;   // 시간 오프셋 (선택적, 엔티티마다 다른 위치에서 시작하게 함)
}


[UpdateInGroup(typeof(SimulationSystemGroup))]
public partial struct MovingUpDownSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<MovingUpDown>();
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state) { }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        float time = (float)SystemAPI.Time.ElapsedTime;
        new MovingUpDownJob { Time = time }.ScheduleParallel();
    }
}

[BurstCompile]
public partial struct MovingUpDownJob : IJobEntity
{
    public float Time;

    void Execute(ref LocalTransform transform, in MovingUpDown movingUpDown)
    {
        float yOffset = movingUpDown.Amplitude * math.sin((Time + movingUpDown.TimeOffset) * movingUpDown.Frequency);
        transform.Position = movingUpDown.StartPosition + new float3(0, yOffset, 0);
    }
}