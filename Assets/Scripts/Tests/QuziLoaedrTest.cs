using NUnit.Framework;
using Unity.Entities;
using Unity.Collections;
using UnityEngine;
using System.IO;
using QuizGame;



[TestFixture]
public class QuizLoaderSystemTests : ECSTestsFixture
{
    private SystemHandle m_SystemHandle;

    private World m_World;

    [SetUp]
    public override void Setup()
    {
        base.Setup();
        m_World = World.DefaultGameObjectInjectionWorld;

        // QuizLoaderSystem 생성
        m_SystemHandle = m_World.CreateSystem<QuizLoaderSystem>();
        // 테스트용 JSON 파일 생성
        CreateTestJsonFiles();
    }

    [TearDown]
    public override void TearDown()
    {
        // world 제거 
        if (m_World != null && m_World.IsCreated)
        {
            m_World.Dispose();
            m_World = null;
        }

        base.TearDown();
        base.TearDown();
    }

    [Test]
    public void QuizLoaderSystem_LoadsOXQuizzes()
    {
        // 시스템 업데이트 실행

        m_World.Unmanaged.GetUnsafeSystemRef<QuizLoaderSystem>(m_SystemHandle).OnUpdate(ref m_World.Unmanaged.ResolveSystemStateRef(m_SystemHandle));
        // OX 퀴즈 엔티티 쿼리
        var query = m_Manager.CreateEntityQuery(typeof(OXQuizComponent), typeof(OXQuizTag));
        var oxQuizzes = query.ToComponentDataArray<OXQuizComponent>(Allocator.Temp);

        // 테스트 검증
        Assert.AreEqual(1, oxQuizzes.Length, "OX 퀴즈가 정확히 로드되지 않았습니다.");
        Assert.AreEqual("행복은 생활에서 충분한 만족과 기쁨을 느끼는 상태를 의미한다.", oxQuizzes[0].Question.ToString());
        Assert.IsTrue(oxQuizzes[0].CorrectAnswer);

        oxQuizzes.Dispose();
    }

    [Test]
    public void QuizLoaderSystem_LoadsWordQuizzes()
    {
        // 시스템 업데이트 실행
        m_World.Unmanaged.GetUnsafeSystemRef<QuizLoaderSystem>(m_SystemHandle).OnUpdate(ref m_World.Unmanaged.ResolveSystemStateRef(m_SystemHandle));

        // 단어 퀴즈 엔티티 쿼리
        var query = m_World.EntityManager.CreateEntityQuery(typeof(OXQuizComponent), typeof(OXQuizTag));
        var wordQuizzes = query.ToComponentDataArray<WordQuizComponent>(Allocator.Temp);

        // 테스트 검증
        Assert.AreEqual(1, wordQuizzes.Length, "단어 퀴즈가 정확히 로드되지 않았습니다.");
        Assert.AreEqual("가벼운", wordQuizzes[0].Word.ToString());
        Assert.AreEqual("무게가 적거나 부담이 크지 않은", wordQuizzes[0].Meaning.ToString());
        Assert.AreEqual("무거운", wordQuizzes[0].WrongAnswer.ToString());

        wordQuizzes.Dispose();
    }

    private void CreateTestJsonFiles()
    {
        string oxQuizJson = @"[{""question"": ""행복은 생활에서 충분한 만족과 기쁨을 느끼는 상태를 의미한다."", ""correctAnswer"": true}]";
        string wordQuizJson = @"[{""word"": ""가벼운"", ""meaning"": ""무게가 적거나 부담이 크지 않은"", ""wrongAnswer"": ""무거운""}]";

        File.WriteAllText(Application.dataPath + "/Resources/Data/OXQuizTemp.json", oxQuizJson);
        File.WriteAllText(Application.dataPath + "/Resources/Data/WordQuizTemp.json", wordQuizJson);
    }



}