using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerAuthManager : MonoBehaviour
{
    public const string BACKEND_IP = "localhost";
    public const ushort BACKEND_PORT = 5066;

    [Serializable]
    public class PlayerAuth
    {
        public string host;
        public ushort port;

        public string username;
    }

    public GameObject NetworkManagerObj;
    public GameObject LoginCanvasObj;
    public TMP_InputField UsernameInputField;

    public PlayerNetworkManager NetworkManager
    {
        get
        {
            return NetworkManagerObj.GetComponent<PlayerNetworkManager>();
        }
    }

    public void Login()
    {
        StartCoroutine(SubmitLoginRequest());
    }

    public class ConnectionRequest
    {
        public bool Success = false;
    }

    IEnumerator SubmitLoginRequest()
    {
        var str = new JObject(new JProperty("username", UsernameInputField.text)).ToString(Formatting.None);
        Debug.Log(str);

        using (UnityWebRequest www = UnityWebRequest.Post($"http://{BACKEND_IP}:{BACKEND_PORT}/auth", str, "application/json"))
        {
            yield return www.SendWebRequest();

            switch(www.result)
            {
                case UnityWebRequest.Result.Success:
                    {
                        //Debug.Log($"Success {www.downloadHandler.text}");

                        ConnectionRequest connectionRequest = new ConnectionRequest();
                        yield return NetworkManager.Connect(connectionRequest, UsernameInputField.text);

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
}
