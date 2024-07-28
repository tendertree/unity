using Unity.Collections;
using Unity.Entities;
using UnityEngine;
using SimulationSystemGroup = Unity.Entities.SimulationSystemGroup;
using SystemState = Unity.Entities.SystemState;


namespace Quiz.test
{
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    [UpdateBefore(typeof(QuizShowSystem))]
    public partial struct MockPlayerInputSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            // 필요한 쿼리 생성
            state.EntityManager.CreateEntityQuery(new EntityQueryDesc
            {
                All = new ComponentType[] { typeof(Player), typeof(EnterQuizShow) },
                None = new ComponentType[] { typeof(PlayerAnswer) }
            });
        }

        public void OnUpdate(ref SystemState state)
        {
            var ecb = new EntityCommandBuffer(Allocator.Temp);

            var query = state.EntityManager.CreateEntityQuery(new EntityQueryDesc
            {
                All = new ComponentType[] { typeof(Player), typeof(EnterQuizShow) },
                None = new ComponentType[] { typeof(PlayerAnswer) }
            });

            var entities = query.ToEntityArray(Allocator.Temp);

            foreach (var entity in entities)
            {
                // Mock 답변 생성
                var mockAnswer = new PlayerAnswer { AnswerNumber = Random.Range(1, 5) };
                ecb.AddComponent(entity, mockAnswer);
            }

            entities.Dispose();
            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }
    }
}