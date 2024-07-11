using Unity.Entities;
using Unity.Collections;

[UpdateInGroup(typeof(InitializationSystemGroup))]
public partial class GameInitializeSystem : SystemBase
{
    private EntityQuery gameStateQuery;

    protected override void OnCreate()
    {
        gameStateQuery = GetEntityQuery(ComponentType.ReadOnly<GameStateData>());
        //state.RequireForUpdate<Config>(); //load require components
    }

    protected override void OnUpdate()
    {
        if (!gameStateQuery.IsEmpty)
        {
            // 게임이 이미 초기화되었으면 아무 것도 하지 않음
            return;
        }

        // 게임 상태 엔티티 생성
        Entity gameStateEntity = EntityManager.CreateEntity();
        EntityManager.AddComponentData(gameStateEntity, new GameStateData
        {
            CurrentRound = 0,
            TotalRounds = 10,
            IsGameActive = true
        });

        // 플레이어 엔티티 생성 (예: 2명의 플레이어)
        for (int i = 0; i < 2; i++)
        {
            Entity playerEntity = EntityManager.CreateEntity();
            EntityManager.AddComponentData(playerEntity, new PlayerData
            {
                PlayerId = i,
                Score = 0
            });
        }

        // 첫 번째 질문 생성
        CreateNewQuestion();

        // 이 시스템이 다시 실행되지 않도록 비활성화
        Enabled = false;
    }

    private void CreateNewQuestion()
    {
        Entity questionEntity = EntityManager.CreateEntity();
        EntityManager.AddComponentData(questionEntity, new Question
        {
            QuestionText = "What is the capital of France?",
            CorrectAnswerIndex = 1
        });
        EntityManager.AddComponent<CurrentQuestionTag>(questionEntity);

        DynamicBuffer<Choice> choices = EntityManager.AddBuffer<Choice>(questionEntity);
        choices.Add(new Choice { Index = 0 }); // London
        choices.Add(new Choice { Index = 1 }); // Paris
        choices.Add(new Choice { Index = 2 }); // Berlin
        choices.Add(new Choice { Index = 3 }); // Rome
    }
}