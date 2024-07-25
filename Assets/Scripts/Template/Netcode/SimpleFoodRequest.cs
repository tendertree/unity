using Unity.Entities;
using Unity.NetCode;
using Unity.Collections;
public struct SimpleFoodRequest : IRpcCommand
{
    public FixedString64Bytes value;
}