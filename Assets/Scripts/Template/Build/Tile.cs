using Unity.Entities;
using Unity.Mathematics;
using Unity.Collections;
using System;


public enum TileType
{
    Empty,
    Solid,
    // 기타 타일 타입들...

}
public struct Tile : IComponentData
{
    public TileType Type;
    public float3 Position;

}
public struct TileNeighbors : IComponentData
{
    public TileType Type;
    public float3 Position;
    public FixedList32Bytes<TileType> NeighborUp;
    public FixedList32Bytes<TileType> NeighborDown;
    public FixedList32Bytes<TileType> NeighborNorth;
    public FixedList32Bytes<TileType> NeighborSouth;
    public FixedList32Bytes<TileType> NeighborEast;
    public FixedList32Bytes<TileType> NeighborWest;
}


