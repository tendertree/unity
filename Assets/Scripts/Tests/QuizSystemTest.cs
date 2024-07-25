using Unity.Entities;
using NUnit.Framework;
    using System.Collections;
    using System.Collections.Generic;
    using NUnit.Framework;
    using UnityEngine;
    using UnityEngine.TestTools;
    using System.IO;
    using Unity.Collections;
using Quiz;
public class QuizSystemTest : ECSTestsFixture
{
    [SetUp]
    public override void Setup()
    {
        base.Setup();
        m_TestWorld = new World("Test World");
        var systemGroup = m_TestWorld.GetOrCreateSystemManaged<SimulationSystemGroup>();
        systemGroup.AddSystemToUpdateList(m_SystemHandle);
    }

    [TearDown]
    public override void TearDown()
    {
        m_TestWorld.Dispose();
        base.TearDown();
    }

    private World m_TestWorld;
    private SystemHandle m_SystemHandle;

    [Test]
    public void QuizInitTest()
    {
        // Arrange

        // Act

        // Assert
    }
    [Test]
    public void QuizLoadTest()
            {
                // Arrange
                string jsonFilePath = Path.Combine(Application.dataPath, "Scripts", "Tests", "Mock", "WordMock.json");
                string jsonContent = File.ReadAllText(jsonFilePath);
            
                // Act
                List<QuizData> quizDataList = JsonToQuizDataConverter.ConvertJsonToQuizDataList(jsonContent);
            
                // Assert
                Assert.IsNotNull(quizDataList, "Quiz data list should not be null");
                Assert.GreaterOrEqual(quizDataList.Count, 3, "There should be at least 3 quiz items");
            
                QuizData thirdQuiz = quizDataList[2];  // 0-based index, so 3rd item is at index 2
                string expectedMeaning = "은밀한, 몰래하는!";
            
                Assert.AreEqual(expectedMeaning, thirdQuiz.Meaning.ToString(), "The meaning of the third quiz item should be '은밀한, 몰래하는'");
            }

}