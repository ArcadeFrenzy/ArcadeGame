using System.Collections.Generic;
using System.IO;
using UnityEngine;

public sealed class GameStartCommand : Command
{
    private int playerNumber;

    private struct PlayerInfo
    {
        public int playerId;
        public string playerName;
        public Vector3 playerPos;
    }

    private List<PlayerInfo> players = new List<PlayerInfo>();

    private GameStartCommand() : base(Commands.GAME_START)
    {
    }

    public override void Process()
    {
        foreach(var player in players)
        {
            NetworkManager.Instance.NetworkedScene.OnPlayerJoin(player.playerId, player.playerName, player.playerPos);
        }

        // TODO: tell game
        GameManager.Instance.OnGameStart();
    }

    protected override void Decode(BinaryReader reader)
    {
        this.playerNumber = reader.ReadInt32();

        int playerCount = reader.ReadInt32();

        for(int i = 0; i < playerCount; i++)
        {
            int playerId = reader.ReadInt32();
            string playerName = reader.ReadString();
            Vector3 pos = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());

            players.Add(new PlayerInfo() { playerId = playerId, playerName = playerName, playerPos = pos });
        }
    }

    protected override void Encode(BinaryWriter writer)
    {
        throw new System.NotImplementedException();
    }
}