using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{
    public static NetworkManager Instance;

    public string username;
    public string ipAddress;
    public ushort port;

    public int playerId;

    public NetworkedScene NetworkedScene // Each scene must have a networked scene script attached to a game object (can be any game object)
    {
        get
        {
            NetworkedScene scene = FindObjectOfType<NetworkedScene>();

            if(scene == null)
            {
                Debug.LogError("Failed to find a NetworkedScene script instance in the Scene.");
            }

            return scene;
        }
    }

    private Queue<Command> commandsToExecuteOnMainThread = new Queue<Command>();

    private TcpClient client;
    private BinaryWriter writer;
    private BinaryReader reader;

    private void Awake()
    {
        // Only ensure one instance of NetworkManager exists.
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Commands.RegisterAllCommands();

            return;
        }

        Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    public void Connect()
    {
        this.client = new TcpClient(ipAddress, port);
        this.writer = new BinaryWriter(this.client.GetStream());
        this.reader = new BinaryReader(this.client.GetStream());

        new Thread(() =>
        {
            this.ProcessLoop();
        }).Start();

        Debug.Log("Connected");

        InvokeRepeating("SendKeepAlive", 15.0f, 15.0f);
    }

    private void SendKeepAlive()
    {
        SendCommand(new KeepAliveCommand());
    }

    public void SendCommand(Command command)
    {
        command.EncodeCommand(this.writer);
        this.writer.Flush();

        Debug.Log($"Sent client command {command.GetType().Name}.");
    }

    private void ProcessLoop()
    {
        while (this.client.Connected)
        {
            Command command = Command.DecodeCommand(this.reader);

            if (command == null)
            {
                continue;
            }

            commandsToExecuteOnMainThread.Enqueue(command);
        }
    }

    void Update()
    {
        if(commandsToExecuteOnMainThread.Count == 0)
        {
            return;
        }

        Command command = commandsToExecuteOnMainThread.Dequeue();
        command.Process();
    }
}
