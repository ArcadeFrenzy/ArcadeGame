using UnityEngine;

public class PongNetworkedScene : NetworkedScene
{
    public override void OnLocalPlayerJoin()
    {
        throw new System.NotImplementedException();
    }

    public override void OnPlayerJoin(int playerId, string playerName, Vector3 playerPos)
    {
        throw new System.NotImplementedException();
    }

    public override void OnPlayerLeave(int playerId)
    {
        throw new System.NotImplementedException();
    }

    void Awake()
    {
        NetworkManager.Instance.SendCommand(new PlayerReadyCommand());
    }
}