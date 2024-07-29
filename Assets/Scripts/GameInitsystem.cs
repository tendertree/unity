using Quiz;
using TileMap;
using Unity.Entities;
using Unity.Logging;
using Unity.Mathematics;
using User;
using Entity = Unity.Entities.Entity;
using InitializationSystemGroup = Unity.Entities.InitializationSystemGroup;
using ISystem = Unity.Entities.ISystem;
using SystemState = Unity.Entities.SystemState;

namespace Main
{
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    public partial struct GameInitsystem : ISystem
    {
        private Entity quizDataEntity;

        public void OnCreate(ref SystemState state)
        {
            state.Enabled = true;
        }

        public void OnUpdate(ref SystemState state)
        {
            var playerEntity = state.EntityManager.CreateEntity();

            // Player 컴포넌트 추가
            state.EntityManager.AddComponent<Player>(playerEntity);

            // EnterQuizInit 컴포넌트 추가 및 활성화
            state.EntityManager.AddComponent<EnterQuizInit>(playerEntity);
            state.EntityManager.SetComponentEnabled<EnterQuizInit>(playerEntity, true);

            // EnterQuizShow 컴포넌트 추가 (초기에는 비활성화 상태로)
            state.EntityManager.AddComponent<EnterQuizShow>(playerEntity);
            state.EntityManager.SetComponentEnabled<EnterQuizShow>(playerEntity, false);

            // EnterQuizResult 컴포넌트 추가 (초기에는 비활성화 상태로)
            state.EntityManager.AddComponent<EnterQuizResult>(playerEntity);
            state.EntityManager.SetComponentEnabled<EnterQuizResult>(playerEntity, false);

            state.EntityManager.AddComponent<PlayerAnswer>(playerEntity);
            state.EntityManager.SetComponentEnabled<PlayerAnswer>(playerEntity, false);


            //tilemap Test 
            state.EntityManager.AddComponent<EnterTileMapInit>(playerEntity);
            Log.Info($"Player entity created with ID: {playerEntity.Index}. All necessary components added.");


            // TileMap System을 위한 setup
            var tileMapEntity = state.EntityManager.CreateEntity();
            state.EntityManager.AddComponentData(tileMapEntity, new TileMapList());
            state.EntityManager.AddComponentData(tileMapEntity, new TileMapSize { x = 10, y = 10, z = 1 }); // 예시 크기
            state.EntityManager.AddComponentData(tileMapEntity, new TileMapOrigin { Position = new float3(0f) });
            state.EntityManager.AddComponentData(tileMapEntity, new EnterTileMapInit());


            state.Enabled = false;
        }
    }
}