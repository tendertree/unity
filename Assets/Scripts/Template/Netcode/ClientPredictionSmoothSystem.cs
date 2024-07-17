using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
[BurstCompile]
[WorldSystemFilter(WorldSystemFilterFlags.ClientSimulation)]
public partial struct ClientPredictionSmoothSystem : ISystem
{
    private static bool enabled = true;
    public void OnCreate(ref SystemState state)
    {
    }
    public void OnUpdate(ref SystemState state)
    {
    }
}