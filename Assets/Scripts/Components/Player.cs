using Unity.Entities;
namespace Moim
{
    public struct PlayerComponent : IComponentData
    {
        public int playerId;
        public string playerName;
    }
}