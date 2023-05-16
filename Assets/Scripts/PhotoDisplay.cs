using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.Experimental.AI;
using System.Collections.Generic;
using TMPro;
using System.Security.Cryptography.X509Certificates;

public class PhotoDisplay : MonoBehaviour
{
    public RawImage[] photoSlots; // Array of Image components for displaying photos
    private string[] photoPaths; // Array of file paths for saved photos
    
    
    public List<string> chosenTexts = new();
    public TextMeshProUGUI[] choiceTextObjs;
    public TextMeshProUGUI[] choiceTextObjs2;


    // variables for zoomed image display
    public GameObject zoomedPhotoPanel;
    public RawImage zoomedPhoto;
    private int currentZoomedIndex;

    public void OnPreviousButtonClicked()
    {
        ScrollZoomedPhotos(-1);
    }

    public void OnNextButtonClicked()
    {
        ScrollZoomedPhotos(1);
    }

    public void SetPhotoDisplayPage()
    {
        SetPhotoDisplayImages();
        SetPhotoDisplayTexts();
        SetPhotoDisplayTexts2();
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
            photoSlots[i].texture = photoTexture;

            // Extract camera type from filename
            string filename = Path.GetFileName(photoPaths[i]);
            if (filename.Contains("front_facing_photo_"))
            {
                Rect uvRect = photoSlots[i].uvRect;
                uvRect.position = new Vector2(1f, 0f);
                uvRect.size = new Vector2(1f, 0.75f);

                photoSlots[i].uvRect = uvRect;
            }
            else if (filename.Contains("back_facing_photo_"))
            {
                Rect uvRect = photoSlots[i].uvRect;
                uvRect.position = new Vector2(1f, 1.25f);
                uvRect.size = new Vector2(1f, 0.75f);

                photoSlots[i].uvRect = uvRect;
            }

        }
    }

    private void SetPhotoDisplayTexts()
    {
        for (int i = 0; i < choiceTextObjs.Length;i++)
        {
            choiceTextObjs[i].text = chosenTexts[i];
        }
    }
    private void SetPhotoDisplayTexts2()
    {
        for (int i = 0; i < choiceTextObjs2.Length; i++)
        {
            choiceTextObjs2[i].text = chosenTexts[i];
        }
    }
    private void SetZoomedPhotoDisplayText()
    {
        if (currentZoomedIndex >= 0 && currentZoomedIndex < chosenTexts.Count)
        {
            zoomedPhoto.GetComponentInChildren<TextMeshProUGUI>().text = chosenTexts[currentZoomedIndex];
        }
    }

    public void AddChosenText(string choiceText)
    {
        chosenTexts.Add(choiceText);
    }
    // displays the zoomed photo panel and sets the current photo index
    public void DisplayZoomedPhoto(int index)
    {
        currentZoomedIndex = index;
        zoomedPhotoPanel.SetActive(true);
        byte[] photoData = File.ReadAllBytes(photoPaths[index]);
        Texture2D photoTexture = new Texture2D(2, 2);
        photoTexture.LoadImage(photoData);
        zoomedPhoto.texture = photoTexture;


        // Extract camera type from filename
        string filename = Path.GetFileName(photoPaths[index]);
        if (filename.Contains("front_facing_photo_"))
        {
            Rect uvRect = zoomedPhoto.uvRect;
            uvRect.position = new Vector2(1f, 0f);
            uvRect.size = new Vector2(1f, 0.75f);

            zoomedPhoto.uvRect = uvRect;
        }
        else if (filename.Contains("back_facing_photo_"))
        {
            Rect uvRect = zoomedPhoto.uvRect;
            uvRect.position = new Vector2(1f, 1.25f);
            uvRect.size = new Vector2(1f, 0.75f);

            zoomedPhoto.uvRect = uvRect;
        }

        // Update the text displayed on the zoomed photo
        SetZoomedPhotoDisplayText();

    }

    // hides the zoomed photo panel
    public void HideZoomedPhoto()
    {
        zoomedPhotoPanel.SetActive(false);
    }

    // scrolls through the photos in the zoomed photo panel
    public void ScrollZoomedPhotos(int direction)
    {
        currentZoomedIndex += direction;
        if (currentZoomedIndex < 0)
        {
            currentZoomedIndex = photoPaths.Length - 1;
        }
        else if (currentZoomedIndex >= photoPaths.Length)
        {
            currentZoomedIndex = 0;
        }
        DisplayZoomedPhoto(currentZoomedIndex);
    }
    
    public void SavePhoto()
    {
        Texture2D photoTexture = zoomedPhoto.texture as Texture2D;
        byte[] photoData = photoTexture.EncodeToJPG();
        string filename = "photo_" + Path.GetFileNameWithoutExtension(photoPaths[currentZoomedIndex]) + ".jpg";
        string filePath = Path.Combine(Application.persistentDataPath, filename);
        File.WriteAllBytes(filePath, photoData);

        // Save to gallery
        NativeGallery.Permission permission = NativeGallery.SaveImageToGallery(photoData, "MyGallery", filename, null);
        Debug.Log("Permission result: " + permission);
    }

}
