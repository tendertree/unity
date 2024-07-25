using Unity.Burst;
using Unity.Entities;

namespace Quiz
{
    [BurstCompile]
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    public partial struct QuizDataLoadSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<QuizList>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            foreach (var (quizBuffer, entity) in SystemAPI.Query<DynamicBuffer<QuizData>>().WithAll<QuizList>()
                         .WithEntityAccess())
            {
                // 데이터 로드 로직 (예: 파일에서 읽기, 데이터베이스에서 가져오기 등)
                // quizBuffer.Add(new QuizData { ... });
            }
        }
    }
}