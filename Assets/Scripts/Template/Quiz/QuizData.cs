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