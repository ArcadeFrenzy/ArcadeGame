using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DropdownTextUpdater : MonoBehaviour
{
    public TMP_Text textToUpdate;
    public Dropdown dropdown;

    void Start()
    {        
        textToUpdate.text = dropdown.options[dropdown.value].text;
        dropdown.onValueChanged.AddListener(OnDropdownValueChanged);
    }

    private void OnDropdownValueChanged(int newSelection)
    {
        textToUpdate.text = dropdown.options[newSelection].text;
    }
}