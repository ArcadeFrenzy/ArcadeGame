using UnityEngine;

public sealed class LobbyGameQueueInteractable : MonoBehaviour
{
    public string gameName;

    private void OnTriggerEnter(Collider other)
    {
        Player player = other.gameObject.GetComponent<Player>();

        if (player == null || !player.client || player.gameQueuedFor == this.gameName)
        {
            return;
        }

        player.gameQueuedFor = this.gameName;
        NetworkManager.Instance.SendCommand(new GameQueueCommand(this.gameName));
    }
}