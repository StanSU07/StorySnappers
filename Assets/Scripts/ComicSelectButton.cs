using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ComicSelectButton : MonoBehaviour
{
    [SerializeField] private StoryInfo storyInfo;
    [SerializeField] private CameraPreview cameraPreview;
    public TextMeshProUGUI storyTextMesh;
    public TextMeshProUGUI storyTextMesh2;

    public Image backgroundPanel;
    public Image endPanel;





    private DialogueController dialogueController;

    // Dictionary to map story names to colors
    private Dictionary<string, Color> storyColors;

    private void OnEnable()
    {
        dialogueController = FindObjectOfType<DialogueController>();
        // Initialize the story name to color mappings
        storyColors = new Dictionary<string, Color>
        {
            { "The Heist", new Color(86f / 255f, 147f / 255f, 1f / 255f, 1f) },
            { "Knight and Princess", new Color(1f / 255f, 101f / 255f, 147f / 255f, 1f) },
            { "The Circus Thief", new Color(217f / 255f, 190f / 255f, 13f / 255f, 1f) }
        };
    }
    public void SelectStory()
    {
        dialogueController.InitializeDialogue(storyInfo);
        cameraPreview.UpdateStoryName(storyInfo.storyName);
        storyTextMesh.text = storyInfo.storyName;
        storyTextMesh2.text = storyInfo.storyName;

        // Check if there's a color mapping for the story name
        if (storyColors.ContainsKey(storyInfo.storyName))
        {
            Color storyColor = storyColors[storyInfo.storyName];
            backgroundPanel.color = storyColor;
            endPanel.color = storyColor;

        }
        else
        {
            // Use a default color if there's no specific mapping
            backgroundPanel.color = Color.white;
        }
    }
}

[System.Serializable]
public struct StoryInfo 
{
    public int numberOfPlayers;
    public List<string> characterNames;
    public string storyName;
}

