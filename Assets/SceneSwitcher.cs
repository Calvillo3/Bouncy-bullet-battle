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
    [SerializeField] Button onePlayerButton;

    GameStateData diff;
    bool onePlayer = false;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        onePlayer = false;
        button1.onClick.AddListener(RunGame);
        button2.onClick.AddListener(RunTutorial);
        button3.onClick.AddListener(QuitGame);
        button4.onClick.AddListener(PlayCoop);
        button5.onClick.AddListener(PlayComp);
        button6.onClick.AddListener(PlayEasy);
        button7.onClick.AddListener(PlayMedium);
        button8.onClick.AddListener(PlayHard);
        onePlayerButton.onClick.AddListener(PlayOnePlayer);

        diff = FindObjectOfType<GameStateData>();
    }
    void Update() {
        if(Input.GetButtonDown("BackButton")) {
            if(button4.gameObject.activeSelf) {
            button1.gameObject.SetActive(true);
            button2.gameObject.SetActive(true);
            button3.gameObject.SetActive(true);
            onePlayerButton.gameObject.SetActive(false);
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
                onePlayerButton.gameObject.SetActive(true);
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
        if (onePlayer)
        {
            diff.mode = "Single";
        }
        else
        {
            diff.mode = "Co-Op";
        }
        diff.diff = .5f;
        SceneManager.LoadScene("Level 1");
    }
    void PlayMedium() {
        if (onePlayer)
        {
            diff.mode = "Single";
        }
        else
        {
            diff.mode = "Co-Op";
        }
        diff.diff = 1f;
        SceneManager.LoadScene("Level 1");
    }
    void PlayHard() {
        if (onePlayer)
        {
            diff.mode = "Single";
        }
        else
        {
            diff.mode = "Co-Op";
        }
        diff.diff = 1.5f;
        SceneManager.LoadScene("Level 1");
    }
    void PlayComp() {
        diff.mode = "Comp";
        diff.diff = 1.0f;
        SceneManager.LoadScene(Random.Range(1, 11));
    }
    void RunGame() {
        //
       button1.gameObject.SetActive(false);
       button2.gameObject.SetActive(false);
       button3.gameObject.SetActive(false);
       button4.gameObject.SetActive(true);
       button5.gameObject.SetActive(true);
       onePlayerButton.gameObject.SetActive(true);
       button4.Select();
       button4.OnSelect(null);

    
    }   
    void PlayCoop() {
       onePlayerButton.gameObject.SetActive(false);
       onePlayer = false;
       button4.gameObject.SetActive(false);
       button5.gameObject.SetActive(false);
       button6.gameObject.SetActive(true);
       button7.gameObject.SetActive(true);
       button8.gameObject.SetActive(true);
       button6.Select();
       button6.OnSelect(null);
    }

    void PlayOnePlayer()
    {
        onePlayer = true;
        onePlayerButton.gameObject.SetActive(false);
        button4.gameObject.SetActive(false);
        button5.gameObject.SetActive(false);
        button6.gameObject.SetActive(true);
        button7.gameObject.SetActive(true);
        button8.gameObject.SetActive(true);
        button6.Select();
        button6.OnSelect(null);
    }

    void RunTutorial() {
        diff.mode = "Tutorial";
        SceneManager.LoadScene("Tutorial 1");
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
