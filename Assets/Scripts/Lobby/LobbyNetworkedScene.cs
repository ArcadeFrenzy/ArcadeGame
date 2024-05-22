using UnityEngine;

public class LobbyNetworkedScene : NetworkedScene
{
    public GameObject PlayerPrefab;
    private GameObject LocalPlayerObj;

    public override void OnLocalPlayerJoin()
    {
        this.LocalPlayerObj = Instantiate(PlayerPrefab);

        this.LocalPlayerObj.GetComponent<Player>().playerId = NetworkManager.Instance.playerId;
        this.LocalPlayerObj.GetComponent<Player>().UsernameText.text = NetworkManager.Instance.username;
        this.LocalPlayerObj.GetComponent<Player>().client = true;

        PlayerManager.Instance.AddPlayer(this.LocalPlayerObj.GetComponent<Player>());
    }

    public override void OnPlayerJoin(int playerId, string playerName, Vector3 playerPos)
    {
        GameObject RemotePlayerObj = Instantiate(PlayerPrefab, playerPos, Quaternion.identity);
        RemotePlayerObj.GetComponent<Player>().playerId = playerId;
        RemotePlayerObj.GetComponent<Player>().UsernameText.text = playerName;

        PlayerManager.Instance.AddPlayer(RemotePlayerObj.GetComponent<Player>());
    }

    public override void OnPlayerLeave(int playerId)
    {
        Player player = PlayerManager.Instance.GetPlayer(playerId);
        Destroy(player.gameObject);

        PlayerManager.Instance.RemovePlayer(player);
    }
}