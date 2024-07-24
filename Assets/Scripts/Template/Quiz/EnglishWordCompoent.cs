
using Unity.Entities;
using Unity.Collections;

public struct EnglishWordComponent : IComponentData
{
    // 영어 단어
    public FixedString64Bytes Word;
    
    // 단어의 뜻
    public FixedString128Bytes Meaning;
    
    // 오답 (한 개)
    public FixedString64Bytes WrongAnswer;
}

