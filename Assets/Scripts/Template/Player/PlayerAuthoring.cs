using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.NetCode;
using UnityEngine;
using Unity.Transforms;
using Unity.Mathematics;

public class PlayerAuthoring : MonoBehaviour
{
    public GameObject prefabs = null;
    public class Baker : Baker<PlayerAuthoring>
    {
        public override void Bake(PlayerAuthoring authoring)
        {
            if (authoring.prefabs != null)
            {
                var entity = GetEntity(TransformUsageFlags.None);
                AddComponent(entity, new PlayerSpawner
                {
                    Prefab = GetEntity(authoring.prefabs, TransformUsageFlags.Dynamic),
                    //SpawnPosition = float3.zero
                    SpawnPosition = authoring.transform.position,

                });

            }
        }
    }
}