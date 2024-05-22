using System.IO;

public sealed class PlayerLeftCommand : Command
{
    private int playerId;

    private PlayerLeftCommand() : base(Commands.PLAYER_LEAVE)
    {
    }

    public override void Process()
    {
        NetworkManager.Instance.NetworkedScene.OnPlayerLeave(this.playerId);
    }

    protected override void Decode(BinaryReader reader)
    {
        this.playerId = reader.ReadInt32();
    }

    protected override void Encode(BinaryWriter writer)
    {
        throw new System.NotImplementedException();
    }
}