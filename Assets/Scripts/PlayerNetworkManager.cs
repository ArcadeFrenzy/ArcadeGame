using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using System.Collections.Generic;

public class PlayerNetworkManager : MonoBehaviour
{
    public string Host
    {
        get
        {
            return NetworkManager.Singleton.GetComponent<UnityTransport>().ConnectionData.ListenEndPoint.Address;
        }
    }

    public ushort Port
    {
        get
        {
            return NetworkManager.Singleton.GetComponent<UnityTransport>().ConnectionData.ListenEndPoint.Port;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Connect(List<PlayerAuthManager.PlayerAuth> clients)
    {
        if(clients.Count == 0) 
        {
            NetworkManager.Singleton.StartHost();
        }
        else
        {
            var hostClient = clients[0];

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(hostClient.host, hostClient.port);
            NetworkManager.Singleton.StartClient();
        }
    }
}
