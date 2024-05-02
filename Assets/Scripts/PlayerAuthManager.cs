using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerAuthManager : MonoBehaviour
{
    private const string BACKEND_IP = "localhost";
    private const ushort BACKEND_PORT = 5066;

    [Serializable]
    public class PlayerAuth
    {
        public string host;
        public ushort port;

        public string username;
    }

    public GameObject NetworkManagerObj;
    public GameObject LoginCanvasObj;

    public PlayerNetworkManager NetworkManager
    {
        get
        {
            return NetworkManagerObj.GetComponent<PlayerNetworkManager>();
        }
    }

    private string usernameText;

    // Start is called before the first frame update
    void Start()
    {
        NetworkManager.UNM.OnClientDisconnectCallback += UNM_OnClientDisconnectCallback;
        NetworkManager.UNM.OnClientStopped += UNM_OnClientStopped;
    }

    private void UNM_OnClientStopped(bool obj)
    {
        StartCoroutine(Host(null));
    }

    private void UNM_OnClientDisconnectCallback(ulong obj)
    {
        // Host if we failed to connect for whatever reason (or got disconnected in the process).
        // TODO: If multiple clients are disconnected, this could cause undefined behavior.

        NetworkManager.UNM.Shutdown(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Login()
    {
        StartCoroutine(SubmitLoginRequest());
    }

    class ConnectionRequest
    {
        public bool Success = false;
    }

    IEnumerator Host(PlayerAuth hostClient)
    {
        var str = JsonUtility.ToJson(new PlayerAuth()
        {
            host = "",
            port = NetworkManager.Port,
            username = this.usernameText,
        }, false);

        switch (NetworkManager.Connect(hostClient))
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

    IEnumerator Connect(ConnectionRequest connectionRequest)
    {
        using (UnityWebRequest www = UnityWebRequest.Get($"http://{BACKEND_IP}:{BACKEND_PORT}/connect"))
        {
            yield return www.SendWebRequest();

            switch(www.result)
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

    IEnumerator SubmitLoginRequest()
    {
        var str = new JObject(new JProperty("username", "username")).ToString(Formatting.None);
        Debug.Log(str);

        using (UnityWebRequest www = UnityWebRequest.Post($"http://{BACKEND_IP}:{BACKEND_PORT}/auth", str, "application/json"))
        {
            yield return www.SendWebRequest();

            switch(www.result)
            {
                case UnityWebRequest.Result.Success:
                    {
                        Debug.Log($"Success {www.downloadHandler.text}");

                        ConnectionRequest connectionRequest = new ConnectionRequest();
                        yield return Connect(connectionRequest);

                        LoginCanvasObj.SetActive(!connectionRequest.Success);

                        break;
                    }
                default:
                    {
                        Debug.LogError("Failed to connect.");
                        break;
                    }
            }
        }
    }

    public void SetUsernameText(string usernameText)
    {
        this.usernameText = usernameText;
    }
}
