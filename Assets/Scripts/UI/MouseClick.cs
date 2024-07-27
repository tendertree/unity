
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;
using UnityEngine;
using Unity.Logging;

// 클릭 가능한 엔티티를 나타내는 컴포넌트
public struct Clickable : IComponentData { }

// 마우스 클릭 시스템
public partial class MouseClickSystem : SystemBase
{
    protected override void OnCreate()
    {
        RequireForUpdate<PhysicsWorldSingleton>();
    }

    protected override void OnUpdate()
    {
        var physicsWorld = SystemAPI.GetSingleton<PhysicsWorldSingleton>();

        if (UnityEngine.Input.GetMouseButtonDown(0)) // 좌클릭 감지
        {
            // 마우스 위치를 월드 좌표로 변환
            UnityEngine.Ray unityRay = Camera.main.ScreenPointToRay(UnityEngine.Input.mousePosition);
            float3 rayStart = unityRay.origin;
            float3 rayEnd = unityRay.origin + unityRay.direction * 100f; // 적당한 거리 설정

            // 레이캐스트 수행
            RaycastInput input = new RaycastInput
            {
                Start = rayStart,
                End = rayEnd,
                Filter = CollisionFilter.Default
            };

            if (physicsWorld.CastRay(input, out Unity.Physics.RaycastHit hit))
            {
                Log.Info("raycast hit ? ");
                Entity hitEntity = hit.Entity;

                if (EntityManager.HasComponent<Clickable>(hitEntity))
                {
                    UnityEngine.Debug.Log($"Clicked entity: {hitEntity}");
                }
            }
        }
    }
}