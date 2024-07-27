using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
<<<<<<< HEAD
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
||||||| parent of 77ef849 (feat: add npc tag)
=======
using Unity.Logging;
using EntityCommandBuffer = Unity.Entities.EntityCommandBuffer;
using ISystem = Unity.Entities.ISystem;
using Random = Unity.Mathematics.Random;
using SystemAPI = Unity.Entities.SystemAPI;
using SystemState = Unity.Entities.SystemState;

/// <summary>
///     퀴즈를 받아서 화면에 제시한다 .
/// </summary>
//TODO: List에서 퀴즈 객체를 받아오는 과정 추가 
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
        Log.Info("check spanNPC trigger");
        //npcSelection spawn
        if (!SystemAPI.HasSingleton<SpawnNpcTrigger>())
        {
            Log.Info("create spanNPC trigger");
            // SpawnNpcTrigger가 없으면 새로 생성
            var triggerEntity = state.EntityManager.CreateEntity();
            state.EntityManager.AddComponentData(triggerEntity, new SpawnNpcTrigger { ShouldSpawn = true });
        }
        else
        {
            Log.Info("modified spanNPC trigger");
            // 이미 존재하면 값을 업데이트
            var trigger = SystemAPI.GetSingletonRW<SpawnNpcTrigger>();
            trigger.ValueRW.ShouldSpawn = true;
        }
>>>>>>> 77ef849 (feat: add npc tag)
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