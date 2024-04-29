using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Search : MonoBehaviour
{
    static void Main()
    {
        List<string> gameTitles = new List<string>
        {
            "Pong",
            "Tetris",
            "Connect Four",
            "Pong",
            "Minesweeper",
            "Space Invaders",
        };

        Console.Write("Search for games");
        string searchTerm = Console.ReadLine();

        string result = SearchGame(gameTitles, searchTerm);

        if (result != null)
        {
            Console.WriteLine($"{result}");
        }
        else
        {
            Console.WriteLine("No game found");
        }
    }

    static string SearchGame(List<string> gameTitles, string searchTerm)
    {
        foreach(string title in gameTitles)
        {
            if (title.Equals(searchTerm, StringComparison.OrdinalIgnoreCase))
            {
                return title;
            }
        }

        return null;
    }
}
