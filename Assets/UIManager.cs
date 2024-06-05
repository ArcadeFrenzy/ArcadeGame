using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject winPanel;
    [SerializeField] TextMeshProUGUI winText;
    [SerializeField] Button restartButton;

    void Start()
    {
        winPanel.SetActive(false); // Hide the win panel on start
        restartButton.onClick.AddListener(RestartGame); // Add listener for restart button
    }

    public void ShowWinScreen(string winner)
    {
        winText.text = winner;
        winPanel.SetActive(true); // Show the win panel
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("Lobby");
    }
}