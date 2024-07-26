using NUnit.Framework;
using Unity.Collections;
using Unity.Entities;

namespace Script.Quiz.Tests
{
    public class QuizCardShowSystemTest
    {
        private EntityManager m_Manager;
        private EntityQuery m_Query;
        private SystemHandle m_SystemHandle;
        private Entity m_TestEntity; // 추가된 클래스 레벨 변수
        private World m_TestWorld;

        [SetUp]
        public void SetUp()
        {
            m_TestWorld = new World("Test World");
            m_Manager = m_TestWorld.EntityManager;


            var systemGroup = m_TestWorld.GetOrCreateSystemManaged<SimulationSystemGroup>();
            m_SystemHandle = m_TestWorld.CreateSystem<QuizCardShowSystem>();
            systemGroup.AddSystemToUpdateList(m_SystemHandle);

            m_Query = m_Manager.CreateEntityQuery(typeof(QuizCard), typeof(Challenger), typeof(PlayerAnswerWait));

            m_TestEntity = m_Manager.CreateEntity(typeof(QuizCard), typeof(Challenger));

            m_Manager.SetComponentData(m_TestEntity, new QuizCard
            {
                Word = new FixedString128Bytes("Bus"),
                Answer = new FixedString128Bytes("Transport"),
                WrongAnswer = new FixedString128Bytes("Vehicle"),
                ShowMeaning = true
            });

            m_Manager.SetComponentData(m_TestEntity, new Challenger
            {
                name = new FixedString64Bytes("kyim")
            });
        }

        [TearDown]
        public void TearDown()
        {
            m_TestWorld.Dispose();
        }

        [Test]
        public void Checck_QuizCard_Correctly_Created()
        {
            // Act
            m_TestWorld.Unmanaged.GetUnsafeSystemRef<QuizCardShowSystem>(m_SystemHandle)
                .OnUpdate(ref m_TestWorld.Unmanaged.ResolveSystemStateRef(m_SystemHandle));

            // Assert
            Assert.AreEqual(1, m_Query.CalculateEntityCount(), "Entity should have PlayerAnswerWait component");

            var quizCard = m_Manager.GetComponentData<QuizCard>(m_TestEntity);
            Assert.AreEqual("Bus", quizCard.Word.ToString());
            Assert.AreEqual("Transport", quizCard.Answer.ToString());
            Assert.AreEqual("Vehicle", quizCard.WrongAnswer.ToString());

            Assert.IsTrue(m_Manager.HasComponent<PlayerAnswerWait>(m_TestEntity));
            Assert.IsTrue(m_Manager.IsComponentEnabled<PlayerAnswerWait>(m_TestEntity));

            // Check if ShowMeaning has changed (it should be random)
        }

        [Test]
        public void Player_Anser_Get_Clerarly()
        {
        }
    }
}