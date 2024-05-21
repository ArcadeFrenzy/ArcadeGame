using UnityEngine;

public abstract class NetworkedScene : MonoBehaviour
{
    public abstract void OnLocalPlayerJoin();

    public abstract void OnPlayerJoin(int playerId, string playerName, Vector3 playerPos);
}