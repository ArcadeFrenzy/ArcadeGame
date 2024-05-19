using System;
using System.Linq;
using UnityEngine;

public class GameNetworkManager : MonoBehaviour
{
    public bool shared;
    public string instanceId;
    public int waitForPlayerCount; // player count to wait for before starting

    public bool connectOnStart = false;

    public GameObject NetworkManagerObj;

    public PlayerNetworkManager NetworkManager
    {
        get
        {
            return NetworkManagerObj.GetComponent<PlayerNetworkManager>();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if(connectOnStart)
        {
            this.QueryAndConnect(() => // on success
            {

            }, () => // on error
            {

            });
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void QueryAndConnect(Action onSuccess, Action onError)
    {
        NetworkManager.QueryGames((lobbies) => // on success
        {
            PlayerNetworkManager.GameLobby lobby = null;

            if(shared)
            {
                lobby = lobbies.FirstOrDefault();
            }
            else
            {
                lobby = lobbies.Where(lobby => lobby.PlayerCount < waitForPlayerCount).FirstOrDefault();
            }

            NetworkManager.TryConnect(lobby, (connectedLobby) =>
            {
                this.instanceId = connectedLobby.InstanceId;

                onSuccess();
            }, onError);
        }, onError);
    }
}
