using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyManager : MonoBehaviour
{
    public GameObject LoginCanvas;
    public GameObject PlayerPrefab;

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
        PlayerManager.Instance.username = username;
    }

    public void Login()
    {
        LoginCanvas.SetActive(false);

        GameObject playerObj = Instantiate(PlayerPrefab);
        playerObj.GetComponent<Player>().UsernameText.text = PlayerManager.Instance.username;
    }
}
