using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetMenus : MonoBehaviour
{
    [SerializeField] private List<GameObject> turnOff;
    [SerializeField] private List<GameObject> turnOn;

    private void Awake()
    {
        ResetGame();
    }
    public void ResetGame()
    {
        foreach (GameObject panel in turnOff) 
        {
            panel.SetActive(false);
        }

        foreach (GameObject panel in turnOn)
        {
            panel.SetActive(true);
        }
    }
}
