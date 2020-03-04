using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InGameUIScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        TextMeshProUGUI p1ScoreBoard = GameObject.Find("GreenScore").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI p2ScoreBoard = GameObject.Find("BlueScore").GetComponent<TextMeshProUGUI>();
        p1ScoreBoard.SetText(GameObject.FindObjectOfType<GameStateData>().p1Kills.ToString());
        p2ScoreBoard.SetText(GameObject.FindObjectOfType<GameStateData>().p2Kills.ToString());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
