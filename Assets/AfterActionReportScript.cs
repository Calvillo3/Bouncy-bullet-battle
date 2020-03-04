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
    [SerializeField] Button button3;

    // Start is called before the first frame update
    void Start()
    {
        // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        if (GameObject.FindObjectOfType<GameStateData>().mode == "Comp")
        {
            button1.onClick.AddListener(NextCompLevel);
        }
        else
        {
            button1.onClick.AddListener(Restart);
        }
        ;
        button2.onClick.AddListener(BackToMenu);
        button3.onClick.AddListener(QuitGame);
    }

    void NextCompLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    void Restart()
    {
        // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        if (GameObject.FindObjectOfType<GameStateData>().mode == "Co-Op")
        {
            SceneManager.LoadScene("Level 1");
        } else
        {
            SceneManager.LoadScene("Comp 1");
        }
        GameObject.FindObjectOfType<GameStateData>().Restart();
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
