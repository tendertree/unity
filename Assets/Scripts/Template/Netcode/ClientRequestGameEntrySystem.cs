using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.NetCode;
using UnityEngine.UIElements.Experimental;

namespace DefaultNamespace
{
    [WorldSystemFilter((WorldSystemFilterFlags.ClientSimulation | WorldSystemFilterFlags.ThinClientSimulation))]
    public partial struct ClientRequestGameEntrySystem : ISystem
    {
        private EntityQuery _pendingNetworkIdQuery;
        public void OnCreate(ref SystemState state)
        {
            //initialize bootstrap query
            var builder = new EntityQueryBuilder(Allocator.Temp).WithAll<NetworkId>().WithNone<NetworkStreamInGame>();
            _pendingNetworkIdQuery = state.GetEntityQuery(builder);
            state.RequireForUpdate(_pendingNetworkIdQuery);
            state.RequireForUpdate<PlayerReq>();

        }
        //send message to Server
        public void OnUpdate(ref SystemState state)
        {

            var player = SystemAPI.GetSingleton<PlayerReq>().id;
            var ecb = new EntityCommandBuffer(Allocator.Temp);
            var pendingNetworkIds = _pendingNetworkIdQuery.ToEntityArray(Allocator.Temp);
            //bootstrap some component for client who first connect, 
            foreach (var pendingNetworkId in pendingNetworkIds)
            {
                ecb.AddComponent<NetworkStreamInGame>(pendingNetworkId);
                //send message to Server 
                var playerEntity = ecb.CreateEntity();
                //혹은 player id를 바탕으로 db에서 데이터를 불러와서 넣기 
                ecb.AddComponent(playerEntity, new SimpleFoodRequest { value = "kimch" });
                ecb.AddComponent(playerEntity, new SendRpcCommandRequest { TargetConnection = pendingNetworkId });
            }
            ecb.Playback(state.EntityManager);
        }

    }
}