using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

namespace TileMap
{
    public enum CellType
    {
        Empty,
        Ground,
        Tree,
        Road,
        Building
    }

    //player가 관리하는 여러개의 tilemap
    public struct TileMapList : IComponentData
    {
        public NativeList<Grid3D> GridData;
    }

    public struct TileMapOrigin : IComponentData
    {
        public float3 Position;
    }

    public struct TileMapSize : IComponentData
    {
        public int x, y, z;
    }

    public struct Grid3D : IBufferElementData
    {
        public NativeArray<CellType> Cells;
        public int3 Dimensions;

        public int GetIndex(int x, int y, int z)
        {
            return x + y * Dimensions.x + z * Dimensions.x * Dimensions.y;
        }
    }

    public struct TileMapOwner : IComponentData
    {
        public Entity OwnerEntity;
    }

    //tag
    public struct EnterTileMapInit : IComponentData
    {
    }

    public struct EnterTileMapPhysicalBuild : IComponentData
    {
    }
}