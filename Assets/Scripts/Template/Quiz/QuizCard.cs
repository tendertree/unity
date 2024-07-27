using Unity.Collections;
using Unity.Entities;

public struct PlayerAnswer : IComponentData
{
    public bool Choice; // player가 현재 선택한 대답 
    public bool HasSubmitted;
}

public struct PlayerQuizSelection : IComponentData
{
    public bool Value; // player가 현재 선택한 대답 
}


//player가 입력을 할때까지 대기 
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
