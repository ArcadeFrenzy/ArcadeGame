using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardInitializer : MonoBehaviour
{
    public GameObject[,] cells;

    private void Awake()
    {
        cells = new GameObject[3, 3];
        int index = 0;
        foreach (Transform child in transform)
        {
            int row = index / 3;
            int col = index % 3;
            cells[row, col] = child.gameObject;
            TurnScript turnScript = child.GetComponent<TurnScript>();
            if (turnScript != null)
            {
                turnScript.row = row;
                turnScript.col = col;
            }
            index++;
        }
    }
}
