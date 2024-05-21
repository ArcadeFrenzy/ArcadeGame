using System.IO;

public sealed class ClientHelloCommand : Command
{
    private readonly string name;

    public ClientHelloCommand(string name) : base(Commands.CLIENT_HELLO)
    {
        this.name = name;
    }

    protected override void Decode(BinaryReader reader)
    {
        throw new System.NotImplementedException();
    }

    protected override void Encode(BinaryWriter writer)
    {
        writer.Write(name);
    }

    public override void Process()
    {
        throw new System.NotImplementedException();
    }
}