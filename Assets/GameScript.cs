using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScript : MonoBehaviour
{
    int spriteIndex = -1;

    public int PlayerTurn()
    {
        spriteIndex++;
        return spriteIndex % 2;
    }

    public void ResetGame()
    {
        // Reset game state logic here (e.g., reset spriteIndex, enable unplayed flag in TurnScript)
        spriteIndex = -1;
        // Find all TurnScript instances and reset their unplayed flag (assuming they're children of GameBoard)
        foreach (TurnScript turnScript in GetComponentsInChildren<TurnScript>())
        {
            turnScript.unplayed = true;
            turnScript.spriteRenderer.sprite = null;
        }
    }
}
