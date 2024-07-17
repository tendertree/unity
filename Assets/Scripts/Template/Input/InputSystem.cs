using Unity.Burst;
using Unity.Collections;
using Unity.NetCode;
using UnityEngine;
using Unity.Entities;

[UpdateInGroup(typeof(GhostInputSystemGroup))]
[WorldSystemFilter(WorldSystemFilterFlags.ClientSimulation)]
partial struct InputSystem : ISystem
{
    private Controls _controls;
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        _controls = new Controls();
        _controls.Enable();

        var query = new EntityQueryBuilder(Allocator.Temp)
                   .WithAny<PlayerInputData>()
                   .Build(ref state);
        state.RequireForUpdate(query);

    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        Vector2 playerMove = _controls.Player.Move.ReadValue<Vector2>();
        foreach (RefRW<PlayerInputData> input in SystemAPI.Query<RefRW<PlayerInputData>>())
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
        _controls.Disable();
        _controls.Dispose();
    }
}
