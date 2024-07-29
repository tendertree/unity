using TileMap;
using Unity.Collections;
using Unity.Entities;
using Unity.Logging;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.Rendering;
using BoxCollider = Unity.Physics.BoxCollider;
using EntityArchetype = Unity.Entities.EntityArchetype;
using Material = UnityEngine.Material;
using Random = Unity.Mathematics.Random;
using SystemBase = Unity.Entities.SystemBase;

namespace TileMap
{
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    [UpdateAfter(typeof(TileMapInitSystem))]
    public partial class GridMapPhysicalBuildSystem : SystemBase
    {
        private EntityArchetype m_TileArchetype;

        protected override void OnCreate()
        {
            RequireForUpdate<TileMapList>();
            RequireForUpdate<TileMapOrigin>();
            RequireForUpdate<EnterTileMapPhysicalBuild>();
            // Create archetype for tile entities
            m_TileArchetype = EntityManager.CreateArchetype(
                typeof(LocalTransform),
                typeof(LocalToWorld),
                typeof(RenderBounds),
                typeof(PhysicsCollider),
                typeof(PhysicsMass),
                typeof(PhysicsVelocity),
                typeof(MaterialMeshInfo)
            );
            Log.Info("[start]▶GridMapPhysicalBuild_System");
        }

        protected override void OnUpdate()
        {
            Log.Info("Enter tile amp physics build system");
            var ecbSingleton = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
            var ecb = ecbSingleton.CreateCommandBuffer(World.Unmanaged);

            var tileArchetype = m_TileArchetype;

            foreach (var (tileMap, tileOrigin, entity) in SystemAPI.Query<RefRO<TileMapList>, RefRO<TileMapOrigin>>()
                         .WithAll<EnterTileMapPhysicalBuild>()
                         .WithEntityAccess())
            {
                foreach (var grid in tileMap.ValueRO.GridData)
                    for (var x = 0; x < grid.Dimensions.x; x++)
                    for (var y = 0; y < grid.Dimensions.y; y++)
                    for (var z = 0; z < grid.Dimensions.z; z++)
                    {
                        var index = grid.GetIndex(x, y, z);
                        var cellType = grid.Cells[index];
                        if (cellType != CellType.Empty)
                        {
                            var position = tileOrigin.ValueRO.Position + new float3(x, y, z);
                            CreateTileEntity(cellType, position, tileArchetype, ecb);
                        }
                    }

                ecb.RemoveComponent<EnterTileMapPhysicalBuild>(entity);
            }
        }

        private void CreateTileEntity(CellType cellType, float3 position, EntityArchetype archetype,
            EntityCommandBuffer ecb)
        {
            var mesh = LoadTileMeshByCellType(cellType);
            if (mesh == null) return;

            var material = Resources.Load<Material>("Material/Green");
            if (material == null)
            {
                Debug.LogError($"Failed to load material for {cellType}");
                return;
            }

            var entity = ecb.CreateEntity(archetype);

            ecb.SetComponent(entity, new LocalTransform
            {
                Position = position,
                Rotation = quaternion.identity,
                Scale = 1f
            });

            var collisionFilter = new CollisionFilter
            {
                BelongsTo = 1u << 0,
                CollidesWith = ~0u,
                GroupIndex = 0
            };

            var boxGeometry = new BoxGeometry
            {
                Center = float3.zero,
                Orientation = quaternion.identity,
                Size = mesh.bounds.size,
                BevelRadius = 0.0f
            };

            var boxCollider = BoxCollider.Create(boxGeometry, collisionFilter, Unity.Physics.Material.Default);

            ecb.SetComponent(entity, new PhysicsCollider { Value = boxCollider });

            var massProperties = boxCollider.Value.MassProperties;
            ecb.SetComponent(entity, PhysicsMass.CreateKinematic(massProperties));
            ecb.SetComponent(entity, new PhysicsVelocity { Linear = float3.zero, Angular = float3.zero });

            // Add tag for rendering

            ecb.AddComponent(entity, new AddRenderMeshTag
            {
                MeshId = mesh.GetInstanceID(),
                MaterialId = material.GetInstanceID()
            });
        }

        private Mesh LoadTileMeshByCellType(CellType cellType)
        {
            var path = $"Tile/{cellType}";
            var meshes = Resources.LoadAll<Mesh>(path);
            if (meshes == null || meshes.Length == 0)
            {
                Debug.LogError($"No meshes found in the '{path}' folder");
                return null;
            }

            Debug.Log($"Loaded {meshes.Length} meshes for {cellType}");
            return meshes[Random.CreateFromIndex((uint)cellType.GetHashCode()).NextInt(0, meshes.Length)];
        }
    }
}

