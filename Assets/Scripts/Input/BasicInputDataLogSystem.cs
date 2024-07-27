using Unity.Entities;
using Unity.Logging;
using UnityEngine;

public partial class BasicInputDataLogSystem : SystemBase
{
    public static bool IsEnabled = false;

    protected override void OnCreate()
    {
        base.OnCreate();
        Enabled = false;
    }

    protected override void OnUpdate()
    {
        if (!IsEnabled) return;

        var inputData = SystemAPI.GetSingleton<BasicInputData>();
        var direction = inputData.MousePostion;
        var directionVector2 = new Vector2(direction.x, direction.y);

        Log.Info($"Direction: {directionVector2}");
    }
}