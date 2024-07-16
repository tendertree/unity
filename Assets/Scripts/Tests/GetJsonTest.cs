using System;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;

public class OXQuiz
{
    public string Question { get; set; }
    public bool CorrectAnswer { get; set; }
}

[TestFixture]
public class JsonHelperTests
{
    private string tempFilePath;

    [SetUp]
    public void Setup()
    {
        // 테스트용 임시 JSON 파일 생성
        tempFilePath = Path.GetTempFileName();
        string jsonContent = @"{
            ""oxQuizzes"": [
                {
                    ""question"": ""가벼운의 뜻은 '무게가 많이 나가는 것'이다."",
                    ""correctAnswer"": false
                },
                {
                    ""question"": ""행복은 생활에서 충분한 만족과 기쁨을 느끼는 상태를 의미한다."",
                    ""correctAnswer"": true
                }
            ]
        }";
        File.WriteAllText(tempFilePath, jsonContent);
    }

    [TearDown]
    public void TearDown()
    {
        // 테스트 후 임시 파일 삭제
        if (File.Exists(tempFilePath))
        {
            File.Delete(tempFilePath);
        }
    }

    [Test]
    public void GetJsonObject_ValidJson_ReturnsCorrectList()
    {
        // Arrange
        string propertyName = "oxQuizzes";

        // Act
        List<OXQuiz> result = JsonHelper.GetJsonObject<OXQuiz>(tempFilePath, propertyName);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(2, result.Count);
        Assert.AreEqual("가벼운의 뜻은 '무게가 많이 나가는 것'이다.", result[0].Question);
        Assert.IsFalse(result[0].CorrectAnswer);
        Assert.AreEqual("행복은 생활에서 충분한 만족과 기쁨을 느끼는 상태를 의미한다.", result[1].Question);
        Assert.IsTrue(result[1].CorrectAnswer);
    }

    [Test]
    public void GetJsonObject_InvalidFilePath_ReturnsNull()
    {
        // Arrange
        string invalidFilePath = "invalid/file/path.json";
        string propertyName = "oxQuizzes";

        // Act
        List<OXQuiz> result = JsonHelper.GetJsonObject<OXQuiz>(invalidFilePath, propertyName);

        // Assert
        Assert.IsNull(result);
    }

    [Test]
    public void GetJsonObject_InvalidPropertyName_ReturnsNull()
    {
        // Arrange
        string invalidPropertyName = "invalidProperty";

        // Act
        List<OXQuiz> result = JsonHelper.GetJsonObject<OXQuiz>(tempFilePath, invalidPropertyName);

        // Assert
        Assert.IsNull(result);
    }
}