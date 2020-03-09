using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroySoundOnLoad : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    void Awake()
    {
        AudioSource[] sounds = FindObjectsOfType<AudioSource>();

        if (sounds.Length > 1)
        {
            Debug.Log("KILL");
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}