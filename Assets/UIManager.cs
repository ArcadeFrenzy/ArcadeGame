using UnityEngine;
using UnityEngine.UI;
using TMPro;
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
        winText.text = winner + " Wins!";
        winPanel.SetActive(true); // Show the win panel
    }

    void RestartGame()
    {
        // Assuming you have a script named GameScript attached to another GameObject
        GameScript gameScript = FindObjectOfType<GameScript>();
        if (gameScript != null)
        {
            gameScript.ResetGame(); // Call the ResetGame function from GameScript
        }
        else
        {
            Debug.LogError("GameScript not found! Cannot reset the game.");
        }
    }
}