using Unity.Collections;
using Unity.Entities;
using Unity.Logging;

namespace Quiz
{
    //TODO: get quiz data from server or db 
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    public partial struct QuizInitSystem : ISystem
    {
        private Entity quizDataEntity;

        public void OnCreate(ref SystemState state)
        {
            var builder = new EntityQueryBuilder(Allocator.Temp)
                .WithAll<Player, EnterQuizInit>();
            var query = state.GetEntityQuery(builder);
            state.RequireForUpdate(query);
            Log.Info("[start]▶Quiz_Init_System");
        }

        public void OnUpdate(ref SystemState state)
        {
            var ecb = new EntityCommandBuffer(Allocator.Temp);


            foreach (var (player, enterQuizInit, entity) in SystemAPI
                         .Query<RefRO<Player>, EnabledRefRO<EnterQuizInit>>().WithEntityAccess())
                if (SystemAPI.IsComponentEnabled<EnterQuizInit>(entity))
                {
                    var quizListData = new QuizListData
                    {
                        TimerPerQuiz = 10f, // 예시 값, 필요에 따라 조정
                        CurrentQuizIndex = 0,
                        PlayerId = player.ValueRO.PlayerId // Player 컴포넌트에 PlayerId가 있다고 가정
                    };

                    // QuizListData 컴포넌트 추가
                    ecb.AddComponent(entity, quizListData);

                    // QuizItem 버퍼 추가 및 초기화
                    var quizBuffer = ecb.AddBuffer<QuizItem>(entity);
                    // 예시 퀴즈 데이터 추가 (실제로는 서버나 DB에서 가져올 것)
                    quizBuffer.Add(new QuizItem { Question = "What is 2 + 2?", CorrectAnswer = 4 });
                    quizBuffer.Add(new QuizItem { Question = "What is the capital of France?", CorrectAnswer = 2 });
                    quizBuffer.Add(new QuizItem { Question = "seoulg age", CorrectAnswer = 41 });
                    quizBuffer.Add(new QuizItem { Question = "your birth day", CorrectAnswer = 12 });
                    // 추가 퀴즈 항목들...

                    // QuizTimer 컴포넌트 추가 및 초기화
                    ecb.AddComponent(entity, new QuizTimer { RemainingTime = quizListData.TimerPerQuiz });

                    // 다음 단계로 진행
                    ecb.SetComponentEnabled<EnterQuizInit>(entity, false);
                    ecb.SetComponentEnabled<EnterQuizShow>(entity, true);


                }
        }
    }
}