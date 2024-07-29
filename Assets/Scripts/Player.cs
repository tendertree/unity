using Quiz;
using Unity.Entities;

namespace User
{
    public readonly partial struct PlayerAspecet : IAspect
    {
        private readonly RefRW<Player> player;
        private readonly EnabledRefRW<EnterQuizInit> enterQuizInit;
        private readonly EnabledRefRW<EnterQuizShow> enterQuizShow;
        private readonly EnabledRefRW<EnterQuizResult> enterQuizResult;
        private readonly EnabledRefRW<PlayerAnswer> playerAnswer;
    }

    public struct Player : IComponentData
    {
        public int score;
        public int PlayerId;
    }

    public struct PlayerAnswer : IComponentData, IEnableableComponent
    {
        public int AnswerNumber;
    }
}