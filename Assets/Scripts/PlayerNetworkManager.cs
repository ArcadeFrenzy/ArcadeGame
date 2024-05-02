using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using System.Collections.Generic;

public class PlayerNetworkManager : MonoBehaviour
{
    public ushort Port
    {
        get
        {
            return NetworkManager.Singleton.GetComponent<UnityTransport>().ConnectionData.Port;
        }
    }

    public NetworkManager UNM => NetworkManager.Singleton;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public enum NetworkState
    {
        HOST,
        CLIENT
    }

    public NetworkState Connect(PlayerAuthManager.PlayerAuth hostClient)
    {
        if(hostClient == null)
        {
            NetworkManager.Singleton.StartHost();
            Debug.Log("Started server");

            return NetworkState.HOST;
        }
        else
        {
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(hostClient.host, hostClient.port);
            NetworkManager.Singleton.StartClient();

            Debug.Log("Started client");

            return NetworkState.CLIENT;
        }
    }
}
