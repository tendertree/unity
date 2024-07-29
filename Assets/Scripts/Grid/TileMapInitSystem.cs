using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Logging;
using Unity.Mathematics;
using ISystem = Unity.Entities.ISystem;
using SystemState = Unity.Entities.SystemState;

namespace TileMap
{
    [BurstCompile]
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    public partial struct TileMapInitSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<EnterTileMapInit>();
            Log.Info("[start]▶TileMapInitSystem;w!");
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var ecb = new EntityCommandBuffer(Allocator.Temp);

            foreach (var (tileMapList, tileMapSize, entity) in SystemAPI.Query<RefRW<TileMapList>, RefRO<TileMapSize>>()
                         .WithAll<EnterTileMapInit>()
                         .WithEntityAccess())
            {
                InitializeTileMap(ref tileMapList.ValueRW, tileMapSize.ValueRO, ref state, entity, ref ecb);
                ecb.RemoveComponent<EnterTileMapInit>(entity);
            }

            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }

        [BurstCompile]
        private void InitializeTileMap(ref TileMapList tileMap, in TileMapSize size, ref SystemState state,
            Entity entity, ref EntityCommandBuffer ecb)

        {
            var dimensions = new int3(size.x, size.y, size.z);
            var totalCells = dimensions.x * dimensions.y * dimensions.z;

            var grid = new Grid3D
            {
                Cells = new NativeArray<CellType>(totalCells, Allocator.Persistent),
                Dimensions = dimensions
            };

            for (var i = 0; i < totalCells; i++) grid.Cells[i] = CellType.Ground;

            if (!tileMap.GridData.IsCreated) tileMap.GridData = new NativeList<Grid3D>(1, Allocator.Persistent);
            tileMap.GridData.Add(grid);


            //Tile positon 
            var screenCenter = GetScreenCenter();
            var tileMapCenter = new float3(dimensions.x / 2f, dimensions.y / 2f, dimensions.z / 2f);
            var originPosition = screenCenter - tileMapCenter;
            ecb.AddComponent(entity, new TileMapOrigin { Position = originPosition });
            ecb.AddComponent<EnterTileMapPhysicalBuild>(entity);

            // 초기화 완료 후 태그 제거
        }

        private float3 GetScreenCenter()
        {
            // 화면 크기를 가져오는 방법은 프로젝트 설정에 따라 다를 수 있습니다.
            // 여기서는 예시로 임의의 값을 사용합니다.
            var screenWidth = 1920f; // 예시 화면 너비
            var screenHeight = 1080f; // 예시 화면 높이
            return new float3(screenWidth / 2f, screenHeight / 2f, 0f);
        }

        private Entity CreateTileMapForPlayer(EntityManager entityManager, Entity playerEntity)
        {
            var tileMapEntity = entityManager.CreateEntity();
            entityManager.AddComponentData(tileMapEntity,
                new TileMapList { GridData = new NativeList<Grid3D>(1, Allocator.Persistent) });
            entityManager.AddComponentData(tileMapEntity, new TileMapOrigin { Position = float3.zero });
            entityManager.AddComponentData(tileMapEntity, new TileMapSize { x = 10, y = 10, z = 1 }); // 예시 크기
            entityManager.AddComponentData(tileMapEntity, new TileMapOwner { OwnerEntity = playerEntity });

            return tileMapEntity;
        }
    }
}