using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Resetti : MonoBehaviour
{
    [SerializeField] PlayerMovement importantPlayer;
    float resetTime;

    // Start is called before the first frame update
    void Start()
    {
        resetTime = Mathf.Infinity;
    }

    // Update is called once per frame
    void Update()
    {
        if (importantPlayer.health == 0 && resetTime == Mathf.Infinity)
        {
            resetTime = Time.time + 1;
        }
        if (Time.time > resetTime)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
