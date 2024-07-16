using Unity.Entities;
using Unity.Mathematics;

public struct Rotate : IComponentData
{
    public float3 CenterPoint;    // 회전의 중심점
    public float Radius;          // 회전 반경
    public float RotationSpeed;   // 회전 속도 (라디안/초)
    public float CurrentAngle;    // 현재 각도 (라디안)
}