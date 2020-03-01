using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using TMPro;

public class PauseMenuScript : MonoBehaviour
{
    [SerializeField] Button button1;
    [SerializeField] Button button2;
    [SerializeField] Button button3;

    // Start is called before the first frame update
    void Start()
    {
        button1.onClick.AddListener(Unpause);
        button2.onClick.AddListener(BackToMenu);
        button3.onClick.AddListener(QuitGame);
    }
    void Update()
    {
        
    }

    // Update is called once per frame
    void Unpause()
    {
        Time.timeScale = 1.0f;
        Destroy(gameObject);
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
