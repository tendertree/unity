using Unity.Collections;
using Unity.Entities;
using Unity.Logging;

namespace Quiz
{
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    [UpdateAfter(typeof(QuizShowSystem))]
    public partial struct QuizResultSystem : ISystem
    {
        private Entity quizDataEntity;

        public void OnCreate(ref SystemState state)
        {
            var builder = new EntityQueryBuilder(Allocator.Temp)
                .WithAll<Player, EnterQuizResult>();
            var query = state.GetEntityQuery(builder);
            state.RequireForUpdate(query);
            Log.Info("[start]▶Quiz_result_System");
        }

        public void OnUpdate(ref SystemState state)
        {
            var ecb = new EntityCommandBuffer(Allocator.Temp);
            var processedAnyEntity = false;

            foreach (var (Player, EnterQuizResult, entity) in SystemAPI
                         .Query<RefRO<Player>, EnabledRefRO<EnterQuizResult>>().WithEntityAccess())
                if (SystemAPI.IsComponentEnabled<EnterQuizResult>(entity))
                    ecb.SetComponentEnabled<EnterQuizResult>(entity, false);

            if (processedAnyEntity) Log.Info("[process]▶Quiz_result_System processed quiz results");
            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }
    }
}