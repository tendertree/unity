using System;
using Unity.Collections;
using Unity.Entities;

[Serializable]
public struct QuizData : IComponentData
{
    public FixedString128Bytes Word;
    public FixedString128Bytes Meaning;
    public FixedString128Bytes WrongAnswer;
}


public struct QuizList : IComponentData
{
    // 이 Entity에 DynamicBuffer<QuizData>가 부착됩니다.
}
public struct QuizState : IComponentData
{
    public float TimeRemaining;
    public bool IsAnswered;
    public bool IsChoiceMade;
    public int ChosenAnswerIndex; // 0: Meaning, 1: WrongAnswer
}

public struct QuizResult : IComponentData
{
    public bool IsCorrect;
}
