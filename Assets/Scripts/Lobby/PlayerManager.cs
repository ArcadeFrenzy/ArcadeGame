using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;

    private Dictionary<int, Player> players = new Dictionary<int, Player>();

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            return;
        }

        Destroy(gameObject);
    }

    public void AddPlayer(Player player)
    {
        this.players.Add(player.playerId, player);
    }

    public void RemovePlayer(Player player)
    {
        this.players.Remove(player.playerId);
    }

    public Player GetPlayer(int playerId)
    {
        return players[playerId];
    }
}