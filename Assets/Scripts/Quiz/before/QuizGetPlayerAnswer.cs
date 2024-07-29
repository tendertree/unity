using Input;
using Unity.Collections;
using Unity.Entities;
using EntityQueryBuilder = Unity.Entities.EntityQueryBuilder;
using ISystem = Unity.Entities.ISystem;
using SimulationSystemGroup = Unity.Entities.SimulationSystemGroup;
using SystemAPI = Unity.Entities.SystemAPI;
using SystemState = Unity.Entities.SystemState;

namespace Quiz
{
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    [UpdateAfter(typeof(BasicInputSystem))]
    [UpdateBefore(typeof(QuizResultSystem))]
    public partial struct QuizGetPlayerAnswerSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<BasicInputData>();
            var builder = new EntityQueryBuilder(Allocator.Temp)
                .WithAll<Player, EnterQuizShow>();
            var query = state.GetEntityQuery(builder);
            //      state.RequireForUpdate(query);

            //claer Log.Info("----enter quzi get player input-----");
        }

        public void OnUpdate(ref SystemState state)
        {
            // BasicInputData 싱글톤 가져오기
            var inputData = SystemAPI.GetSingleton<BasicInputData>();
            var ecb = new EntityCommandBuffer(Allocator.Temp);
            foreach (var (tag, _, entity)
                     in SystemAPI.Query<RefRO<Player>, EnabledRefRO<EnterQuizShow>>()
                         .WithDisabled<PlayerAnswer>()
                         .WithEntityAccess())
                if (inputData.Click > 0.1f)
                {
                    //baseed on input, select answer 
                    var answerNumber = 3;


                    // Player 엔티티 찾기 및 PlayerAnswer 활성화 및 업데이트
                    ecb.SetComponentEnabled<PlayerAnswer>(entity, true);
                    ecb.SetComponent(entity, new PlayerAnswer { AnswerNumber = answerNumber });
                }

            // EntityCommandBuffer 실행 및 정리
            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }
    }
}