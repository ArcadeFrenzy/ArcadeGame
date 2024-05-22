using System.IO;

public sealed class KeepAliveCommand : Command
{
    public KeepAliveCommand() : base(Commands.KEEP_ALIVE)
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
        writer.Write(NetworkManager.Instance.playerId);
    }
}