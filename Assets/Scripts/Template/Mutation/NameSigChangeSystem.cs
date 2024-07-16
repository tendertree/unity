using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using TMPro;
using UnityEngine;
using Unity.Logging;

public partial struct NameSignNameChangeSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.Enabled = false;
    }
    public void OnUpdate(ref SystemState state)
    {
    }
}