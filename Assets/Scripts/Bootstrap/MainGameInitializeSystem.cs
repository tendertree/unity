using Unity.Entities;
using Unity.Logging;

[UpdateInGroup(typeof(InitializationSystemGroup))]
public partial class MainGameSceneBootStrapSystem : SystemBase
{
    private EntityQuery _mainSceneQuery;

    protected override void OnCreate()
    {
        base.OnCreate();
        _mainSceneQuery = GetEntityQuery(ComponentType.ReadOnly<MainSceneTag>());
        RequireForUpdate(_mainSceneQuery);
    }

    protected override void OnUpdate()
    {
        if (!_mainSceneQuery.IsEmpty)
        {
            Log.Info("MainSceneCreated");
            CheckUserStatus();
            // 시스템을 한 번만 실행하려면 여기에 Enabled = false; 를 추가
            Enabled = false;
        }
    }

    public void CheckUserStatus()
    {
        Log.Info("Logged in user");
    }
}