﻿using Unity.Entities;
using NUnit.Framework;
using Quiz;
public class QuizSystemTest : ECSTestsFixture
{
    [SetUp]
    public override void Setup()
    {
        base.Setup();
        m_TestWorld = new World("Test World");
        var systemGroup = m_TestWorld.GetOrCreateSystemManaged<SimulationSystemGroup>();
        m_SystemHandle = m_TestWorld.CreateSystem<QuizDataLoadSystem>();
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

}