using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class LobbyGameQueueInteractable : MonoBehaviour
{
    public string sceneName;

    private void OnTriggerEnter(Collider other)
    {
        Player player = other.gameObject.GetComponent<Player>();
        SceneManager.LoadScene(this.sceneName);
    }
}