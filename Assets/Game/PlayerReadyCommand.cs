using System.IO;

public sealed class PlayerReadyCommand : Command
{
    public PlayerReadyCommand() : base(Commands.PLAYER_READY)
    {
    }

    public override void Process()
    {
        throw new System.NotImplementedException();
    }

    protected override void Decode(BinaryReader reader)
    {
        throw new System.NotImplementedException();
    }

    protected override void Encode(BinaryWriter writer)
    {
    }
}