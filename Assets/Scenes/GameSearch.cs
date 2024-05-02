using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSearch : MonoBehaviour
{
    public Text resultText;
    public InputField searchInput;

    private List<string> gameTitles = new List<string>
    {
        "Pong",
        "Tetris",
        "Space Invaders",
        "Connect four",
        "Tic Tac Toe",
        "Minesweeper",
        "Hangman",
        "Sudoku"
    };

    private void Start()
    {
        searchInput.onValueChanged.AddListener(delegate { OnSearchInputChange(); });
    }

    private void OnSearchInputChange()
    {
        string searchTerm = searchInput.text;
        List<string> results = SearchGame(searchTerm);
        if (results.Count > 0)
        {
            resultText.text = string.Join("\n", results);
        }
        else
        {
            resultText.text = "No matching games found.";
        }
    }

    private List<string> SearchGame(string searchTerm)
    {
        List<string> results = new List<string>();
        foreach (string title in gameTitles)
        {
            if (title.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
            {
                results.Add(title);
            }
        }
        return results;
    }

}
