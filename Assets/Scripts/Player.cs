using Mirror;
using TMPro;
using UnityEngine;

public class Player : NetworkBehaviour
{
    [SyncVar(hook = nameof(UpdateUsernameHook))]
    public string username;

    public TextMeshPro UsernameText;

    void UpdateUsernameHook(string oldUsername, string newUsername)
    {
        //Debug.Log($"Update from {oldUsername} to {newUsername}");
        UsernameText.text = newUsername;
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(isLocalPlayer)
        {
            Vector3 pos = this.transform.position;

            if (MoveUpdate(ref pos))
            {
                MoveUpdate(pos);
            }
        }
    }

    [Command]
    void MoveUpdate(Vector3 newPos)
    {
        transform.position = newPos;
    }

    bool MoveUpdate(ref Vector3 pos)
    {
        pos.x += Input.GetAxis("Horizontal") * 2f * Time.fixedDeltaTime;
        pos.z += Input.GetAxis("Vertical") * 2f * Time.fixedDeltaTime;

        bool change = Mathf.Abs(Input.GetAxis("Horizontal")) > float.Epsilon | Mathf.Abs(Input.GetAxis("Vertical")) > float.Epsilon;

        return change;
    }

    [Command]
    void SubmitPositionRequest()
    {
        var randomPosition = new Vector3(Random.Range(-3f, 3f), 1f, Random.Range(-3f, 3f));
        transform.position = randomPosition;
    }

    [Command]
    void SubmitUsername()
    {
        //Debug.Log(this.username + " to " + PlayerNetworkManager.Username);

        this.username = PlayerNetworkManager.Username;
        UsernameText.text = this.username;
    }

    public override void OnStartClient()
    {
        base.OnStartClient();

        if (isLocalPlayer)
        {
            SubmitUsername();
            SubmitPositionRequest();

            this.username = PlayerNetworkManager.Username;
            UsernameText.text = this.username;
        }
    }
}