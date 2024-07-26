using Unity.Collections;
using Unity.Entities;

public struct PlayerAnswer : IComponentData
{
    public bool Choice; // true면 "뜻"을 선택, false면 "단어"를 선택
    public bool HasSubmitted;
}

public struct CurrentPlayerSelectAnswer : IComponentData
{
    public bool IsCorrect;
}

public struct PlayerAnswerWait : IComponentData, IEnableableComponent
{
}

public struct Challenger : IComponentData
{
    // 여기에 Player 정보를 담는 필드를 추가하세요
    // 예: public int PlayerId;
    public FixedString64Bytes name;
}

//word 지문 , m
public struct QuizCard : IComponentData
{
    public FixedString128Bytes Word;
    public FixedString128Bytes Answer;
    public FixedString128Bytes WrongAnswer;
    public bool ShowMeaning;
}


public struct RandomSingleton : IComponentData
{
    public uint Seed;
}