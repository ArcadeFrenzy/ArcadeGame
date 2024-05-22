using System.IO;

public sealed class ServerHelloCommand : Command
{
    private int playerId;

    private ServerHelloCommand() : base(Commands.SERVER_HELLO)
    {
    }

    protected override void Decode(BinaryReader reader)
    {
        this.playerId = reader.ReadInt32();
    }

    protected override void Encode(BinaryWriter writer)
    {
        throw new System.NotImplementedException();
    }

    public override void Process()
    {
        NetworkManager.Instance.playerId = this.playerId;
        AuthManager.Instance.LoginCanvasObj.SetActive(false);

        NetworkManager.Instance.NetworkedScene.OnLocalPlayerJoin();
    }
}