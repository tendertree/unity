using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
[Burstcompiler]
[UpdateInGroup(typeof(PredictedSimulationSystemGroup))]
public partial struct PlayerMovementSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {

        var builder = new EntityQueryBuilder(Allocator.Temp);
        builder.WithAny<PlayerData, PlayerInputData, LocalTransform>();
        RequireForUpdate(state.GetEntityQuery(builder));


    }
    public void OnUpdate(ref SystemState state)
    {
        var job = new PlayerMovmentJob
        {
            deltatime = SystemAPI.Time.DeltaTime
        };
        State.Dependency = job.ScheduleParallel(state.Dependency);
    }
}

public partial struct PlayerMovmentJob : IJobEntity
{
    public float deltatime;
    public void Execute(
        PlayerData player,
        PlayerInputData input,
        ref LocalTransfrom transform)
    {
        float3 movement = new float3(input.move.x, 0, input.move.y) * player.speed. * deltatime;
        transform.Position = transform.Translate(movemment).Position;
    }
}