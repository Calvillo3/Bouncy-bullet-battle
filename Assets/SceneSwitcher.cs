using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneSwitcher : MonoBehaviour
{
    [SerializeField] Button button1;
    [SerializeField] Button button2;
    [SerializeField] Button button3;
    // Start is called before the first frame update
    void Start()
    {
        button1.onClick.AddListener(RunGame);
        button2.onClick.AddListener(RunOptions);
        button3.onClick.AddListener(QuitGame);
    }

    // Update is called once per frame
    void RunGame() {
        //
       button1.SetActive(false);
       button2.SetActive(false);
       button3.SetActive(false);
    }   
    void RunOptions() {
        //
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
