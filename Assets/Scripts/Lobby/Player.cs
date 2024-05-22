using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int playerId;
    public bool client;

    public string gameQueuedFor;
    public TextMeshPro UsernameText;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (client)
        {
            Vector3 pos = this.transform.position;

            if (MoveUpdate(ref pos))
            {
                this.transform.position = pos;
                NetworkManager.Instance.SendCommand(new SetPositionCommand(pos));
            }
        }
    }

    bool MoveUpdate(ref Vector3 pos)
    {
        pos.x += Input.GetAxis("Horizontal") * 2f * Time.fixedDeltaTime;
        pos.z += Input.GetAxis("Vertical") * 2f * Time.fixedDeltaTime;

        bool change = Mathf.Abs(Input.GetAxis("Horizontal")) > float.Epsilon | Mathf.Abs(Input.GetAxis("Vertical")) > float.Epsilon;

        return change;
    }
}