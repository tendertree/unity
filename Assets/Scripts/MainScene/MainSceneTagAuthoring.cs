using Unity.Entities;
using UnityEngine;

namespace DefaultNamespace
{
    public class MainSceneTagAuthoring : MonoBehaviour
    {
        private class MainSceneTagAuthoringBaker : Baker<MainSceneTagAuthoring>
        {
            public override void Bake(MainSceneTagAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.None);
                AddComponent<MainSceneTag>(entity);
            }
        }
    }
}