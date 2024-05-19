using kcp2k;
using Mirror;
using Newtonsoft.Json;
using UnityEngine;
using static PlayerAuthManager;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.SceneManagement;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class PlayerNetworkManager : NetworkManager
{
    public static string Username = "";
    private NetworkManagerConnectionInfo currentConnInfo = null;

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

    public class ConnectionInfo
    {
        public string Host;
        public ushort Port;

        public ConnectionInfo(string host, ushort port)
        {
            this.Host = host;
            this.Port = port;
        }
    }

    private class NetworkManagerConnectionInfo
    {
        public GameLobby GameLobby;
        public Action<GameLobby> OnConnect;

        public bool Connected = false;
        public bool Host = false;

        public NetworkManagerConnectionInfo(GameLobby gameLobby, Action<GameLobby> onSuccess)
        {
            this.GameLobby = gameLobby;
            this.OnConnect = onSuccess;
        }
    }

    public override void OnClientConnect()
    {
        base.OnClientConnect();

        currentConnInfo.Connected = true;

        if (currentConnInfo.OnConnect != null)
        {
            currentConnInfo.OnConnect(currentConnInfo.GameLobby);
        }
    }

    public override void OnClientDisconnect() // If the client loses connection, continue hosting.
    {
        if(currentConnInfo == null)
        {
            return;
        }

        if (!currentConnInfo.Connected)
        {
            Debug.Log("Failed to connect, hosting.");
            this.Host();
        }
        else
        {
            Debug.LogError("TODO: Try reconnect.");
            this.Host();
        }
    }

    // Networking API

    public void TryConnect(GameLobby lobby, Action<GameLobby> onSuccess, Action onFailure)
    {
        ConnectionInfo info = lobby?.ConnInfo;

        if (info != null)
        {
            if (currentConnInfo != null && currentConnInfo.Connected)
            {
                // TODO: Check if connected, disconnect if needed
            }

            currentConnInfo = new NetworkManagerConnectionInfo(lobby, onSuccess);

            this.networkAddress = info.Host;
            ((KcpTransport)this.transport).port = info.Port;

            this.StartClient();
            Debug.Log($"Started client {info.Host} {info.Port}");
        }
        else
        {
            currentConnInfo = new NetworkManagerConnectionInfo(new GameLobby(new ConnectionInfo("", this.Port), SceneManager.GetActiveScene().name, null, 1), onSuccess);
            currentConnInfo.Host = true;

            this.Host();
        }
    }

    public void Host()
    {
        if (this.isNetworkActive)
        {
            // If we are still connected as a client, wait until the cleanup process is finished.
            if (this.currentConnInfo != null)
            {
                if (this.currentConnInfo.Host)
                {
                    this.StopHost();
                }
                else
                {
                    this.StopClient();
                }

                Task.Run(() =>
                {
                    Task.Delay(3000);
                    this.Host();
                });
            }

            return;
        }

        var str = JsonConvert.SerializeObject(this.currentConnInfo.GameLobby, Formatting.None);

        StartCoroutine(SendWebRequest("host", (jsonStr) =>
        {
            Debug.Log("Registered with backend as host. " + jsonStr);

            currentConnInfo.GameLobby.InstanceId = jsonStr.Replace("\"", "");
            this.StartHost();
        }, () =>
        {
            Debug.LogError("Failed to register with backend as host.");
        }, str));
    }

    public class GameLobby
    {
        public ConnectionInfo ConnInfo;

        public string SceneName;
        public string InstanceId;

        public int PlayerCount;

        public GameLobby(ConnectionInfo connectionInfo, string sceneName, string instanceId, int playerCount)
        {
            this.ConnInfo = connectionInfo;
            this.SceneName = sceneName;
            this.InstanceId = instanceId;
            this.PlayerCount = playerCount;
        }
    }

    public void QueryGames(Action<List<GameLobby>> onComplete, Action onError)
    {
        QueryGames(SceneManager.GetActiveScene().name, onComplete, onError);
    }

    public void QueryGames(string sceneName, Action<List<GameLobby>> onComplete, Action onError)
    {
        string currentScene = SceneManager.GetActiveScene().name;

        StartCoroutine(SendWebRequest($"games?scene={sceneName}", (jsonStr) =>
        {
            List<GameLobby> lobbies = JsonConvert.DeserializeObject<List<GameLobby>>(jsonStr);

            onComplete(lobbies);
        }, onError));
    }

    public enum WebRequestType
    {
        Get,
        Post
    }

    private IEnumerator SendWebRequest(string endpoint, Action<string> onSuccess, Action onError)
    {
        Debug.Log("Bap");
        yield return SendWebRequest(WebRequestType.Get, $"http://{BACKEND_IP}:{BACKEND_PORT}", endpoint, onSuccess, onError, null);
    }

    private IEnumerator SendWebRequest(string endpoint, Action<string> onSuccess, Action onError, string jsonStr)
    {
        Debug.Log("Bop");
        yield return SendWebRequest(WebRequestType.Post, $"http://{BACKEND_IP}:{BACKEND_PORT}", endpoint, onSuccess, onError, jsonStr);
    }

    private IEnumerator SendWebRequest(WebRequestType type, string url, string endpoint, Action<string> onSuccess, Action onError, string jsonStr)
    {
        string fullUrl = $"{url}/{endpoint}";
        UnityWebRequest www = null;

        if(type == WebRequestType.Get)
        {
            www = UnityWebRequest.Get(fullUrl);
        }
        else if(type == WebRequestType.Post)
        {
            www = UnityWebRequest.Post(fullUrl, jsonStr, "application/json");
        }

        // Send request
        yield return www.SendWebRequest();

        // Attempt to handle response depending on initial state of request
        switch (www.result)
        {
            case UnityWebRequest.Result.Success:
                {
                    onSuccess(www.downloadHandler.text);
                    break;
                }

            default:
                {
                    onError();
                    break;
                }
        }
    }
}