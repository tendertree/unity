using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UIElements;
using Unity.Entities;
using Unity.Collections;
using Unity.Transforms;
using Unity.Logging;
using System;
public class ButtonAction : MonoBehaviour
{
    private EntityManager entityManager;
    private EntityQuery playerCountQuery;
    private Button btn;
    private Button btn_move;
    private Label label;
    // Start is called before the first frame update
    void Start()
    {
        entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        var entity = entityManager.CreateEntity();
        entityManager.AddComponentData(entity, new SystemActivationTag { IsActive = false });

        playerCountQuery = entityManager.CreateEntityQuery(typeof(PlayerCountData));

        var root = GetComponent<UIDocument>().rootVisualElement;
        btn = root.Q<Button>("Button");
        btn_move = root.Q<Button>("Move");
        label = root.Q<Label>("Label");

        // btn.RegisterCallback<ClickEvent>(ButtonClick);
        // btn_move.RegisterCallback<ClickEvent>(MoveObject);
        btn.RegisterCallback<ClickEvent>(ButtonClick, TrickleDown.TrickleDown);
        btn_move.RegisterCallback<ClickEvent>(MoveObject, TrickleDown.TrickleDown);

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void MoveObject(ClickEvent evt)
    {
        Log.Info("am I pushed");

        var activationQuery = entityManager.CreateEntityQuery(typeof(Player));

        try
        {
            var activationEntity = activationQuery.GetSingletonEntity();

            // Player 컴포넌트를 가진 엔티티가 존재함
            var localTransform = entityManager.GetComponentData<LocalTransform>(activationEntity);

            // MovingUpDown 컴포넌트가 이미 있는지 확인
            if (!entityManager.HasComponent<MovingUpDown>(activationEntity))
            {
                entityManager.AddComponentData(activationEntity, new MovingUpDown
                {
                    Amplitude = 1f,  // 원하는 값으로 설정
                    Frequency = 1f,  // 원하는 값으로 설정
                    StartPosition = localTransform.Position,
                    TimeOffset = 0f  // 필요하다면 랜덤 값을 사용할 수 있습니다
                });
                Debug.Log("MovingUpDown component added to Player entity.");
            }
            else
            {
                Debug.Log("Player entity already has MovingUpDown component.");
            }
        }
        catch (InvalidOperationException)
        {
            // Player 컴포넌트를 가진 엔티티가 존재하지 않거나 여러 개임
            Debug.LogWarning("No single entity with Player component found. Skipping MovingUpDown addition.");
        }
        evt.StopPropagation();
    }



    private void ButtonClick(ClickEvent evt)
    {
        evt.StopPropagation();
        var activationQuery = entityManager.CreateEntityQuery(typeof(SystemActivationTag));
        var activationEntity = activationQuery.GetSingletonEntity();
        var activationTag = entityManager.GetComponentData<SystemActivationTag>(activationEntity);
        activationTag.IsActive = !activationTag.IsActive;
        entityManager.SetComponentData(activationEntity, activationTag);
        evt.StopPropagation();
    }
    public bool IsPointerOverButton(Vector2 screenPosition)
    {
        return IsPointOverElement(btn, screenPosition) || IsPointOverElement(btn_move, screenPosition);
    }
    private bool IsPointOverElement(VisualElement element, Vector2 screenPosition)
    {
        // 스크린 좌표를 패널 좌표로 변환
        Vector2 panelPosition = RuntimePanelUtils.ScreenToPanel(element.panel, screenPosition);

        // 요소의 월드 경계를 가져옴
        Rect elementRect = element.worldBound;

        // 패널 좌표가 요소의 경계 내에 있는지 확인
        return elementRect.Contains(panelPosition);
    }
}
