using System.Diagnostics;
using Unity.Entities;
using Unity.Logging;
using Unity.Mathematics;
using UnityEngine;
    public partial class BasicInputDataLogSystem : SystemBase
    {
               public static bool IsEnabled = false;

               protected override void OnCreate()
               {
                    base.OnCreate();
                           Enabled = false;
               }
        protected override void OnUpdate()
        {
            
            foreach (RefRO<BasicInputData> data in SystemAPI.Query<RefRO<BasicInputData>>())
            {
        float2 direction = data.ValueRO.MousePostion;
                   Vector2 directionVector2 = new Vector2(direction.x, direction.y);
       
                   Log.Info($"Direction: {directionVector2}"); 
       
            } 
            }
    }