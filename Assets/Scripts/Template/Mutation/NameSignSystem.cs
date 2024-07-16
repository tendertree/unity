using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using TMPro;
using UnityEngine;
using Unity.Logging;
[UpdateInGroup(typeof(SimulationSystemGroup))]
public partial struct NameSignSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        // NameSign 컴포넌트가 있는 엔티티가 존재할 때만 이 시스템이 실행되도록 합니다.
        state.RequireForUpdate<NameSign>();
    }

    public void OnDestroy(ref SystemState state)
    {
    }

    public void OnUpdate(ref SystemState state)
    {

        foreach (var (nameSign, transform) in SystemAPI.Query<RefRO<NameSign>, RefRO<LocalTransform>>().WithAll<NameSign>())
        {
            if (state.EntityManager.HasComponent<TextMeshPro>(nameSign.ValueRO.TextMeshEntity))
            {
                var textMesh = state.EntityManager.GetComponentObject<TextMeshPro>(nameSign.ValueRO.TextMeshEntity);

                // 텍스트를 업데이트합니다.
                //textMesh.text = nameSign.ValueRO.Name.ToString();
                textMesh.text = "hello";

                // TextMeshPro 오브젝트의 위치를 엔티티의 머리 위로 조정합니다.
                float3 position = transform.ValueRO.Position + nameSign.ValueRO.Offset;
                textMesh.transform.position = position;

                // 텍스트가 항상 카메라를 향하도록 합니다 (선택사항).
                if (Camera.main != null)
                {
                    textMesh.transform.rotation = Quaternion.LookRotation(
                        textMesh.transform.position - Camera.main.transform.position
                    );
                }
                Log.Info("Text Mesh text is.{ textMesh.text}!");
            }
        }
        Log.Info("Text Mesh Updated");
    }
}