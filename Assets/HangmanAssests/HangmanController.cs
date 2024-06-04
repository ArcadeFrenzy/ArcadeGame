using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HangmanController : MonoBehaviour
{
    [SerializeField] GameObject wordBox;
    [SerializeField] GameObject keyboardBox;
    [SerializeField] GameObject letterBox;
    [SerializeField] GameObject[] hangmanStages;
    [SerializeField] GameObject letterButton;
    [SerializeField] TextAsset possibleWord;
    [SerializeField] TextMeshProUGUI playerTurnText;
    [SerializeField] TextMeshProUGUI player1ScoreText;
    [SerializeField] TextMeshProUGUI player2ScoreText;
    [SerializeField] TextMeshProUGUI endGameText; // Added TextMeshProUGUI for end game message

    private string word;
    private int correctGuesses;
    private int incorrectGuesses;

    private int currentPlayer;
    private int[] playerScores;
    private bool gameEnded; // Added to track if the game has ended

    void Start()
    {
        playerScores = new int[2] { 0, 0 }; // Initialize player scores once
        currentPlayer = 0; // Player 1 starts the game
        gameEnded = false;
        endGameText.gameObject.SetActive(false);
        InitialiseButtons();
        InitialiseGame();
    }

    private void InitialiseButtons()
    {
        for (int i = 65; i <= 90; i++)
        {
            createButton(i);
        }
    }

    private void InitialiseGame()
    {
        // Reset data for a new round
        incorrectGuesses = 0;
        correctGuesses = 0;

        foreach (Button child in keyboardBox.GetComponentsInChildren<Button>())
        {
            child.interactable = true;
        }
        foreach (Transform child in wordBox.GetComponentInChildren<Transform>())
        {
            Destroy(child.gameObject);
        }
        foreach (GameObject stage in hangmanStages)
        {
            stage.SetActive(false);
        }

        // Generate word
        word = generateWord().ToUpper();
        foreach (char letter in word)
        {
            var temp = Instantiate(letterBox, wordBox.transform);
        }

        UpdatePlayerTurnText();
        UpdateScoreText();
    }

    private void createButton(int i)
    {
        GameObject temp = Instantiate(letterButton, keyboardBox.transform);
        temp.GetComponentInChildren<TextMeshProUGUI>().text = ((char)i).ToString();
        temp.GetComponent<Button>().onClick.AddListener(delegate { CheckLetter(((char)i).ToString()); });
    }

    private string generateWord()
    {
        string[] wordList = possibleWord.text.Split('\n');
        string line = wordList[Random.Range(0, wordList.Length - 1)];
        return line.Substring(0, line.Length - 1);
    }

    private void CheckLetter(string inputLetter)
    {
        if (gameEnded) return; // Do nothing if the game has ended

        bool letterInWord = false;
        for (int i = 0; i < word.Length; i++)
        {
            if (inputLetter == word[i].ToString())
            {
                letterInWord = true;
                correctGuesses++;
                wordBox.GetComponentsInChildren<TextMeshProUGUI>()[i].text = inputLetter;
            }
        }
        if (!letterInWord)
        {
            incorrectGuesses++;
            hangmanStages[incorrectGuesses - 1].SetActive(true);
        }
        checkOutcome();
        if (correctGuesses != word.Length && incorrectGuesses != hangmanStages.Length) // Only switch turn if the game is not over
        {
            SwitchPlayerTurn();
        }
    }

    private void checkOutcome()
    {
        if (correctGuesses == word.Length) // Win
        {
            playerScores[currentPlayer]++;
            for (int i = 0; i < word.Length; i++)
            {
                wordBox.GetComponentsInChildren<TextMeshProUGUI>()[i].color = Color.green;
            }
            UpdateScoreText();
            if (playerScores[currentPlayer] >= 3)
            {
                EndGame(currentPlayer);
            }
            else
            {
                SwitchPlayerTurn(); // Switch starting player for next round
                Invoke("InitialiseGame", 3f);
            }
        }
        else if (incorrectGuesses == hangmanStages.Length) // Lose
        {
            for (int i = 0; i < word.Length; i++)
            {
                wordBox.GetComponentsInChildren<TextMeshProUGUI>()[i].color = Color.red;
                wordBox.GetComponentsInChildren<TextMeshProUGUI>()[i].text = word[i].ToString();
            }
            SwitchPlayerTurn(); // Switch starting player for next round
            Invoke("InitialiseGame", 3f);
        }
    }

    private void SwitchPlayerTurn()
    {
        currentPlayer = 1 - currentPlayer; // Toggle between 0 and 1
        UpdatePlayerTurnText();
    }

    private void UpdatePlayerTurnText()
    {
        playerTurnText.text = "Player " + (currentPlayer + 1) + "'s Turn";
    }

    private void UpdateScoreText()
    {
        player1ScoreText.text = "Player 1 Score: " + playerScores[0];
        player2ScoreText.text = "Player 2 Score: " + playerScores[1];
    }

    private void EndGame(int winningPlayer)
    {
        gameEnded = true;
        playerTurnText.gameObject.SetActive(false);
        endGameText.text = "Player " + (winningPlayer + 1) + " Wins!";
        endGameText.gameObject.SetActive(true);

        // Disable all buttons to prevent further interaction
        foreach (Button child in keyboardBox.GetComponentsInChildren<Button>())
        {
            child.interactable = false;
        }
    }
}
