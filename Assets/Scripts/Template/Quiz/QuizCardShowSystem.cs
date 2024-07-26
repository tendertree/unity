using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using EntityCommandBuffer = Unity.Entities.EntityCommandBuffer;
using ISystem = Unity.Entities.ISystem;
using Random = Unity.Mathematics.Random;
using SystemAPI = Unity.Entities.SystemAPI;
using SystemState = Unity.Entities.SystemState;


[BurstCompile]
public partial struct QuizCardShowSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.EntityManager.CreateSingleton(new RandomSingleton
        {
            Seed = (uint)SystemAPI.Time.ElapsedTime
        });
        state.RequireForUpdate<QuizCard>();
        state.RequireForUpdate<Challenger>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var randomSingleton = SystemAPI.GetSingleton<RandomSingleton>();
        var random = Random.CreateFromIndex(randomSingleton.Seed);
        var randomValue = random.NextFloat();

        var ecb = new EntityCommandBuffer(Allocator.Temp);

        foreach (var (quizCard, challenger, entity) in
                 SystemAPI.Query<RefRO<QuizCard>, RefRO<Challenger>>().WithEntityAccess())
        {
            var showMeaning = randomValue < 0.5f;
            var wordToShow = quizCard.ValueRO.Word;
            var answerToShow = showMeaning
                ? quizCard.ValueRO.Answer
                : quizCard.ValueRO.WrongAnswer;

            if (!SystemAPI.HasComponent<PlayerAnswerWait>(entity))
                ecb.AddComponent<PlayerAnswerWait>(entity);
            ecb.SetComponentEnabled<PlayerAnswerWait>(entity, true);
        }

        ecb.Playback(state.EntityManager);
        ecb.Dispose();

        // Update random seed
        randomSingleton.Seed = (uint)SystemAPI.Time.ElapsedTime;
        SystemAPI.SetSingleton(randomSingleton);
    }
}