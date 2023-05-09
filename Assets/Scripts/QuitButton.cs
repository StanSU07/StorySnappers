using UnityEngine;
using UnityEngine.UI;

public class QuitButton : MonoBehaviour
{
    void Start()
    {
        Button btn = GetComponent<Button>();
        btn.onClick.AddListener(QuitApplication);
    }

    public void QuitApplication()
    {
        Application.Quit();
    }
}
