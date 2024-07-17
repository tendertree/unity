using Unity.Entities;
using Unity.NetCode;
using UnityEngine;
using Unity.Mathematics;
[GhostComponent(PrefabType = GhostPrefabType.AllPredicted)]
public struct PlayerInputData : IInputComponentData
{
    public float2 move;
    public InputEvent jump;

}
