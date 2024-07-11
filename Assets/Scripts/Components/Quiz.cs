
using Unity.Entities;
using Unity.Collections;
namespace Moim
{


    public struct Question : IComponentData
    {
        public FixedString128Bytes QuestionText;
        public int CorrectAnswerIndex;
    }

    public struct Choice : IBufferElementData
    {
        public int Index;
    }

    public struct CurrentQuestionTag : IComponentData { }

    public struct AnswerSubmitted : IComponentData
    {
        public int PlayerId;
        public int AnswerIndex;
    }

    public struct QuizGameState : IComponentData
    {
        public int CurrentRound;
        public int TotalRounds;
        public bool IsGameActive;
    }
}