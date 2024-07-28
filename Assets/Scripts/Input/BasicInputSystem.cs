using Unity.Entities;
using UnityEngine;

namespace Input
{
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    public partial class BasicInputSystem : SystemBase
    {
        private BasictInput input;
        private Entity inputEntity;

        protected override void OnCreate()
        {
            input = new BasictInput();
            input.Enable();

            // BasicInputData 싱글톤 엔티티 생성
            inputEntity = EntityManager.CreateEntity();
            EntityManager.AddComponentData(inputEntity, new BasicInputData());
        }

        protected override void OnUpdate()
        {
            var inputData = SystemAPI.GetSingletonRW<BasicInputData>();

            // 입력 데이터 업데이트
            inputData.ValueRW.Direction = input.Character.Direction.ReadValue<Vector2>();
            inputData.ValueRW.Click = input.Character.Click.ReadValue<float>();
            inputData.ValueRW.MousePostion = input.Character.MousePosition.ReadValue<Vector2>();

            // 디버그 로그 추가
            //Log.Info($"BasicInputSystem Update - Click: {inputData.ValueRO.Click}, "$"Direction: {inputData.ValueRO.Direction}, " $"MousePosition: {inputData.ValueRO.MousePostion}");
        }

        protected override void OnDestroy()
        {
            if (input != null)
            {
                input.Disable();
                input.Dispose();
            }
        }
    }
}