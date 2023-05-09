using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerNameInput : MonoBehaviour
{
    [SerializeField] private TMP_Text playerLabelText;


    public void UpdatePlayerLabelText(string text)
    {
        playerLabelText.text = text;
    }
}
