using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ConnectFour
{


    public class ConnectFourGame1 : MonoBehaviour
    {
        enum Piece
        {
            Empty = 0, Yellow = 1, Red = 2
        }

        //size of board
        [Range(3, 8)]
        public int numRows = 6;

        [Range(3, 8)]
        public int numCols = 7;

        //set amount of tiles to win and whether diagonal wins
        [Tooltip("How many pieces must be connected to win the game?")]
        public int numToWin = 4;

        [Tooltip("Allow diagonal wins?")]
        public bool allowDiag = true;

        public float dropTime = 4f;

        //prefabs for game pieces
        public GameObject pieceRed;
        public GameObject pieceYellow;

        //prefab for game board
        public GameObject pieceField;

        //text for displaying win,loss,draw
        public GameObject winningText;
        public string playerWonText = "You Won";
        public string playerLoseText = "You Lose";
        public string drawText = "Draw";

        //play again button GameObject
        //checks if mouse is touching and changes colour
        public GameObject playAgainButton;
        bool playAgainButtonTouching = false;
        Color playAgainButtonOrigColour;
        Color playAgainButtonHoverColour = new Color(255, 143, 4);

        //quit button GameObject
        public GameObject quitButton;
        bool quitButtonTouching = false;
        Color quitButtonOrigColour;
        Color quitButtonHoverColour = new Color(255, 143, 4);


        //parent object for game field objects
        GameObject gameObjectField;

        //creates a temporary piece for current turn
        //at mouse position until click
        GameObject gameObjectTurn;


        //game field where
        //0 = empty,
        //1 = yellow,
        //2 = red
        int[,] field;

        //various boolean checks, default starting player is 1
        bool isPlayerOne = true;
        bool isLoading = true;
        bool isDropping = false;
        bool mouseButtonPressed = false;
        bool gameOver = false;
        bool isCheckingForWinner = false;

        //method is called when the game is started
        private void Start()
        {

            //checks if pieces to win is bigger than available tiles
            int max = Mathf.Max(numRows, numCols);

            if (numToWin > max)
            {
                numToWin = max;
            }

            //creates game board
            CreateField();

            //randomly chooses first player
            isPlayerOne = System.Convert.ToBoolean(Random.Range(0, 1));

            //store colour of "play again" button
            playAgainButtonOrigColour = playAgainButton.GetComponent<Renderer>().material.color;

            //store colour of "quit" button
            quitButtonOrigColour = quitButton.GetComponent<Renderer>().material.color;
        }

        //creates a new game field
        void CreateField()
        {
            //hides winning text and play again button
            winningText.SetActive(false);
            playAgainButton.SetActive(false);
            quitButton.SetActive(false);

            isLoading = true;

            //destroys existing game field if any
            gameObjectField = GameObject.Find("Field");
            if (gameObjectField != null)
            {
                DestroyImmediate(gameObjectField);
                Debug.Log("Destroyed existing game field");
            }

            //creates game object for the game field
            gameObjectField = new GameObject("Field");


            //create a new game board array with empty cells
            field = new int[numCols, numRows];
            for (int x = 0; x < numCols; x++)
            {
                for (int y = 0; y < numRows; y++)
                {
                    field[x, y] = (int)Piece.Empty;  //setting every cell to empty

                    //
                    GameObject g = Instantiate(pieceField, new Vector3(x, y * -1, -1), Quaternion.identity) as GameObject;
                    g.transform.parent = gameObjectField.transform;
                }
            }

            isLoading = false;
            gameOver = false;

            //position main camera, winning text and play again button
            Camera.main.transform.position = new Vector3((numCols - 1) / 2.0f, -((numRows - 1) / 2.0f), Camera.main.transform.position.z);

            winningText.transform.position = new Vector3((numCols - 1) / 2.0f, -((numRows - 1) / 2.0f) + 1, winningText.transform.position.z);

            playAgainButton.transform.position = new Vector3((numCols - 1) / 2.0f, -((numRows - 1) / 2.0f) - 1, playAgainButton.transform.position.z);

            quitButton.transform.position = new Vector3((numCols - 1) / 2.0f, -((numRows - 1) / 2.0f) - 2, quitButton.transform.position.z);
        }

        //controls spawning pieces and switching player turns
        GameObject SpawnPiece()
        {
            //get mouse position
            Vector3 spawnPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            //if it is not player ones turn, start ai turn 
            //must change to player two logic
            if (!isPlayerOne)
            {
                List<int> moves = GetPossibleMoves();

                if (moves.Count > 0)
                {
                    int column = moves[Random.Range(0, moves.Count)];

                    spawnPos = new Vector3(column, 0, 0);
                }
            }

            //spawns counter above first row, colour depends on player (default yellow)
            GameObject g = Instantiate(
                isPlayerOne ? pieceYellow : pieceRed,
                new Vector3(
                    Mathf.Clamp(spawnPos.x, 0, numCols - 1),
                    gameObjectField.transform.position.y + 1, 0),
                Quaternion.identity) as GameObject;



            return g;
        }

        //updates play again button if pressed
        void UpdatePlayAgainButton()
        {
            RaycastHit hit;

            //creates ray at mouse position
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            //if mouse is hovering over the play again button, change the colour
            if (Physics.Raycast(ray, out hit) && hit.collider.name == playAgainButton.name)
            {
                playAgainButton.GetComponent<Renderer>().material.color = playAgainButtonHoverColour;

                //if left mouse is clicked while hovering, reload the game
                if (Input.GetMouseButtonDown(0) || Input.touchCount > 0 && playAgainButtonTouching == false)
                {
                    playAgainButtonTouching = true;

                    SceneManager.LoadScene(0);
                }

            }

            //if mouse is removed change colour back
            else
            {
                playAgainButton.GetComponent<Renderer>().material.color = playAgainButtonOrigColour;
            }

            if (Input.touchCount == 0)
            {
                playAgainButtonTouching = false;
            }
        }

        //updates play again button if pressed
        void UpdateQuitButton()
        {
            RaycastHit hit;

            //creates ray at mouse position
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            //if mouse is hovering over the play again button, change the colour
            if (Physics.Raycast(ray, out hit) && hit.collider.name == quitButton.name)
            {
                quitButton.GetComponent<Renderer>().material.color = quitButtonHoverColour;

                //if left mouse is clicked while hovering, reload the game
                if (Input.GetMouseButtonDown(0) || Input.touchCount > 0 && quitButtonTouching == false)
                {
                    quitButtonTouching = true;

                    SceneManager.LoadScene("Lobby");
                }

            }

            //if mouse is removed change colour back
            else
            {
                quitButton.GetComponent<Renderer>().material.color = quitButtonOrigColour;
            }

            if (Input.touchCount == 0)
            {
                quitButtonTouching = false;
            }
        }

        void Update()
        {

            //stops game updates if game is loading
            if (isLoading)
                return;

            //stops game updates if game is checking for winner
            if (isCheckingForWinner)
                return;

            //stops game updates if game is over, and loads play again button
            if (gameOver)
            {
                winningText.SetActive(true);
                playAgainButton.SetActive(true);
                quitButton.SetActive(true);

                UpdatePlayAgainButton();
                UpdateQuitButton();

                return;
            }


            //handles player ones turn
            if (isPlayerOne)
            {

                //if temp object is null, spawn one
                if (gameObjectTurn == null)
                {
                    gameObjectTurn = SpawnPiece();
                }


                else
                {
                    //makes counter follow mouse position, but clamps between board x boundaries
                    Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    gameObjectTurn.transform.position = new Vector3(Mathf.Clamp(pos.x, 0, numCols - 1),
                        gameObjectField.transform.position.y + 1, 0);

                    //if left mouse is pressed while it is not already
                    //and the counter is not already dropping, then drop counter
                    if (Input.GetMouseButtonDown(0) && !mouseButtonPressed && !isDropping)
                    {
                        mouseButtonPressed = true;

                        StartCoroutine(dropPiece(gameObjectTurn));
                    }

                    else
                    {
                        mouseButtonPressed = false;
                    }
                }
            }

            //handles player 2 turn
            else
            {

                //if temp object is null, spawn one
                if (gameObjectTurn == null)
                {
                    gameObjectTurn = SpawnPiece();
                }

                //start dropping, must change to player two logic
                else
                {
                    //makes counter follow mouse position, but clamps between x boundaries
                    Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    gameObjectTurn.transform.position = new Vector3(Mathf.Clamp(pos.x, 0, numCols - 1),
                        gameObjectField.transform.position.y + 1, 0);

                    //if left mouse is pressed while it is not already
                    //and the counter is not already dropping, then drop counter
                    if (Input.GetMouseButtonDown(0) && !mouseButtonPressed && !isDropping)
                    {
                        mouseButtonPressed = true;

                        StartCoroutine(dropPiece(gameObjectTurn));
                    }

                    else
                    {
                        mouseButtonPressed = false;
                    }
                }
            }
        }

        //creates move list for AI
        public List<int> GetPossibleMoves()
        {
            List<int> possibleMoves = new List<int>();
            for (int x = 0; x < numCols; x++)
            {
                for (int y = numRows - 1; y >= 0; y--)
                {
                    if (field[x, y] == (int)Piece.Empty)
                    {
                        possibleMoves.Add(x);
                        break;
                    }
                }
            }

            return possibleMoves;

        }

        IEnumerator dropPiece(GameObject gObject)
        {
            isDropping = true;

            Vector3 startPosition = gObject.transform.position;
            Vector3 endPosition = new Vector3();

            int x = Mathf.RoundToInt(startPosition.x);
            startPosition = new Vector3(x, startPosition.y, startPosition.z);

            bool foundFreeCell = false;
            for (int i = numRows - 1; i >= 0; i--)
            {
                if (field[x, i] == 0)
                {
                    foundFreeCell = true;
                    field[x, i] = isPlayerOne ? (int)Piece.Yellow : (int)Piece.Red;
                    endPosition = new Vector3(x, i * -1, startPosition.z);

                    break;
                }
            }

            if (foundFreeCell)
            {
                GameObject g = Instantiate(gObject) as GameObject;
                gameObjectTurn.GetComponent<Renderer>().enabled = false;

                float distance = Vector3.Distance(startPosition, endPosition);

                float t = 0;
                while (t < 1)
                {
                    t += Time.deltaTime * ((numRows - distance) + 1);

                    g.transform.position = Vector3.Lerp(startPosition, endPosition, t);
                    yield return null;
                }

                g.transform.parent = gameObjectField.transform;

                DestroyImmediate(gameObjectTurn);

                StartCoroutine(Won());

                while (isCheckingForWinner)
                    yield return null;

                isPlayerOne = !isPlayerOne;
            }

            isDropping = false;

            yield return 0;
        }

        IEnumerator Won()
        {
            //checks for winner
            isCheckingForWinner = true;

            //iterates through all cells
            for (int x = 0; x < numCols; x++)
            {
                for (int y = 0; y < numRows; y++)
                {
                    //
                    int layermask = isPlayerOne ? (1 << 8) : (1 << 9);

                    //checks for current players piece, if cell doesn't contain then continue
                    if (field[x, y] != (isPlayerOne ? (int)Piece.Yellow : (int)Piece.Red))
                    {
                        continue;
                    }

                    //sends horizontal raycast to check for numToWin-1 pieces of current players pieces
                    RaycastHit[] hitHorizontal = Physics.RaycastAll(new Vector3(x, y * -1, 0), Vector3.right, numToWin - 1, layermask);

                    //if raycast length is correct amount, end game
                    if (hitHorizontal.Length == numToWin - 1)
                    {
                        gameOver = true;
                        break;
                    }

                    //send vertical raycast to check for numToWin-1 pieces of current players pieces
                    RaycastHit[] hitVertical = Physics.RaycastAll(new Vector3(x, y * -1, 0), Vector3.up, numToWin - 1, layermask);

                    //if raycast length is correct amount, end game
                    if (hitVertical.Length == numToWin - 1)
                    {
                        gameOver = true;
                        break;
                    }

                    //test for diagonal win if enabled
                    if (allowDiag)
                    {
                        //calculate length of diagonal ray based on numToWin
                        float length = Vector2.Distance(new Vector2(0, 0), new Vector2(numToWin - 1, numToWin - 1));

                        //send raycast diagonally left to check for winning sequence
                        RaycastHit[] hitDiagLeft = Physics.RaycastAll(new Vector3(x, y * -1, 0), new Vector3(-1, 1), length, layermask);

                        //if raycast length is correct amount, end game
                        if (hitDiagLeft.Length == numToWin - 1)
                        {
                            gameOver = true;
                            break;
                        }

                        //sends raycast diagonally right to check for winning sequence
                        RaycastHit[] hitDiagRight = Physics.RaycastAll(new Vector3(x, y * -1, 0), new Vector3(1, 1), length, layermask);

                        //if raycast length is correct amount, end game
                        if (hitDiagRight.Length == numToWin - 1)
                        {
                            gameOver = true;
                            break;
                        }
                    }

                    yield return null;
                }

                yield return null;
            }

            //if winner is found set winning text to winning player
            if (gameOver == true)
            {
                winningText.GetComponent<TextMesh>().text = isPlayerOne ? playerWonText : playerLoseText;
            }
            //if there is a draw, end game and set winning text to draw text
            else
            {
                if (!FieldContainsEmptyCell())
                {
                    gameOver = true;
                    winningText.GetComponent<TextMesh>().text = drawText;
                }
            }

            isCheckingForWinner = false;

            yield return 0;
        }

        //checks for any empty cells on game board, exits if any are found
        bool FieldContainsEmptyCell()
        {
            for (int x = 0; x < numCols; x++)
            {
                for (int y = 0; y < numRows; y++)
                {
                    if (field[x, y] == (int)Piece.Empty)
                        return true;
                }
            }
            return false;
        }

    }
}