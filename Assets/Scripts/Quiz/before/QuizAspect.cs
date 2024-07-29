using Unity.Collections;
using Unity.Entities;

namespace Quiz
{
    public readonly partial struct QuizAspect : IAspect
    {
        private readonly RefRW<Player> QuizCard;
    }

    public struct QuizCard : IComponentData
    {
    }

//quiz list item 
    public struct QuizListData : IComponentData
    {
        public DynamicBuffer<QuizItem> Quizzes;
        public float TimerPerQuiz;
        public int CurrentQuizIndex;
        public int PlayerId;
    }

    public struct QuizItem : IBufferElementData
    {
        public FixedString128Bytes Question;
        public int CorrectAnswer;
    }

    public struct QuizTimer : IComponentData
    {
        public float RemainingTime;
    }


    //tag component 

    public struct EnterQuizInit : IComponentData, IEnableableComponent
    {
    }

    public struct EnterQuizShow : IComponentData, IEnableableComponent
    {
    }

    public struct EnterQuizResult : IComponentData, IEnableableComponent
    {
    }
}