using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.NetCode;
using UnityEngine;
public class Prefab : MonoBehaviour
{
    public GameObject unit = null;
    public GameObject player = null;
}
public struct PrefabData : IComponentData
{
    public Entity unit;
    public Entity player;
}
public class PrefabBaker : Baker<Prefab>
{
    public override void Bake(Prefab authoring)
    {
        Entity unitPrefab = default;
        Entity playerPrefab = default;
        if (authoring.unit != null)
        {
            unitPrefab = GetEntity(authoring.unit, TransformUsageFlags.Dynamic);
        }
        if (authoring.player != null)
        {
            playerPrefab = GetEntity(authoring.player, TransformUsageFlags.Dynamic);
        }
        var entity = GetEntity(TransformUsageFlags.Dynamic);
        AddComponent(entity, new PrefabData
        {
            unit = GetEntity(authoring.unit, TransformUsageFlags.Dynamic)
        });
    }
}