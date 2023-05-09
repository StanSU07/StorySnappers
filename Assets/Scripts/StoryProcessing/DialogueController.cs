using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using static DialogueObject;
using TMPro;

public class DialogueController : MonoBehaviour
{
    [SerializeField] private TextAsset twineText;

    private Dialogue curDialogue;
    private Node currNode;

    public event Action<Node> EnteredNodeEvent;
    public PhotoDisplay photoDisplayObj;

    public TextMeshProUGUI postChoiceTextObj01;
    public TextMeshProUGUI postChoiceTextObj02;

    private void Awake()
    {
        curDialogue = new Dialogue(twineText);
    }

    public Node GetCurrentNode()
    {
        return currNode;
    }

    public void InitializeDialogue(StoryInfo storyInfo)
    {
        currNode = curDialogue.GetNode(storyInfo.storyName);
        EnteredNodeEvent(currNode);
    }

    public void FillStoryWithPlayers(List<string> playerNames)
    {
        curDialogue.FillStoryWithPlayers(playerNames);
    }

    public List<Response> GetCurrentResponses()
    {
        return currNode.responses;
    }

    public void ChosenResponse(int responseIndex)
    {
        string nextNodeID = currNode.responses[responseIndex].destinationNode;
        string ChosenResponse = currNode.responses[responseIndex].displayText;
        photoDisplayObj.AddChosenText(ChosenResponse);
        postChoiceTextObj01.text = ChosenResponse;
        postChoiceTextObj02.text = ChosenResponse;

        Node nextNode = curDialogue.GetNode(nextNodeID);
        currNode = nextNode;
        EnteredNodeEvent(nextNode);
    }
}