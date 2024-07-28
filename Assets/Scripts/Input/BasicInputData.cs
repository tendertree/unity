using Unity.Entities;
using Unity.Mathematics;

namespace Input
{
    public struct BasicInputData : IComponentData
    {
        public float Click;
        public float2 Direction;
        public float2 MousePostion;
    }
}