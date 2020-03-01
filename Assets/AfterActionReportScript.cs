using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using TMPro;

public class AfterActionReportScript : MonoBehaviour
{
    [SerializeField] Button button1;
    [SerializeField] Button button2;

    // Start is called before the first frame update
    void Start()
    {
        button1.onClick.AddListener(BackToMenu);
        button2.onClick.AddListener(QuitGame);
    }
    void Update()
    {

    }

    void BackToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
    }
}
