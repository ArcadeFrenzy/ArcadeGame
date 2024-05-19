using UnityEngine;

public class GameNetworkManager : MonoBehaviour
{
    public bool shared;
    public string gameInstanceId; // only set if shared is ticked otherwise will be randomly generated
    public int waitForPlayerCount; // player count to wait for before starting

    public string backendIp;
    public short backendPort;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
