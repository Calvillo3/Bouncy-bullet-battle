using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class player1ammo : MonoBehaviour
{
    // Start is called before the first frame update
   
    [SerializeField] GameObject player1;
    public TextMeshProUGUI scoreText;
    // Update is called once per frame
    void Update()
    {
        scoreText.text = player1.GetComponent<PlayerMovement>().ammoCount.ToString(); 
    }
}
