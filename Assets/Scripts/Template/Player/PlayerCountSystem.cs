
using Unity.Entities;
using Unity.Collections;

public partial struct PlayerCountSystem : ISystem
{
    private Entity playerCountEntity;

    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<Player>();

        // PlayerCountData 싱글톤 엔티티가 없으면 생성합니다.
        if (!SystemAPI.TryGetSingleton<PlayerCountData>(out _))
        {
            playerCountEntity = state.EntityManager.CreateEntity();
            state.EntityManager.AddComponentData(playerCountEntity, new PlayerCountData());
        }
        else
        {
            // 기존의 싱글톤 엔티티를 가져옵니다.
            playerCountEntity = SystemAPI.GetSingletonEntity<PlayerCountData>();
        }
    }

    public void OnUpdate(ref SystemState state)
    {
        var playerQuery = SystemAPI.QueryBuilder().WithAll<Player>().Build();
        int playerCount = playerQuery.CalculateEntityCount();

        // PlayerCountData를 갱신합니다.
        var playerCountData = new PlayerCountData { Count = playerCount };
        state.EntityManager.SetComponentData(playerCountEntity, playerCountData);
    }
}

public struct PlayerCountData : IComponentData
{
    public int Count;
}
