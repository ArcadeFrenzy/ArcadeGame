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

    private string word;
    private int correctGuesses;
    private int incorrectGuesses;
    
    void Start()
    {
        InitialiseButtons();
        InitialiseGame();
    }

    private void InitialiseButtons()
    {
        for(int i = 65; i <= 90; i++)
        {
            createButton(i);
        }
    }

    private void InitialiseGame()
    {
        //reset data
        incorrectGuesses = 0;
        correctGuesses = 0;
        foreach(Button child in keyboardBox.GetComponentsInChildren<Button>())
        {
            child.interactable = true;
        }
        foreach(Transform child in wordBox.GetComponentsInChildren<Transform>())
        {
            Destroy(child.gameObject);
        }
        foreach(GameObject stage in hangmanStages)
        {
            stage.SetActive(false);
        }

        //generate word
        word = generateWord().ToUpper();
        foreach(char letter in word)
        {
            var temp = Instantiate(letterBox, wordBox.transform);
        }
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
        bool letterInWord = false;
        for(int i = 0; i < word.Length; i++)
        {
            if(inputLetter == word[i].ToString())
            {
                letterInWord = true;
                correctGuesses++;
                wordBox.GetComponentsInChildren<TextMeshProUGUI>()[i].text = inputLetter;
            }
        }
        if(!letterInWord)
        {
            incorrectGuesses++;
            hangmanStages[incorrectGuesses - 1].SetActive(true);
        }
        checkOutcome();
    }

    private void checkOutcome()
    {
        if(correctGuesses == word.Length) //win
        {
            for(int i = 0; i < word.Length; i++)
            {
                wordBox.GetComponentsInChildren<TextMeshProUGUI>()[i].color = Color.green;
            }
            Invoke("InitialiseGame", 3f);
        }
        if(incorrectGuesses == hangmanStages.Length) //lose
        {
            for(int i = 0; i < word.Length; i++)
            {
                wordBox.GetComponentsInChildren<TextMeshProUGUI>()[i].color = Color.red;
                wordBox.GetComponentsInChildren<TextMeshProUGUI>()[i].text = word[i].ToString();
            }
            Invoke("InitialiseGame", 3f);
        }
    }
}
