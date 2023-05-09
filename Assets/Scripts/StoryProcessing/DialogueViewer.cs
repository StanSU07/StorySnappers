using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static DialogueObject;
using UnityEngine.Events;
using System;
using System.Runtime.InteropServices;
using TMPro;
using System.Net;
using Unity.VisualScripting;

public class DialogueViewer : MonoBehaviour {
    [SerializeField] private Transform responseParent;
    [SerializeField] private TMP_Text storyText;
    [SerializeField] private Button responsePrefab;
    [SerializeField] private Button endButtonPrefab;
    [SerializeField] private Transform endPanel;
    [SerializeField] private DialogueController dialogueController;
    [SerializeField] private GameObject cameraController;
    [SerializeField] private GameObject photosPanel;
    [SerializeField] private PhotoDisplay photoDisplayObj;

    private void OnEnable()
    {
        if (dialogueController != null) 
        {
            dialogueController.EnteredNodeEvent += OnNodeEntered;
        }
    }

    private void OnDisable()
    {
        dialogueController.EnteredNodeEvent -= OnNodeEntered;
    }

    public static void DestroyResponses(Transform parent) 
    {
        for (int childIndex = parent.childCount - 1; childIndex >= 0; childIndex--) 
        {
            Button child = parent.GetChild(childIndex).GetComponent<Button>();

            if (child != null)
            {
                child.onClick.RemoveAllListeners();
                Destroy(child.gameObject);
            }
        }
    }

    private void OnNodeSelected(int indexChosen) 
    {
        if (indexChosen == -1)
        {
            dialogueController.ChosenResponse(0);
            return;
        }

        EnableCamera();
        dialogueController.ChosenResponse(indexChosen);
    }

    private void OnNodeEntered(Node newNode)
    {
        DestroyResponses(responseParent);
        storyText.text = newNode.text;

        List<Response> responses = dialogueController.GetCurrentResponses();
        if (responses.Count > 0) 
        {
            InstantiateResponses(responses);
        }
        else
        {
            // Check if current node has "END" tag and add new button if so
            if (newNode.tags.Contains("END"))
            {
                Button newButton = Instantiate(endButtonPrefab, endPanel);
                //newButton.onClick.AddListener(ContinueStory);
                newButton.onClick.AddListener(() => {
                    photosPanel.SetActive(true);
                    photoDisplayObj.SetPhotoDisplayPage();
                });
            }
        }
    }
    private void EnableCamera()
    {
        cameraController.SetActive(true);
    }

    private void InstantiateResponses(List<Response> responses)
    {
        for (int i = 0; i < responses.Count; i++)         
        {
            Button newResponse = Instantiate(responsePrefab, responseParent);
            newResponse.GetComponentInChildren<TMP_Text>().text = responses[i].displayText;
            newResponse.gameObject.name = i.ToString();

            newResponse.onClick.AddListener(delegate { OnNodeSelected(int.Parse(newResponse.gameObject.name)); });
        }
    }

    private void ContinueStory()
    {
        if (dialogueController.GetCurrentNode().tags.Contains("END"))
        {
            OnNodeSelected(-1);
        }
        else
        {
            //end story
        }
    }
}
