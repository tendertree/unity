using Unity.Entities;
using Unity.Mathematics;
public struct PlayerSpawner : IComponentData
{
    public Entity Prefab;
    public float3 SpawnPosition;
}


public struct Player : IComponentData
{
    public int grade;
}



