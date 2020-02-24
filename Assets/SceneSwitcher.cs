using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using TMPro;

public class SceneSwitcher : MonoBehaviour
{
    [SerializeField] Button button1;
    [SerializeField] Button button2;
    [SerializeField] Button button3;
    [SerializeField] Button button4;
    [SerializeField] Button button5;
    [SerializeField] Button button6;
    [SerializeField] Button button7;
    [SerializeField] Button button8;

    // Start is called before the first frame update
    void Start()
    {
        button1.onClick.AddListener(RunGame);
        button2.onClick.AddListener(RunOptions);
        button3.onClick.AddListener(QuitGame);
        button4.onClick.AddListener(PlayCoop);
        button6.onClick.AddListener(PlayEasy);
        button7.onClick.AddListener(PlayMedium);
        button8.onClick.AddListener(PlayHard);
    }
    void Update() {
        if(Input.GetButtonDown("BackButton")) {
            if(button4.gameObject.activeSelf) {
            button1.gameObject.SetActive(true);
            button2.gameObject.SetActive(true);
            button3.gameObject.SetActive(true);
            button4.gameObject.SetActive(false);
            button5.gameObject.SetActive(false);
            button4.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
            button5.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
            button1.Select();
            button1.OnSelect(null);
            }
            if(button6.gameObject.activeSelf) {
                button4.gameObject.SetActive(true);
                button5.gameObject.SetActive(true);
                button6.gameObject.SetActive(false);
                button7.gameObject.SetActive(false);
                button8.gameObject.SetActive(false);
                button6.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
                button7.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
                button8.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
                button4.Select();
                button4.OnSelect(null);

            }
        }
    }

    // Update is called once per frame
    void PlayEasy() {
        SceneManager.LoadScene("Easy");
    }
    void PlayMedium() {
        SceneManager.LoadScene("Medium");
    }
    void PlayHard() {
        SceneManager.LoadScene("Hard");
    }
    void RunGame() {
        //
       button1.gameObject.SetActive(false);
       button2.gameObject.SetActive(false);
       button3.gameObject.SetActive(false);
       button4.gameObject.SetActive(true);
       button5.gameObject.SetActive(true);
       button4.Select();
       button4.OnSelect(null);

    
    }   
    void PlayCoop() {
       button4.gameObject.SetActive(false);
       button5.gameObject.SetActive(false);
       button6.gameObject.SetActive(true);
       button7.gameObject.SetActive(true);
       button8.gameObject.SetActive(true);
       button6.Select();
       button6.OnSelect(null);
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
