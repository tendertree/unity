using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class PetAuthoring : MonoBehaviour
{
    public GameObject PetPrefab;
    public float3 InitialHomePosition;
    public float InitialEnergy = 100f;
    public float InitialHappiness = 100f;
    public GameObject InitialOwner; // Unity 씬에서 주인 오브젝트를 할당할 수 있게 합니다.

    class Baker : Baker<PetAuthoring>
    {
        public override void Bake(PetAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent(entity, new PetComponent
            {
                HomePosition = authoring.InitialHomePosition,
                Owner = GetEntity(authoring.InitialOwner, TransformUsageFlags.Dynamic),
                Energy = authoring.InitialEnergy,
                Happiness = authoring.InitialHappiness,
                CurrentState = PetState.Idle
            });

            AddComponent(entity, new PetUtilityScores
            {
                IdleScore = 0,
                GoHomeScore = 0,
                FollowOwnerScore = 0
            });

            AddComponent(entity, new PetTargetPosition
            {
                Value = authoring.InitialHomePosition
            });

            // Pet Prefab을 Entity로 변환하고 참조를 저장합니다.
            if (authoring.PetPrefab != null)
            {
                var prefabEntity = GetEntity(authoring.PetPrefab, TransformUsageFlags.Dynamic);
                AddComponent(entity, new PetPrefabReference { Value = prefabEntity });
            }
        }
    }
}

// Pet Prefab Entity 참조를 저장하기 위한 새 컴포넌트
public struct PetPrefabReference : IComponentData
{
    public Entity Value;
}