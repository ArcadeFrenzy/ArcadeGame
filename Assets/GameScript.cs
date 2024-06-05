using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScript : MonoBehaviour
{
    int spriteIndex = -1;
    int[,] board = new int[3, 3];
    UIManager uiManager;

    private void Start()
    {
        uiManager = FindObjectOfType<UIManager>();
    }

    public int PlayerTurn()
    {
        spriteIndex++;
        return spriteIndex % 2;
    }

    public void MakeMove(int row, int col, int player)
    {
        board[row, col] = player + 1; // Store 1 for player 1, 2 for player 2

        if (CheckWin(player + 1))
        {
            uiManager.ShowWinScreen("Winner: Player " + (player + 1));
        }
        else if (spriteIndex >= 8)
        {
            uiManager.ShowWinScreen("It's a draw!");
        }
    }

    public bool CheckWin(int player)
    {
        // Check rows, columns, and diagonals
        for (int i = 0; i < 3; i++)
        {
            if (board[i, 0] == player && board[i, 1] == player && board[i, 2] == player) return true;
            if (board[0, i] == player && board[1, i] == player && board[2, i] == player) return true;
        }
        if (board[0, 0] == player && board[1, 1] == player && board[2, 2] == player) return true;
        if (board[0, 2] == player && board[1, 1] == player && board[2, 0] == player) return true;

        return false;
    }
}
