using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class player2ammo : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject player2;
    public TextMeshProUGUI scoreText;
    // Update is called once per frame
    void Update()
    {
        scoreText.text = player2.GetComponent<PlayerMovement>().ammoCount.ToString(); 
    }
}
