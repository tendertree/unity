using Unity.Entities;
using Unity.Collections;

public partial struct PlayerCountSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<Player>();
        if (!SystemAPI.TryGetSingleton<PlayerCountData>(out _))
        {
            var entity = state.EntityManager.CreateEntity();
            state.EntityManager.AddComponentData(entity, new PlayerCountData());
        }
    }

    public void OnUpdate(ref SystemState state)
    {
        var playerQuery = SystemAPI.QueryBuilder().WithAll<Player>().Build();
        int playerCount = playerQuery.CalculateEntityCount();

        // 싱글톤 엔티티를 통해 플레이어 수 정보를 저장

        if (SystemAPI.TryGetSingleton<PlayerCountData>(out var playerCountData))
        {
            playerCountData.Count = playerCount;
            SystemAPI.SetSingleton(playerCountData);
        }
        else
        {
            // 만약 PlayerCountData가 없다면 여기서 생성
            var entity = state.EntityManager.CreateEntity();
            state.EntityManager.AddComponentData(entity, new PlayerCountData { Count = playerCount });
        }
    }
}

public struct PlayerCountData : IComponentData
{
    public int Count;
}