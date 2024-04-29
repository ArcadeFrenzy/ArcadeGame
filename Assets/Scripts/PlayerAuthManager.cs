using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerAuthManager : MonoBehaviour
{
    [Serializable]
    public class PlayerAuth
    {
        public string host;
        public ushort port;

        public string username;
    }

    public GameObject NetworkManagerObj;
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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Login()
    {
        StartCoroutine(SubmitLoginRequest());
    }

    IEnumerator SubmitLoginRequest()
    {
        var str = JsonUtility.ToJson(new PlayerAuth()
        {
            host = NetworkManager.Host,
            port = NetworkManager.Port,
            username = this.usernameText,
        }, false);

        using (UnityWebRequest www = UnityWebRequest.Post("http://localhost:5066/auth", str, "application/json"))
        {
            yield return www.SendWebRequest();

            switch(www.result)
            {
                case UnityWebRequest.Result.Success:
                    {
                        Debug.Log($"Success {www.downloadHandler.text}");
                        List<PlayerAuth> clients = JsonConvert.DeserializeObject<List<PlayerAuth>>(www.downloadHandler.text);


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
