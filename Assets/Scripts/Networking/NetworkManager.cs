using System;
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
    public NetworkedScene networkedScene;

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
