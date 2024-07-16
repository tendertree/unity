using UnityEngine;
using Unity.Entities;
using Unity.Collections;
using TMPro;
public class NameSignAuthoring : MonoBehaviour
{
    public string Name;
    public TextMeshPro TextMeshPro;
    public Vector3 Offset;
}

public class NameSignBaker : Baker<NameSignAuthoring>
{
    public override void Bake(NameSignAuthoring authoring)
    {
        var entity = GetEntity(TransformUsageFlags.Dynamic);

        AddComponent(entity, new NameSign
        {
            Name = new FixedString32Bytes(authoring.Name),
            TextMeshEntity = GetEntity(authoring.TextMeshPro, TransformUsageFlags.Dynamic),
            Offset = authoring.Offset
        });
    }
}