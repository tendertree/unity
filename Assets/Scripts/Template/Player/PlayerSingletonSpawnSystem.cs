using Unity.Entities;
using Unity.Collections;
using UnityEngine;

[UpdateInGroup(typeof(InitializationSystemGroup))]
public partial class PlayerSingletonSpawnSystem : SystemBase
{
    private EntityQuery playerQuery;
    private bool hasCalculatedCount = false;

    protected override void OnCreate()
    {
        playerQuery = GetEntityQuery(typeof(Player));

        // PlayerCountData 싱글톤 생성
        Entity entity = EntityManager.CreateEntity();
        EntityManager.AddComponentData(entity, new PlayerCountData { Count = 0 });
    }

    protected override void OnUpdate()
    {
        if (!hasCalculatedCount)
        {
            int playerCount = playerQuery.CalculateEntityCount();
            Debug.Log($"PlayerCount: {playerCount}");
            hasCalculatedCount = true;
        }
    }


}

