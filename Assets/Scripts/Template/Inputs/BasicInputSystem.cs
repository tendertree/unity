using Unity.Entities;
using Unity.Logging;
using Unity.Mathematics;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.InputSystem;

public partial class BasicInputSystem : SystemBase
    {
        private BasictInput input = null;
        protected override void OnCreate()
        {
            input = new BasictInput();
            input.Enable();
        }

        protected override void OnUpdate()
        {
            foreach (RefRW<BasicInputData> data in SystemAPI.Query<RefRW<BasicInputData>>())
            {
                data.ValueRW.Direction = input.Character.Direction.ReadValue<Vector2>();
                data.ValueRW.Click= input.Character.Click.ReadValue<float>();
                data.ValueRW.MousePostion = input.Character.MousePosition.ReadValue<Vector2>();
            }
        }
    }
    
    