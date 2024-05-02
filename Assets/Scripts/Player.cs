using Unity.Netcode;
using UnityEngine;

public class Player : NetworkBehaviour
{
    public NetworkVariable<Vector3> Position = new NetworkVariable<Vector3>();
    public string Username;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Position.Value;
    }

    [Rpc(SendTo.Server)]
    void SubmitPositionRequestServerRpc(RpcParams rpcParams = default)
    {
        var randomPosition = new Vector3(Random.Range(-3f, 3f), 1f, Random.Range(-3f, 3f));
        transform.position = randomPosition;
        Position.Value = randomPosition;
    }

    public override void OnNetworkSpawn()
    {
        if(IsOwner)
        {
            SubmitPositionRequestServerRpc();
        }
    }
}
