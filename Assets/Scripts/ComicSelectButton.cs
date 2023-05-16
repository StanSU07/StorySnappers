using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComicSelectButton : MonoBehaviour
{
    [SerializeField] private StoryInfo storyInfo;
    [SerializeField] private CameraPreview cameraPreview;

    private DialogueController dialogueController;

    private void OnEnable()
    {
        dialogueController = FindObjectOfType<DialogueController>();
    }
    public void SelectStory()
    {
        dialogueController.InitializeDialogue(storyInfo);
        cameraPreview.UpdateStoryName(storyInfo.storyName);
    }
}

[System.Serializable]
public struct StoryInfo 
{
    public int numberOfPlayers;
    public List<string> characterNames;
    public string storyName;
}

