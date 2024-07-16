using NUnit.Framework;
using Unity.Entities;
using Unity.Collections;
using Unity.Burst;

[TestFixture]
public class DummySystemTests : ECSTestsFixture
{
    private World m_TestWorld;
    private SystemHandle m_SystemHandle;

    [SetUp]
    public override void Setup()
    {
        base.Setup();
        m_TestWorld = new World("Test World");
        var systemGroup = m_TestWorld.GetOrCreateSystemManaged<SimulationSystemGroup>();
        m_SystemHandle = m_TestWorld.CreateSystem<DummySystem>();
        systemGroup.AddSystemToUpdateList(m_SystemHandle);
    }

    [Test]
    public void DummySystem_IncreasesAge()
    {
        // Arrange
        var entity = m_TestWorld.EntityManager.CreateEntity(typeof(DummyTagComponent), typeof(AgeComponent));
        m_TestWorld.EntityManager.SetComponentData(entity, new AgeComponent { Value = 0 });

        // Act
        m_TestWorld.Update();

        // Assert
        var age = m_TestWorld.EntityManager.GetComponentData<AgeComponent>(entity);
        Assert.AreEqual(1, age.Value, "Age should be increased by 1");
    }

    [Test]
    public void DummySystem_IgnoresEntitiesWithoutDummyTag()
    {
        // Arrange
        var entityWithTag = m_TestWorld.EntityManager.CreateEntity(typeof(DummyTagComponent), typeof(AgeComponent));
        var entityWithoutTag = m_TestWorld.EntityManager.CreateEntity(typeof(AgeComponent));

        m_TestWorld.EntityManager.SetComponentData(entityWithTag, new AgeComponent { Value = 0 });
        m_TestWorld.EntityManager.SetComponentData(entityWithoutTag, new AgeComponent { Value = 0 });

        // Act
        m_TestWorld.Update();

        // Assert
        var ageWithTag = m_TestWorld.EntityManager.GetComponentData<AgeComponent>(entityWithTag);
        var ageWithoutTag = m_TestWorld.EntityManager.GetComponentData<AgeComponent>(entityWithoutTag);

        Assert.AreEqual(1, ageWithTag.Value, "Age of entity with DummyTag should be increased");
        Assert.AreEqual(0, ageWithoutTag.Value, "Age of entity without DummyTag should not change");
    }

    [Test]
    public void DummySystem_HandlesMultipleEntities()
    {
        // Arrange
        var entityCount = 100;
        var query = m_TestWorld.EntityManager.CreateEntityQuery(typeof(DummyTagComponent), typeof(AgeComponent));

        var entities = m_TestWorld.EntityManager.CreateEntity(m_TestWorld.EntityManager.CreateArchetype(typeof(DummyTagComponent), typeof(AgeComponent)), entityCount, Allocator.Temp);

        // Act
        m_TestWorld.Update();

        // Assert
        var ages = query.ToComponentDataArray<AgeComponent>(Allocator.Temp);
        foreach (var age in ages)
        {
            Assert.AreEqual(1, age.Value, "All ages should be increased to 1");
        }

        ages.Dispose();
        entities.Dispose();
    }

    [TearDown]
    public override void TearDown()
    {
        m_TestWorld.Dispose();
        base.TearDown();
    }
}

// DummySystem 정의 (ISystem 인터페이스 사용)
public partial struct DummySystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state) { }

    [BurstCompile]
    public void OnDestroy(ref SystemState state) { }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        new DummyJob().ScheduleParallel();
    }
}

// DummyJob 정의
[BurstCompile]
public partial struct DummyJob : IJobEntity
{
    void Execute(ref AgeComponent age, in DummyTagComponent tag)
    {
        age.Value++;
    }
}