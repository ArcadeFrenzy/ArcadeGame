using System;
using System.Linq;
using TMPro;
using UnityEngine;

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

    public GameNetworkManager NetworkManager
    {
        get
        {
            return NetworkManagerObj.GetComponent<GameNetworkManager>();
        }
    }

    public void Login()
    {
        // Set username
        PlayerNetworkManager.Username = UsernameInputField.text;

        NetworkManager.QueryAndConnect(() => // on success
        {
            LoginCanvasObj.SetActive(false);
        }, () => // on error
        {
            // Failed.
        });
    }
}
