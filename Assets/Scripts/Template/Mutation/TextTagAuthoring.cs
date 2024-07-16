using UnityEngine;
using Unity.Entities;
using TMPro;

public class TextTagAuthoring : MonoBehaviour
{
    public TextMeshPro TextMeshPro;
}

public class TextTagBaker : Baker<TextTagAuthoring>
{
    public override void Bake(TextTagAuthoring authoring)
    {
        var entity = GetEntity(TransformUsageFlags.Dynamic);
        AddComponent(entity, new TextTagComponent());

        // TextMeshPro 컴포넌트를 하이브리드 컴포넌트로 추가
        if (authoring.TextMeshPro != null)
        {
            AddComponentObject(entity, authoring.TextMeshPro);
        }
    }
}