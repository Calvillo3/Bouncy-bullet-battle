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

    int currindex;
    int index;
    bool done;


    // Start is called before the first frame update
    void Start()
    {
        done = false;
        currindex = SceneManager.GetActiveScene().buildIndex;
        if (GameObject.FindObjectOfType<GameStateData>().mode == "Comp")
        {
            button1.onClick.AddListener(NextCompLevel);
        }
        else
        {
            button1.onClick.AddListener(Restart);
        }
        
        button2.onClick.AddListener(BackToMenu);
        button3.onClick.AddListener(QuitGame);
    }

    void NextCompLevel()
    {
        while(!done) {
            index = Random.Range(1, 11); //these values need to be hardcoded according to build index
            //make sure you don't do a repeat scene
            if (index != currindex) {  
                done = true;
            }
        }
        SceneManager.LoadScene(index);
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    void Restart()
    {
        // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        /* if (GameObject.FindObjectOfType<GameStateData>().mode == "Co-Op")
        {
            SceneManager.LoadScene("Level 1");
        } else
        {
            SceneManager.LoadScene("Comp 1");
        }*/
        SceneManager.LoadScene("Level 1");
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
