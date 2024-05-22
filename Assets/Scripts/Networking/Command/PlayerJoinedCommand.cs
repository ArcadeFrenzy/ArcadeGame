using System.IO;
using UnityEngine;

public sealed class PlayerJoinedCommand : Command
{
    private int playerId;
    private string playerName;

    private Vector3 playerPos;

    private PlayerJoinedCommand() : base(Commands.PLAYER_JOIN)
    {
    }

    public override void Process()
    {
        NetworkManager.Instance.NetworkedScene.OnPlayerJoin(this.playerId, this.playerName, this.playerPos);
    }

    protected override void Decode(BinaryReader reader)
    {
        this.playerId = reader.ReadInt32();
        this.playerName = reader.ReadString();

        this.playerPos = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
    }

    protected override void Encode(BinaryWriter writer)
    {
        throw new System.NotImplementedException();
    }
}