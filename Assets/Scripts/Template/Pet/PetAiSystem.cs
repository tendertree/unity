using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Burst;

[BurstCompile]
[UpdateInGroup(typeof(SimulationSystemGroup))]
public partial struct PetAISystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<PetComponent>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        // Utility 점수 계산
        foreach (var (pet, scores) in SystemAPI.Query<RefRW<PetComponent>, RefRW<PetUtilityScores>>())
        {
            CalculateUtilityScores(ref pet.ValueRW, ref scores.ValueRW);
        }

        // 최고 점수의 행동 선택 및 실행
        foreach (var (pet, scores, targetPos) in
            SystemAPI.Query<RefRW<PetComponent>, RefRO<PetUtilityScores>, RefRW<PetTargetPosition>>())
        {
            ChooseAndExecuteAction(ref pet.ValueRW, scores.ValueRO, ref targetPos.ValueRW);
        }

        // 이동 처리
        foreach (var (pet, transform, targetPos) in
            SystemAPI.Query<RefRW<PetComponent>, RefRW<LocalTransform>, RefRO<PetTargetPosition>>())
        {
            MoveTowardsTarget(ref pet.ValueRW, ref transform.ValueRW, targetPos.ValueRO);
        }
    }

    [BurstCompile]
    private void CalculateUtilityScores(ref PetComponent pet, ref PetUtilityScores scores)
    {
        // 여기에 각 행동의 유틸리티 점수 계산 로직을 구현
        scores.IdleScore = CalculateIdleScore(pet);
        scores.GoHomeScore = CalculateGoHomeScore(pet);
        scores.FollowOwnerScore = CalculateFollowOwnerScore(pet);
    }

    [BurstCompile]
    private void ChooseAndExecuteAction(ref PetComponent pet, in PetUtilityScores scores, ref PetTargetPosition targetPos)
    {
        // 최고 점수의 행동 선택
        if (scores.IdleScore >= scores.GoHomeScore && scores.IdleScore >= scores.FollowOwnerScore)
        {
            pet.CurrentState = PetState.Idle;
            // Idle 상태에서는 현재 위치를 유지
            targetPos.Value = pet.HomePosition; // 또는 현재 위치
        }
        else if (scores.GoHomeScore >= scores.FollowOwnerScore)
        {
            pet.CurrentState = PetState.GoingHome;
            targetPos.Value = pet.HomePosition;
        }
        else
        {
            pet.CurrentState = PetState.FollowingOwner;
            // 여기서는 주인의 위치를 가져와야 합니다. 예를 들어:
            // targetPos.Value = GetOwnerPosition(pet.Owner);
        }
    }

    [BurstCompile]
    private void MoveTowardsTarget(ref PetComponent pet, ref LocalTransform transform, in PetTargetPosition targetPos)
    {
        float3 direction = targetPos.Value - transform.Position;
        float distance = math.length(direction);

        if (distance > 0.1f) // 일정 거리 이상일 때만 이동
        {
            float3 movement = math.normalize(direction) * math.min(distance, 0.1f); // 이동 속도 조절
            transform.Position += movement;
        }
    }

    // Utility 점수 계산을 위한 보조 메서드들
    private float CalculateIdleScore(in PetComponent pet)
    {
        // Idle 상태의 점수 계산 로직
        return 0f; // 임시 반환값
    }

    private float CalculateGoHomeScore(in PetComponent pet)
    {
        // 집으로 가기 상태의 점수 계산 로직
        return 0f; // 임시 반환값
    }

    private float CalculateFollowOwnerScore(in PetComponent pet)
    {
        // 주인 따라가기 상태의 점수 계산 로직
        return 0f; // 임시 반환값
    }
}