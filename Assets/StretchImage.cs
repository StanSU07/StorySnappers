using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StretchImage : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.rect.Set(rectTransform.rect.x, rectTransform.rect.y, rectTransform.rect.height, rectTransform.rect.height);

        rectTransform.ForceUpdateRectTransforms();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeAspectRatio(int height, int width)
    {
        RawImage image = GetComponent<RawImage>();
        image.uvRect = new Rect(0, 0, (float)width / (float)width, 1);
    }
}
