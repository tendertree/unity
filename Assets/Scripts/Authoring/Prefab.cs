using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.NetCode;
using UnityEngine;
public class Prefix : MonoBehaviour
{
    public GameObject prefabs = null;
}
public struct PrefixData : IComponentData
{
    public Entity prefab;
}
public class PrefixBaker : Baker<Prefix>
{
    public override void Bake(Prefix authoring)
    {
        if (authoring.prefabs != null)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new PrefixData
            {
                prefab = GetEntity(authoring.prefabs, TransformUsageFlags.Dynamic)
            });
        }
    }
}