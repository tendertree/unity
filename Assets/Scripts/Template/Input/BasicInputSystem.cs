using Unity.Entities;
using UnityEngine;

public partial class BasicInputSystem : SystemBase
{
    private BasictInput input;

    protected override void OnCreate()
    {
        input = new BasictInput();
        input.Enable();

        // BasicInputData 싱글톤 엔티티 생성
        var inputEntity = EntityManager.CreateEntity();
        EntityManager.AddComponentData(inputEntity, new BasicInputData());
    }

    protected override void OnUpdate()
    {
        // 싱글톤 BasicInputData 가져오기
        var inputData = SystemAPI.GetSingletonRW<BasicInputData>();

        // 입력 데이터 업데이트
        inputData.ValueRW.Direction = input.Character.Direction.ReadValue<Vector2>();
        inputData.ValueRW.Click = input.Character.Click.ReadValue<float>();
        inputData.ValueRW.MousePostion = input.Character.MousePosition.ReadValue<Vector2>();
    }
}