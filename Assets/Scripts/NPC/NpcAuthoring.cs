using Unity.Entities;
using UnityEngine;
using Unity.Mathematics;


 class NpcAuthoring : MonoBehaviour
{
    public GameObject prefabs = null;
    public class Baker : Baker<NpcAuthoring>
    {
        public override void Bake(NpcAuthoring authoring)
        {
            if (authoring.prefabs != null)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new Npc
                {
                    prefab = GetEntity(authoring.prefabs, TransformUsageFlags.Dynamic),
                    //SpawnPosition = float3.zero
                    SpawnPosition = authoring.transform.position,

                });

            }
        }
    }
}

public struct Npc : IComponentData
{
    public Entity prefab;
    public float3 SpawnPosition;

}