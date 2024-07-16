using Unity.Entities;
using Unity.Collections;
using Unity.Mathematics;

public struct NameSign : IComponentData
{
    public FixedString32Bytes Name;  // ECS에 최적화된 문자열 타입
    public Entity TextMeshEntity;    // TextMeshPro 객체를 포함하는 엔티티에 대한 참조
    public float3 Offset;            // 텍스트의 오프셋 위치 (필요한 경우)
}