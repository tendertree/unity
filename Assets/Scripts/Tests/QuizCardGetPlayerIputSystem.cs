using Unity.Entities;
using Unity.Logging;
using Unity.Mathematics;
using Unity.Physics;
using UnityEditor;
using UnityEngine;
using SystemAPI = Unity.Entities.SystemAPI;
using SystemBase = Unity.Entities.SystemBase;

// 수정된 QuizCardGetPlayerAnswer 시스템
[UpdateAfter(typeof(QuizCardShowSystem))]
public partial class QuizCardGetPlayerAnswer : SystemBase
{
    protected override void OnCreate()
    {
        RequireForUpdate<BasicInputData>();
        RequireForUpdate<PhysicsWorldSingleton>();
        RequireForUpdate<PlayerAnswer>(); // PlayerAnswer 싱글톤이 존재해야 함
    }

    protected override void OnUpdate()
    {
        var inputData = SystemAPI.GetSingleton<BasicInputData>();
        var physicsWorld = SystemAPI.GetSingleton<PhysicsWorldSingleton>();
        var playerAnswer = SystemAPI.GetSingletonRW<PlayerAnswer>();

        if (inputData.Click != 0)
        {
            float3 rayStart = Camera.main.transform.position;
            var rayDirection = math.normalize(Camera.main
                .ScreenPointToRay(new Vector3(inputData.MousePostion.x, inputData.MousePostion.y, 0)).direction);
            var rayLength = 100f;
            var raycastInput = new RaycastInput
            {
                Start = rayStart,
                End = rayStart + rayDirection * rayLength,
                Filter = CollisionFilter.Default
            };

            Debug.DrawRay(rayStart, rayDirection * rayLength, Color.red, 1f);
            Log.Info("Active RayCast for Quiz Selection");

            if (physicsWorld.CastRay(raycastInput, out var hit))
            {
                Log.Info($"Ray hit something at {hit.Position}");
                Debug.DrawLine(rayStart, hit.Position, Color.green, 1f);

                var hitEntity = hit.Entity;
                if (EntityManager.HasComponent<Npc>(hitEntity) && EntityManager.HasComponent<Selection>(hitEntity))
                {
                    var selection = EntityManager.GetComponentData<PlayerQuizSelection>(hitEntity);

                    // PlayerAnswer 업데이트
                    playerAnswer.ValueRW.Choice = selection.Value;
                    playerAnswer.ValueRW.HasSubmitted = true;

                    Log.Info($"Player selected NPC. Answer set to: {playerAnswer.ValueRO.Choice}");
                    Debug.DrawLine(hit.Position, hit.Position + new float3(0, 0.5f, 0), Color.blue, 1f);
                }
                else
                {
                    Log.Info("Hit entity is not a selectable NPC");
                }
            }
            else
            {
                Log.Info("Ray did not hit anything");
            }
        }
    }
}