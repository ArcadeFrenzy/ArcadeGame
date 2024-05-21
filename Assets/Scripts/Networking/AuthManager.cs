using UnityEngine;

public class AuthManager : MonoBehaviour
{
    public static AuthManager Instance;

    public GameObject LoginCanvasObj;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            return;
        }

        Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetUsername(string username)
    {
        NetworkManager.Instance.username = username;
    }

    public void Login()
    {
        NetworkManager.Instance.Connect();
        NetworkManager.Instance.SendCommand(new ClientHelloCommand(NetworkManager.Instance.username));
    }
}
