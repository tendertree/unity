using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

internal class NpcSelectionAuthoring : MonoBehaviour
{
    public GameObject prefabs;

    public class Baker : Baker<NpcSelectionAuthoring>
    {
        public override void Bake(NpcSelectionAuthoring authoring)
        {
            if (authoring.prefabs != null)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new NpcSelection
                {
                    Prefab = GetEntity(authoring.prefabs, TransformUsageFlags.Dynamic),
                    //SpawnPosition = float3.zero
                    SpawnPosition = authoring.transform.position,
                    Choice = default
                });
            }
        }
    }
}

public struct SelectionBool : IComponentData
{
    public bool
        Choice;
}

public struct NpcSelection : IComponentData
{
    public Entity Prefab;
    public float3 SpawnPosition;
    public bool Choice;
}

public struct SpawnNpcTrigger : IComponentData
{
    public bool ShouldSpawn;
}