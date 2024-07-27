using Unity.Burst;
using Unity.Entities;
using Random = Unity.Mathematics.Random;

[BurstCompile]
[UpdateAfter(typeof(QuizCardShowSystem))]
public partial struct QuizCardGetPlayerInputSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<QuizCard>();
        state.RequireForUpdate<Challenger>();
        state.RequireForUpdate<PlayerAnswerWait>();
        state.RequireForUpdate<RandomSingleton>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var randomSingleton = SystemAPI.GetSingleton<RandomSingleton>();
        var random = Random.CreateFromIndex(randomSingleton.Seed);

        var ecbSingleton = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
        var ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);

        new ProcessPlayerInputJob
        {
            Random = random,
            ECB = ecb.AsParallelWriter(),
            ElapsedTime = SystemAPI.Time.ElapsedTime,
            DeltaTime = SystemAPI.Time.DeltaTime
        }.ScheduleParallel();

        // Update random seed
        randomSingleton.Seed = (uint)SystemAPI.Time.ElapsedTime;
        SystemAPI.SetSingleton(randomSingleton);
    }

    [BurstCompile]
    private partial struct ProcessPlayerInputJob : IJobEntity
    {
        public Random Random;
        public EntityCommandBuffer.ParallelWriter ECB;
        public double ElapsedTime;
        public float DeltaTime;

        private void Execute(Entity entity,
            in QuizCard quizCard,
            in Challenger challenger,
            EnabledRefRW<PlayerAnswerWait> playerAnswerWait,
            [EntityIndexInQuery] int sortKey)
        {
            if (!playerAnswerWait.ValueRO)
                return;

            var hasSubmitted = false;
            var playerChoice = false;

            // 여기에 실제 플레이어 입력 처리 로직을 구현합니다.
            // 예: if (Input.GetKeyDown(KeyCode.Space)) { hasSubmitted = true; playerChoice = true; }

            // 10초가 지났는지 확인 (실제 구현에서는 시작 시간을 저장하고 비교해야 합니다)
            if (ElapsedTime % 10 < DeltaTime)
            {
                hasSubmitted = true;
                playerChoice = Random.NextBool(); // 랜덤 선택
            }

            if (hasSubmitted)
            {
                ECB.AddComponent(sortKey, entity, new CurrentPlayerSelectAnswer
                {
                    IsCorrect = playerChoice
                });

                ECB.AddComponent(sortKey, entity, new PlayerAnswer
                {
                    Choice = playerChoice,
                    HasSubmitted = true
                });

                ECB.SetComponentEnabled<PlayerAnswerWait>(sortKey, entity, false);
            }
        }
    }
}