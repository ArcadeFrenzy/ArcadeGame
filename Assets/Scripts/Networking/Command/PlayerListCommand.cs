using System.Collections.Generic;
using System.IO;
using UnityEngine;

public sealed class PlayerListCommand : Command
{
    private class PlayerEntry
    {
        public int playerId;
        public string playerName;

        public Vector3 playerPos;
    }

    private PlayerEntry[] entries;

    private PlayerListCommand() : base(Commands.PLAYER_LIST)
    {
    }

    public override void Process()
    {
        foreach(var player in this.entries)
        {
            NetworkManager.Instance.NetworkedScene.OnPlayerJoin(player.playerId, player.playerName, player.playerPos);
        }
    }

    protected override void Decode(BinaryReader reader)
    {
        int playerCount = reader.ReadInt32();
        this.entries = new PlayerEntry[playerCount];

        for(int i = 0; i < playerCount; i++)
        {
            int playerId = reader.ReadInt32();
            string playerName = reader.ReadString();

            Vector3 playerPos = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());

            this.entries[i] = new PlayerEntry
            { 
                playerId = playerId,
                playerName = playerName,
                playerPos = playerPos
            };
        }
    }

    protected override void Encode(BinaryWriter writer)
    {
        throw new System.NotImplementedException();
    }
}