using NUnit.Framework;
using Unity.Entities;

namespace Quiz.test
{
    [TestFixture]
    public class QuizShowSystemTests : ECSTestsFixture
    {
        [OneTimeSetUp]
        public void RunBeforeAnyTests()
        {
            base.Setup();
            m_TestWorld = new World("TestWorld");
            m_Manager = m_TestWorld.EntityManager;

            // 필요한 시스템들을 추가
            var simulationSystemGroup = m_TestWorld.GetOrCreateSystemManaged<SimulationSystemGroup>();
            var mockPlayerInputSystem = m_TestWorld.CreateSystem<MockPlayerInputSystem>();
            simulationSystemGroup.AddSystemToUpdateList(mockPlayerInputSystem);

            // 모든 테스트 전에 한 번만 실행되는 코드
            // 예: 전역 리소스 초기화, 데이터베이스 설정 등
        }

        [SetUp]
        public override void Setup()
        {
            // 플레이어 엔티티 생성 및 초기화
            m_PlayerEntity = m_Manager.CreateEntity();
            m_Manager.AddComponentData(m_PlayerEntity, new Player());
            m_Manager.AddComponentData(m_PlayerEntity, new QuizListData
            {
                CurrentQuizIndex = 0,
                TimerPerQuiz = 10f
            });
            m_Manager.AddComponentData(m_PlayerEntity, new QuizTimer { RemainingTime = 10f });
            m_Manager.AddComponent<EnterQuizShow>(m_PlayerEntity);

            var quizBuffer = m_Manager.AddBuffer<QuizItem>(m_PlayerEntity);
            quizBuffer.Add(new QuizItem { Question = "Test Question 1", CorrectAnswer = 2 });
            quizBuffer.Add(new QuizItem { Question = "Test Question 2", CorrectAnswer = 3 });
        }

        [TearDown]
        public override void TearDown()
        {
            m_TestWorld.Dispose();
            base.TearDown();
        }

        private EntityManager m_Manager;
        private Entity m_PlayerEntity;
        private World m_TestWorld;

        [Test]
        public void QuizShowSystem_ProcessesPlayerAnswer()
        {
            // 시스템 업데이트 실행
            m_TestWorld.Update();

            // PlayerAnswer 컴포넌트가 추가되었는지 확인
            Assert.IsTrue(m_Manager.HasComponent<PlayerAnswer>(m_PlayerEntity));

            // QuizShowSystem 실행
            m_TestWorld.GetExistingSystemManaged<SimulationSystemGroup>().Update();

            // PlayerAnswer 컴포넌트가 제거되었는지 확인
            //Assert.IsFalse(m_Manager.HasComponent<PlayerAnswer>(m_PlayerEntity));

            // CurrentQuizIndex가 증가했는지 확인
            var quizListData = m_Manager.GetComponentData<QuizListData>(m_PlayerEntity);
            Assert.AreEqual(1, quizListData.CurrentQuizIndex);
        }
    }
}