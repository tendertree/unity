using Unity.Entities;
using Unity.Collections;
namespace QuizGame
{
    // OX 퀴즈 컴포넌트
    public struct OXQuizComponent : IComponentData
    {
        public FixedString128Bytes Question;
        public bool CorrectAnswer;
    }

    // 단어 퀴즈 컴포넌트
    public struct WordQuizComponent : IComponentData
    {
        public FixedString64Bytes Word;
        public FixedString128Bytes Meaning;
        public FixedString64Bytes WrongAnswer;
    }

    // 퀴즈 타입을 구분하기 위한 태그 컴포넌트
    public struct OXQuizTag : IComponentData { }
    public struct WordQuizTag : IComponentData { }
}