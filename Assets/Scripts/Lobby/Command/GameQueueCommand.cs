using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class GameQueueCommand : Command
{
    enum GameQueueStatus : int
    {
        Request = 0,
        Response = 1,
        PlayerQueued = 2,
        PlayerDequeued = 3,
        QueueReady = 4
    }

    private GameQueueStatus status;
    private string gameName;

    private string sceneName;
    private int queuePosition;

    public GameQueueCommand(string gameName) : base(Commands.LOBBY_QUEUE)
    {
        this.gameName = gameName;
    }

    private GameQueueCommand() : base(Commands.LOBBY_QUEUE)
    {
    }

    public override void Process()
    {
        switch(this.status)
        {
            case GameQueueStatus.Response:
                {
                    Debug.Log("Queued at " + queuePosition);
                } break;
            case GameQueueStatus.QueueReady:
                {
                    SceneManager.LoadScene(this.sceneName);
                } break;
            default:
                {
                    Debug.LogWarning($"Invalid status {this.status} for GameQueueCommand provided.");
                } break;
        }
    }

    protected override void Decode(BinaryReader reader)
    {
        this.status = (GameQueueStatus)reader.ReadInt32();

        switch (this.status)
        {
            case GameQueueStatus.Response:
                {
                    this.queuePosition = reader.ReadInt32();
                } break;
            case GameQueueStatus.QueueReady:
                {
                    this.sceneName = reader.ReadString();
                } break;
        }
    }

    protected override void Encode(BinaryWriter writer)
    {
        writer.Write((int)this.status);

        switch (this.status)
        {
            case GameQueueStatus.Request:
                {
                    writer.Write(this.gameName);
                } break;
            default:
                {
                    Debug.LogError($"Invalid status {this.status} for GameQueueCommand encoding provided.");
                } break;
        }
    }
}