using QuizGame;
using System.IO;
using UnityEngine;
using Unity.Collections;
using Unity.Entities;

public class JsonLoader : MonoBehaviour
{
    public static NativeArray<OXQuizComponent> LoadOXQuizzes(string path)
    {

        string json = File.ReadAllText(path);
        var quizzes = JsonUtility.FromJson<OXQuizData[]>(json);
        //var quizzes = JsonConvert.DeserializeObject<OXQuizData[]>(json);
        var nativeArray = new NativeArray<OXQuizComponent>(quizzes.Length, Allocator.Persistent);

        for (int i = 0; i < quizzes.Length; i++)
        {
            nativeArray[i] = new OXQuizComponent
            {
                Question = new FixedString128Bytes(quizzes[i].question),
                CorrectAnswer = quizzes[i].correctAnswer
            };
        }

        return nativeArray;
    }

    public static NativeArray<WordQuizComponent> LoadWordQuizzes(string path)
    {
        string json = File.ReadAllText(path);
        var quizzes = JsonUtility.FromJson<WordQuizData[]>(json);
        var nativeArray = new NativeArray<WordQuizComponent>(quizzes.Length, Allocator.Persistent);

        for (int i = 0; i < quizzes.Length; i++)
        {
            nativeArray[i] = new WordQuizComponent
            {
                Word = new FixedString64Bytes(quizzes[i].word),
                Meaning = new FixedString128Bytes(quizzes[i].meaning),
                WrongAnswer = new FixedString64Bytes(quizzes[i].wrongAnswer)
            };
        }

        return nativeArray;
    }
}

// JSON 데이터 구조체
[System.Serializable]
public struct OXQuizData
{
    public string question;
    public bool correctAnswer;
}

[System.Serializable]
public struct WordQuizData
{
    public string word;
    public string meaning;
    public string wrongAnswer;
}