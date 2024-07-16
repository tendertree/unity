using Unity.Entities;
using Unity.Collections;
namespace PetSystem
{
    // Pet의 기본 정보를 담는 컴포넌트
    public struct Pet : IComponentData
    {
        public Entity Prefab;  // Prefab용 Entity 참조
    }

    // Pet의 이름을 담는 컴포넌트
    public struct PetNameComponent : IComponentData
    {
        public FixedString32Bytes Value;
    }

    // Pet 생성이 완료되었음을 나타내는 태그 컴포넌트
    public struct PetsSpawnedTag : IComponentData { }

    // Prefab을 식별하기 위한 태그 컴포넌트
    public struct PetPrefabTag : IComponentData { }
}