using System.IO;
using UnityEngine;

public sealed class SetPositionCommand : Command
{
    private int playerId;
    private Vector3 position;

    private SetPositionCommand() : base(Commands.PLAYER_LOBBY_MOVE)
    {
    }

    public SetPositionCommand(Vector3 newPosition) : base(Commands.PLAYER_LOBBY_MOVE)
    {
        this.playerId = NetworkManager.Instance.playerId;
        this.position = newPosition;
    }

    protected override void Decode(BinaryReader reader)
    {
        this.playerId = reader.ReadInt32();

        float x = reader.ReadSingle();
        float y = reader.ReadSingle();
        float z = reader.ReadSingle();

        this.position = new Vector3(x, y, z);
    }

    protected override void Encode(BinaryWriter writer)
    {
        writer.Write(this.playerId);
        writer.Write(position.x);
        writer.Write(position.y);
        writer.Write(position.z);
    }

    public override void Process()
    {
        PlayerManager.Instance.GetPlayer(this.playerId).transform.position = this.position;
    }
}