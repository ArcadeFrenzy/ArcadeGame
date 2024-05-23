using System.IO;

public sealed class GameStartCommand : Command
{
    private int playerNumber;

    private GameStartCommand() : base(Commands.GAME_START)
    {
    }

    public override void Process()
    {
        // TODO: tell game
        GameManager.Instance.OnGameStart();
    }

    protected override void Decode(BinaryReader reader)
    {
        this.playerNumber = reader.ReadInt32();
    }

    protected override void Encode(BinaryWriter writer)
    {
        throw new System.NotImplementedException();
    }
}