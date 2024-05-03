using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    GameObject red, yellow;

    bool isPlayer, hasGameFinished;

    [SerializeField]
    Text turnMessage;

    const string RED_MESSAGE = "Red's Turn";
    const string YELLOW_MESSAGE = "Yellows Turn";

    Color RED_COLOR = new Color(231, 29, 54, 255) / 255;
    Color YELLOW_COLOR = new Color(162, 255, 0, 255) / 255;

    private void Awake()
    {
        isPlayer = true;
        hasGameFinished = false;
        turnMessage.text = RED_MESSAGE;
        turnMessage.color = RED_COLOR;
    }


    public void GameStart()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);

    }

    public void GameQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //If GameFinished then return
            if (hasGameFinished) return;

            //Raycast2D
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);

            if (!hit.collider) return;

            if (hit.collider.CompareTag("Press"))
            {
                //Check out of bounds
                if(hit.collider.gameObject.GetComponent<Column>().targetLocation.y > 5f) return;

                //Spawn the Gameobject
                Vector3 spawnPos = hit.collider.gameObject.GetComponent<Column>().spawnLocation;
                Vector3 targetPos = hit.collider.gameObject.GetComponent<Column>().targetLocation;
                GameObject circle = Instantiate(isPlayer ? red : yellow);
                circle.transform.position = spawnPos;
                circle.GetComponent<Mover>().targetPosition = targetPos;

                //Increase the target location height
                hit.collider.gameObject.GetComponent<Column>().targetLocation = new Vector3(targetPos.x, targetPos.y + 1f, targetPos.z);

                //UpdateBoard

                //TurnMessage
                turnMessage.text = !isPlayer ? RED_MESSAGE : YELLOW_MESSAGE;
                turnMessage.color = !isPlayer ? RED_COLOR : YELLOW_COLOR;

                //Change PlayerTurn
                isPlayer = !isPlayer;

            }

        }
    }
}
