using Unity.Entities;
using Unity.Logging;
using Unity.NetCode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Unity.Networking.Transport;

public class ClientConnectionManager : MonoBehaviour
{
    private ushort Port;
    private string Adderess;
    private void DestryoLocalSimulationWorld()
    {
        foreach (var world in World.All)
        {
            world.Dispose();
            break;
        }
    }

    private void StartServer()
    {
        var serverWorld = ClientServerBootstrap.CreateServerWorld("Server");

        var serverEndPoiint = NetworkEndpoint.AnyIpv4.WithPort(Port);
        {
            using var networkDriverQuery =
                serverWorld.EntityManager.CreateEntityQuery(ComponentType.ReadWrite<NetworkStreamDriver>());
            networkDriverQuery.GetSingletonRW<NetworkStreamDriver>().ValueRW.Listen(serverEndPoiint);
        }
    }

    private void StartClient()
    {
        var clientWorld = ClientServerBootstrap.CreateClientWorld("client");
        var connectionEndPoint = NetworkEndpoint.Parse(Adderess, Port);
        {
            using var networkDriverQuery =
                clientWorld.EntityManager.CreateEntityQuery(ComponentType.ReadWrite<NetworkStreamDriver>());
            networkDriverQuery.GetSingletonRW<NetworkStreamDriver>().ValueRW.Connect(clientWorld.EntityManager, connectionEndPoint);
        }
        World.DefaultGameObjectInjectionWorld = clientWorld;
        //TODO: get Player ID form DB
        var player = clientWorld.EntityManager.CreateEntity();
        clientWorld.EntityManager.AddComponentData<PlayerReq>(player, new PlayerReq
        {
            id = "choidae14"
        });
    }
}

