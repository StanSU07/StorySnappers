using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraScript : MonoBehaviour
{

    int currentCamIndex = 0;

    WebCamTexture tex;

    public RawImage display;
    public Transform _cameraDisplayWrapperTransform;

    public void SwapCam_Clicked()
    {
        if (WebCamTexture.devices.Length > 0)
        {
            currentCamIndex += 1;
            currentCamIndex %= WebCamTexture.devices.Length;
        }

        StartStopCam_Clicked();
    }

    public void StartStopCam_Clicked()
    {
        StopCam();

        WebCamDevice device = WebCamTexture.devices[currentCamIndex];
        tex = new WebCamTexture(device.name);
        display.texture = tex;

        tex.Play();

        //_cameraDisplayWrapperTransform.rotation = Quaternion.Euler(0, 0, device.isFrontFacing ? 90 : 270);
    }
        
    public void StopCam()
    {
        if (tex != null) // Stop the camera
        {
            display.texture = null;
            tex.Stop();
            tex = null;
        }
    }
}
   
