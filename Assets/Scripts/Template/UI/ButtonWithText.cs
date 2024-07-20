using UnityEngine;
using UnityEngine.UIElements;
using Unity.Entities;
using Unity.Collections;

public class ButtonAction : MonoBehaviour
{
    private EntityManager entityManager;
    private EntityQuery playerCountQuery;
    private Button btn;
    private Label label;

    void Start()
    {
        entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        var entity = entityManager.CreateEntity();
        entityManager.AddComponentData(entity, new SystemActivationTag { IsActive = false });

        playerCountQuery = entityManager.CreateEntityQuery(typeof(PlayerCountData));

        var root = GetComponent<UIDocument>().rootVisualElement;
        btn = root.Q<Button>("Button");
        label = root.Q<Label>("Label");
        btn.RegisterCallback<ClickEvent>(ButtonClick);

        // 초기화 시 PlayerCountData 정리
        CleanupPlayerCountData();
    }

    private void ButtonClick(ClickEvent evt)
    {
        var activationQuery = entityManager.CreateEntityQuery(typeof(SystemActivationTag));
        var activationEntity = activationQuery.GetSingletonEntity();
        var activationTag = entityManager.GetComponentData<SystemActivationTag>(activationEntity);
        activationTag.IsActive = !activationTag.IsActive;
        entityManager.SetComponentData(activationEntity, activationTag);
    }

    void Update()
    {
        UpdatePlayerCountDisplay();
    }

    private void CleanupPlayerCountData()
    {
        var entities = playerCountQuery.ToEntityArray(Allocator.Temp);
        if (entities.Length > 1)
        {
            Debug.LogWarning($"Found {entities.Length} PlayerCountData entities. Cleaning up...");
            for (int i = 1; i < entities.Length; i++)
            {
                entityManager.DestroyEntity(entities[i]);
            }
        }
        else if (entities.Length == 0)
        {
            Debug.Log("No PlayerCountData found. Creating one...");
            var newEntity = entityManager.CreateEntity();
            entityManager.AddComponentData(newEntity, new PlayerCountData { Count = 0 });
        }
        entities.Dispose();
    }

    private void UpdatePlayerCountDisplay()
    {
        if (playerCountQuery.TryGetSingleton<PlayerCountData>(out var playerCountData))
        {
            label.text = $"Player Count: {playerCountData.Count}";
        }
        else
        {
            label.text = "Player Count: N/A";
            Debug.LogWarning("PlayerCountData not found");
        }
    }
}


/* World world = World.DefaultGameObjectInjectionWorld;
        EntityManager entityManager = world.EntityManager;
        entityManager.CompleteAllTrackedJobs();
        EntityQuery shipsQuery = new EntityQueryBuilder(Allocator.Temp).WithAll<Ship>().Build(entityManager);

        m_ShipCountLabel.text = $"{shipsQuery.CalculateEntityCount()}";
 */