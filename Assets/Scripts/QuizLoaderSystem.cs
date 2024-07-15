using Unity.Entities;
using Unity.Collections;
using UnityEngine;

namespace QuizGame
{
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    public partial struct QuizLoaderSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            // JSON 파일 경로 설정
            string oxQuizPath = "Data/OXQuiz";
            string wordQuizPath = "Data/WordQuiz";

            // JSON 파일 로드
            NativeArray<OXQuizComponent> oxQuizzes = JsonLoader.LoadOXQuizzes(oxQuizPath);
            NativeArray<WordQuizComponent> wordQuizzes = JsonLoader.LoadWordQuizzes(wordQuizPath);

            // Entity 생성 및 컴포넌트 추가
            EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.Temp);

            for (int i = 0; i < oxQuizzes.Length; i++)
            {
                Entity entity = ecb.CreateEntity();
                ecb.AddComponent(entity, oxQuizzes[i]);
                ecb.AddComponent<OXQuizTag>(entity);
            }

            for (int i = 0; i < wordQuizzes.Length; i++)
            {
                Entity entity = ecb.CreateEntity();
                ecb.AddComponent(entity, wordQuizzes[i]);
                ecb.AddComponent<WordQuizTag>(entity);
            }

            ecb.Playback(state.EntityManager);
            ecb.Dispose();

            // 네이티브 배열 해제
            oxQuizzes.Dispose();
            wordQuizzes.Dispose();
        }

        public void OnUpdate(ref SystemState state)
        {
            // 초기화 이후에는 업데이트할 필요가 없으므로 비워둡니다.
        }

        public void OnDestroy(ref SystemState state)
        {
            // 필요한 경우 정리 작업을 수행합니다.
        }
    }
}