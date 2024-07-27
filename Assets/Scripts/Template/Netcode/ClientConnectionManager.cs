using Unity.Entities;
using Unity.Logging;
using Unity.NetCode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Unity.Networking.Transport;

public class ClientConnectionManager : MonoBehaviour
{
    [SerializeField] private InputField _addressField;
    [SerializeField] private InputField _portField;
    [SerializeField] private Dropdown _connectionModeDropdown;

    [SerializeField] private Dropdown _teacherChoiceDropdown;

    [SerializeField] private Button _connectButton;

    private ushort Port => ushort.Parse(_portField.text);

    private string Adderess => _addressField.text;

    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }

    // Start is called before the first frame update  
    private void OnEnable()
    {
        _connectionModeDropdown.onValueChanged.AddListener(OnConnectionModeChanged);
        _connectButton.onClick.AddListener(OnButtonConnect);
        OnConnectionModeChanged(_connectionModeDropdown.value);
    }

    private void OnDisable()
    {
        _connectionModeDropdown.onValueChanged.RemoveAllListeners();
        _connectButton.onClick.RemoveAllListeners();
    }

    private void OnConnectionModeChanged(int index)
    {
        string buttonLabel;
        _connectButton.enabled = true;
    }

    private void OnButtonConnect()
    {
        DestryoLocalSimulationWorld();
        SceneManager.LoadScene(1);
        switch (_connectionModeDropdown.value)
        {
            case 0:
                StartServer();
                StartClient();
                break;
            case 1:
                StartServer();
                break;
            case 2:
                StartClient();
                break;
            default:

                Log.Info("Hello, {username}!", "World");
                break;
        }
    }

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
            networkDriverQuery.GetSingletonRW<NetworkStreamDriver>().ValueRW
                .Connect(clientWorld.EntityManager, connectionEndPoint);
        }
        World.DefaultGameObjectInjectionWorld = clientWorld;
    }
}