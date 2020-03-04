using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateData : MonoBehaviour
{
    public float diff;
    public string mode;
    public int p1Kills;
    public int p2Kills;
    public int p1Wins;
    public int p2Wins;

    private void Awake()
    {
        GameStateData[] diffs = FindObjectsOfType<GameStateData>();
        p1Kills = 0;
        p2Kills = 0;
        p1Wins = 0;
        p2Wins = 0;
        if (diffs.Length > 1)
        {
            Destroy(this.gameObject);
        }
        else
        {
            DontDestroyOnLoad(this.gameObject);
        }
    }

    public void Restart()
    {
        p1Kills = 0;
        p2Kills = 0;
        p1Wins = 0;
        p2Wins = 0;
    }

}
