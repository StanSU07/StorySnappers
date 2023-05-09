using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.Experimental.AI;
using System.Collections.Generic;
using TMPro;

public class PhotoDisplay : MonoBehaviour
{
    public Image[] photoSlots; // Array of Image components for displaying photos
    private string[] photoPaths; // Array of file paths for saved photos
    
    
    public List<string> chosenTexts = new List<string>();
    public TextMeshProUGUI[] choiceTextObjs;

    public void SetPhotoDisplayPage()
    {
        SetPhotoDisplayImages();
        SetPhotoDisplayTexts();
    }

    void SetPhotoDisplayImages ()
    {
        // Load photo paths into array
        photoPaths = Directory.GetFiles(Application.persistentDataPath, "*.jpg");

        // Sort paths in ascending order (assuming file names are in format "photo_<storyName>_<photoCount>.jpg")
        System.Array.Sort(photoPaths);

        // Load photos into Image components
        for (int i = 0; i < photoPaths.Length && i < photoSlots.Length; i++)
        {
            byte[] photoData = File.ReadAllBytes(photoPaths[i]);
            Texture2D photoTexture = new Texture2D(2, 2);
            photoTexture.LoadImage(photoData);
            photoSlots[i].sprite = Sprite.Create(photoTexture, new Rect(0, 0, photoTexture.width, photoTexture.height), Vector2.zero);
        }
    }

    private void SetPhotoDisplayTexts()
    {
        for (int i = 0; i < choiceTextObjs.Length;i++)
        {
            choiceTextObjs[i].text = chosenTexts[i];
        }
    }

    public void AddChosenText(string choiceText)
    {
        chosenTexts.Add(choiceText);
    }
}
