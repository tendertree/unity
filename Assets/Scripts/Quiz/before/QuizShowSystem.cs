using Unity.Collections;
using Unity.Entities;
using Unity.Logging;

namespace Quiz
{
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    [UpdateAfter(typeof(QuizInitSystem))]
    public partial struct QuizShowSystem : ISystem
    {
        private Entity quizDataEntity;

        public void OnCreate(ref SystemState state)
        {
            var builder = new EntityQueryBuilder(Allocator.Temp)
                .WithAll<Player, EnterQuizShow, QuizListData, QuizTimer>();
            var query = state.GetEntityQuery(builder);
            state.RequireForUpdate(query);
            Log.Info("[start]▶Quiz_Show_System");
        }

        public void OnUpdate(ref SystemState state)
        {
            var ecb = new EntityCommandBuffer(Allocator.Temp);
            var deltaTime = SystemAPI.Time.DeltaTime;

            foreach (var (player, quizListData, quizTimer, entity) in SystemAPI
                         .Query<RefRO<Player>, RefRW<QuizListData>, RefRW<QuizTimer>>()
                         .WithAll<EnterQuizShow>()
                         .WithEntityAccess())
                if (SystemAPI.IsComponentEnabled<EnterQuizShow>(entity))
                {
                    quizTimer.ValueRW.RemainingTime -= deltaTime;

                    if (quizTimer.ValueRO.RemainingTime <= 0 || SystemAPI.HasComponent<PlayerAnswer>(entity))
                    {
                        if (SystemAPI.HasComponent<PlayerAnswer>(entity))
                        {
                            // PlayerAnswer 컴포넌트의 값을 가져옴
                            var playerAnswer = SystemAPI.GetComponent<PlayerAnswer>(entity);

                            // TODO: 답변 확인 로직
                            // ...

                            // PlayerAnswer 컴포넌트 제거
                            ecb.RemoveComponent<PlayerAnswer>(entity);
                            Log.Info($"Player answered: {playerAnswer}. PlayerAnswer component removed.");
                        }
                        else
                        {
                            // PlayerAnswer 컴포넌트가 없는 경우 (시간 초과 등)
                            Log.Info("No player answer received (timeout).");
                        }

                        //end quiz
                        Log.Info("[found]▶Quiz_Show_System");
                        ecb.SetComponentEnabled<EnterQuizShow>(entity, false);
                        ecb.SetComponentEnabled<EnterQuizResult>(entity, true);
                    }
                }

            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }
    }
}