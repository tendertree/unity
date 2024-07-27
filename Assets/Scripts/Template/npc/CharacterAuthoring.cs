using Unity.Entities;
using UnityEngine;

// float2를 사용하기 위해 필요
public struct CharacterData : IComponentData
{
    public Entity prefab;
    public float speed;
}

public class CharacterAuthoring : MonoBehaviour
{
    public GameObject prefabs;
    public float speed = 2f;

    private class CharacterBaker : Baker<CharacterAuthoring>
    {
        public override void Bake(CharacterAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new CharacterData
            {
                prefab = GetEntity(authoring.prefabs, TransformUsageFlags.Dynamic),
                speed = authoring.speed
            });
        }
    }
}