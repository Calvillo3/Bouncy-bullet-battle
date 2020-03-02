using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Difficulty : MonoBehaviour
{
    public float diff;
    private void Awake()
    {
        Difficulty[] diffs = FindObjectsOfType<Difficulty>();

        if (diffs.Length > 1)
        {
            Destroy(this.gameObject);
        }
        else
        {
            DontDestroyOnLoad(this.gameObject);
        }
    }

}
