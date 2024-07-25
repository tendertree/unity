using Unity.Burst;
using Unity.Entities;
using Unity.Collections;

// DummyTagComponent 정의
public struct DummyTagComponent : IComponentData
{
    // 태그 컴포넌트이므로 데이터가 필요 없습니다.
}

// Age 컴포넌트 정의
public struct AgeComponent : IComponentData
{
    public int Value;
}

// DummySystem 정의 (ISystem 사용)
public partial struct DummySystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        // 초기화가 필요한 경우 여기에 작성
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
        // 정리가 필요한 경우 여기에 작성
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        // Delta Time을 가져옵니다. 필요한 경우 사용할 수 있습니다.
        float deltaTime = SystemAPI.Time.DeltaTime;

        // Job을 생성하고 스케줄링합니다.
        new DummyJob { }.ScheduleParallel();
    }
}

// DummyJob 정의
[BurstCompile]
public partial struct DummyJob : IJobEntity
{
    void Execute(ref AgeComponent age, in DummyTagComponent dummy)
    {
        // 여기서 Age 컴포넌트를 조작합니다.
        // 예를 들어, 매 프레임마다 나이를 1씩 증가시킵니다.
        age.Value++;

        // 추가적인 로직을 여기에 구현할 수 있습니다.
        // 예: 특정 나이에 도달하면 다른 작업 수행
        if (age.Value >= 100)
        {
            // 예: 100살 이상이 되면 어떤 작업을 수행
            // 주의: 엔티티 구조를 변경하는 작업(예: 컴포넌트 추가/제거)은 
            // EntityCommandBuffer를 사용해야 합니다.
        }
    }
}