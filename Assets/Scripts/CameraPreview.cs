using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Collections.Generic;
using TMPro;

public class CameraPreview : MonoBehaviour
{
    public RawImage previewImage;
    public RawImage capturedImage;
    public Button switchButton;
    private WebCamTexture camTexture;
    public List<WebCamDevice> validCameraDevices = new List<WebCamDevice>();
    public List<string> validCameraNames = new List<string>();
    private int currentCameraIndex = 0;

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
        
        // Check if any cameras are available
        if (validCameraDevices.Count == 0)
        {
            Debug.LogWarning("No cameras detected.");
            return;
        } 
        //else
        //{
        //    string names = "";
        //    validCameraDevices.ForEach(cam => names += cam.name + ", ");
        //    camNameText.text = "cam names: " + names;
        //}

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
            camRotation.eulerAngles = new Vector3( 180, 0, 0);
        }
        else
        {
            camRotation.eulerAngles = new Vector3(0, 0, 180);
        }
        previewImage.transform.localRotation = camRotation;


        camTexture.Play();

    // Set the preview texture of the specified RawImage component 0,180,90 / 0,0,-90
    previewImage.texture = camTexture;

    if (validCameraDevices[currentCameraIndex].isFrontFacing)
    {
        camRotation.eulerAngles = new Vector3(180, 0, 0);
        capturedImage.transform.localRotation = camRotation;

    }
    else
    {
        camRotation.eulerAngles = new Vector3(0, 0, 180);
        capturedImage.transform.localRotation = camRotation;

    }


    }

    public void TakePhoto()
    {
        // Capture a photo from the camera feed
        Texture2D photoTexture = new Texture2D(camTexture.width, camTexture.height);
        photoTexture.SetPixels(camTexture.GetPixels());
        photoTexture.Apply();

        // Set the captured texture of the specified RawImage component
        capturedImage.texture = photoTexture;

        // Save the photo to disk

        byte[] photoData = photoTexture.EncodeToJPG(100);
        File.WriteAllBytes(Application.persistentDataPath + "/photo_" + storyName+ "_" + photoCount + ".jpg", photoData);
        photoCount++;
    }

    public void UpdateStoryName(string name) {
        storyName = name;
        photoCount = 0;
    }
}
