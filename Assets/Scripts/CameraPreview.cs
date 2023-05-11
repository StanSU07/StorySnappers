using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Collections.Generic;
using TMPro;
using System.Linq;

public class CameraPreview : MonoBehaviour
{
    public RawImage previewImage;
    public RawImage capturedImage;
    public Button switchButton;
    private WebCamTexture camTexture;
    public List<WebCamDevice> validCameraDevices = new List<WebCamDevice>();
    public List<string> validCameraNames = new List<string>();
    private int currentCameraIndex = 0;

    public StretchImage stretchImage;

    private int photoCount;
    private string storyName;

    private void Start()
    {

        int camCount = WebCamTexture.devices.Length;
        for (int i = 0; i < camCount; i++)
        {
            Debug.Log("Camera " + i + " name: " + WebCamTexture.devices[i].name);
            if (validCameraNames.Contains(WebCamTexture.devices[i].name))
            {
                validCameraDevices.Add(WebCamTexture.devices[i]);
            }
        }

        // Start the camera preview using the current camera index
        SwitchCamera();

        // Set the preview texture of the specified RawImage component
        previewImage.texture = camTexture;

        // Assign the switchCamera method to the button's onClick event
        switchButton.onClick.AddListener(SwitchCamera);
    }

    private void SwitchCamera()
    {
        // Stop the current camera and start the new one
        if (camTexture != null)
        {
            camTexture.Stop();
        }
        currentCameraIndex = (currentCameraIndex + 1) % validCameraDevices.Count;

        camTexture = new WebCamTexture(validCameraDevices[currentCameraIndex].name);

        //Set the camera's orientation to flip the image back to the correct orientation
        Quaternion camRotation = Quaternion.identity;
        if (validCameraDevices[currentCameraIndex].isFrontFacing)
        {
            camRotation.eulerAngles = new Vector3(0, 180, 90);
        }
        else
        {
            camRotation.eulerAngles = new Vector3(0, 0, 270);
        }
        previewImage.transform.localRotation = camRotation;


        camTexture.Play();

        // Set the preview texture of the specified RawImage component
        previewImage.texture = camTexture;

        if (validCameraDevices[currentCameraIndex].isFrontFacing)
        {
            camRotation.eulerAngles = new Vector3(0, 180, 90);
            capturedImage.transform.localRotation = camRotation;

        }
        else
        {
            camRotation.eulerAngles = new Vector3(0, 0, 270);
            capturedImage.transform.localRotation = camRotation;
        }
    }

    public void TakePhoto()
    {

        // Capture a photo from the camera feed
        Texture2D photoTexture = new Texture2D(camTexture.width, camTexture.height);

        // Copy the pixels from the camera feed to the photo texture
        Color[] pixels = camTexture.GetPixels();

        // Update the photo texture with the rotated pixels
        photoTexture.SetPixels(pixels);
        photoTexture.Apply();

        // Set the captured texture of the specified RawImage component
        capturedImage.texture = photoTexture;

        // Save the photo to disk based on camera facing direction
        byte[] photoData = photoTexture.EncodeToJPG(100);
        if (validCameraDevices[currentCameraIndex].isFrontFacing)
        {
            File.WriteAllBytes(Application.persistentDataPath + "/front_facing_photo_" + storyName + "_" + photoCount + ".jpg", photoData);
        }
        else
        {
            File.WriteAllBytes(Application.persistentDataPath + "/back_facing_photo_" + storyName + "_" + photoCount + ".jpg", photoData);
        }

        // Increment photo count
        photoCount++;
    }


    public void UpdateStoryName(string name) {
        storyName = name;
        photoCount = 0;
    }
}
