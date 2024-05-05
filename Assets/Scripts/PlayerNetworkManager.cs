using kcp2k;
using Mirror;
using Newtonsoft.Json;
using UnityEngine;
using static PlayerAuthManager;
using UnityEngine.Networking;
using System.Collections;

public class PlayerNetworkManager : NetworkManager
{
    public ushort Port
    {
        get
        {
            return ((KcpTransport)this.transport).port;
        }
    }

    public enum NetworkState
    {
        HOST,
        CLIENT
    }

    public static string Username;

    public NetworkState Connect(PlayerAuthManager.PlayerAuth hostClient, string username)
    {
        Username = username;

        if(hostClient == null)
        {
            NetworkManager.singleton.StartHost();
            Debug.Log("Started server");

            return NetworkState.HOST;
        }
        else
        {
            NetworkManager.singleton.networkAddress = hostClient.host;
            ((KcpTransport)this.transport).port = hostClient.port;
            this.StartClient();
            Debug.Log($"Started client {hostClient.host} {hostClient.port}");

            return NetworkState.CLIENT;
        }
    }

    public override void OnClientDisconnect()
    {
        StartCoroutine(this.Host(null));
    }

    IEnumerator Host(PlayerAuth hostClient)
    {
        if(this.isNetworkActive)
        {
            // If we are still connected as a client, wait until the cleanup process is finished.
            yield return null;
        }

        var str = JsonUtility.ToJson(new PlayerAuth()
        {
            host = "",
            port = this.Port,
            username = Username,
        }, false);

        switch (this.Connect(hostClient, Username))
        {
            case PlayerNetworkManager.NetworkState.HOST:
                {
                    // We are the host.

                    using (UnityWebRequest www2 = UnityWebRequest.Post($"http://{BACKEND_IP}:{BACKEND_PORT}/host", str, "application/json"))
                    {
                        yield return www2.SendWebRequest();

                        switch (www2.result)
                        {
                            case UnityWebRequest.Result.Success:
                                {
                                    Debug.Log("Registered with backend as host.");
                                    break;
                                }
                            default:
                                {
                                    Debug.LogError("Failed to register with backend as host.");
                                    break;
                                }
                        }
                    }
                    break;
                }
            case PlayerNetworkManager.NetworkState.CLIENT:
                {
                    // Do nothing, we connected to the server.
                    break;
                }
        }
    }

    public IEnumerator Connect(ConnectionRequest connectionRequest, string username)
    {
        Username = username;

        using (UnityWebRequest www = UnityWebRequest.Get($"http://{BACKEND_IP}:{BACKEND_PORT}/connect"))
        {
            yield return www.SendWebRequest();

            switch (www.result)
            {
                case UnityWebRequest.Result.Success:
                    {
                        PlayerAuth hostClient = JsonConvert.DeserializeObject<PlayerAuth>(www.downloadHandler.text);

                        yield return Host(hostClient);

                        connectionRequest.Success = true;
                        break;
                    }
                default:
                    {
                        // Failed to connect.
                        break;
                    }
            }
        }
    }
}