using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoController : MonoBehaviour
{
    [SerializeField] private Transform nameSelectionParent;
    [SerializeField] private TMP_InputField numberOfPlayersInput;
    [SerializeField] private PlayerNameInput nameSelectionPrefab;
    [SerializeField] private Button beginButton;
    [SerializeField] private int minPlayers;
    [SerializeField] private int maxPlayers;
    [SerializeField] private int playerNameCap;
    [SerializeField] private List<string> finalPlayerNames = new(); 

    private Dictionary<PlayerNameInput, bool> filledPlayerNames = new();
    private DialogueController dialogueController;

    private void OnEnable()
    {
        dialogueController = FindObjectOfType<DialogueController>();    
        beginButton.gameObject.SetActive(false);
        beginButton.onClick.AddListener(FinalizePlayerNames);
;    }

    public List<string> GetPlayerNames()
    {
        return finalPlayerNames;
    }

    private void FinalizePlayerNames()
    {
        foreach (KeyValuePair<PlayerNameInput, bool> name in filledPlayerNames)
        {
            finalPlayerNames.Add(name.Key.GetComponent<TMP_InputField>().text);
        }

        dialogueController.FillStoryWithPlayers(finalPlayerNames);
    }

    //make sure that what is typed is a number and fits the range of min and max players
    public void ValidateInput()
    {
        string input = numberOfPlayersInput.text;

        if (input != "")
        {
            int numberOfPlayers;
            int.TryParse(input, out numberOfPlayers);
            beginButton.interactable = true;

            if (numberOfPlayers != 0)
            {
                if (numberOfPlayers < minPlayers)
                {
                    numberOfPlayers = minPlayers;
                }
                else if (numberOfPlayers > maxPlayers) 
                {
                    numberOfPlayers = maxPlayers;
                }

                InstantiateNameSelection(numberOfPlayers); 
                numberOfPlayersInput.text = numberOfPlayers.ToString();
            }
            else
            { 
                numberOfPlayersInput.text = null;
            }
        }
    }

    private void CheckFilledPlayerNames(PlayerNameInput playerInfoInput, string input)
    {
        if (input.Length > 0)
        { 
            filledPlayerNames[playerInfoInput] = true;
        }
        else 
        {
            filledPlayerNames[playerInfoInput] = false;
        }

        if (input.Length > playerNameCap) 
        {
            TMP_InputField inputField = playerInfoInput.GetComponent<TMP_InputField>();
            inputField.text = input.Substring(0, playerNameCap);
        }

        foreach (KeyValuePair<PlayerNameInput, bool> player in filledPlayerNames)
        {
            if (!player.Value)
            {
                beginButton.gameObject.SetActive(false);
                return;
            }
        }

        beginButton.gameObject.SetActive(true);
    }

    //create name selection gameobjects  Z
    public void InstantiateNameSelection(int numberOfPlayers)
    {
        DestroyPlayerNameInputs();
        filledPlayerNames.Clear();
        beginButton.gameObject.SetActive(false);

        for (int i = 0; i < numberOfPlayers; i++)
        {
            PlayerNameInput newPlayerNameInput = Instantiate(nameSelectionPrefab, nameSelectionParent);
            newPlayerNameInput.GetComponentInChildren<TMP_Text>().text = i.ToString(); 
            newPlayerNameInput.gameObject.name = i.ToString();
            newPlayerNameInput.GetComponent<PlayerNameInput>().UpdatePlayerLabelText("Player " + (i+1) + ":");

            filledPlayerNames.Add(newPlayerNameInput, false);
            newPlayerNameInput.GetComponent<TMP_InputField>().onValueChanged.AddListener(delegate { CheckFilledPlayerNames(newPlayerNameInput, newPlayerNameInput.GetComponentInChildren<TMP_InputField>().text); });
        }
    }
 
    //destroy the name selection gameobjects  
    public void DestroyPlayerNameInputs()
    {
        for (int childIndex = nameSelectionParent.childCount - 1; childIndex >= 0; childIndex--)
        {
            Transform child = nameSelectionParent.GetChild(childIndex);

            if (child != null)
            {
                PlayerNameInput name = child.GetComponent<PlayerNameInput>();

                if (filledPlayerNames.ContainsKey(name))
                {
                    filledPlayerNames.Remove(name);

                }

                Destroy(child.gameObject);
            }
        }
    }
}
