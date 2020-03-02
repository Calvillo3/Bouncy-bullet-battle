using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Spawner : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI waveText;
    [SerializeField] EnemyMovement[] enemies;
    [SerializeField] float timeInBetween;
    [SerializeField] int waveSelector;
    float nextSpawnTime;
    // Start is called before the first frame update
    float currtime;
    Transform[] spawners;
    GameObject currspawner;
    float diff;

    // These params can be played with at 
    // https://docs.google.com/spreadsheets/d/1IfBrU4DdlSni5aqVUrOrI0jhVTFAdG4zZzFWZSZqzjI/edit#gid=0
    // with graphs showing their effect
    Dictionary<string, float>[] spawnParams = new Dictionary<string, float>[]
    {
        // x = level, y = wave
        // Enemy 0
        // 10log(x) + 2log(y) + -1.3x + 0y + 2
        new Dictionary<string, float> 
        {
            {"log(level)", 10f},
            {"log(wave)", 2f},
            {"level", -1.3f},
            {"wave", 0f},
            {"constant", 2f} 
        },
        // Enemy 1
        // 1log(x) + 1log(y) + 0.1x + 0.1y + 0
        new Dictionary<string, float> 
        {
            {"log(level)", 1f},
            {"log(wave)", 1f},
            {"level", .1f},
            {"wave", .1f},
            {"constant", 0f} 
        },
        // Enemy 2 
        // -0.2log(x) + 1log(y) + 0.3x + 0.1y + -1
        new Dictionary<string, float> 
        {
            {"log(level)", -.2f},
            {"log(wave)", 1f},
            {"level", 0.3f},
            {"wave", 0.1f},
            {"constant", -1} 
        },
        // Enemy 3
        // 0log(x) + 0log(y) + 0.5x + 0.5y + -5
        new Dictionary<string, float> 
        {
            {"log(level)", 0f},
            {"log(wave)", 0f},
            {"level", 0.5f},
            {"wave", 0.5f},
            {"constant", -5} 
        },
    };

    Dictionary<string, int[]> hardCodedEnemies = new Dictionary<string, int[]>
    {
        //{ "1.2", new int[] { 1 }},
        //{ "1.3", new int[] { 2 }},
        //{ "1.4", new int[] { 3 }},
    };



    int[][] waveEnemies = new int[][]
    {
        new int[] { }
    };

    int[][] waveEnemies0 = new int[][]
    {
        new int[] { 0 },
        new int[] { 0, 0},
        new int[] { 0, 0, 0},
        new int[] { 0, 0, 0, 0,},
        new int[] { 0, 0, 0, 0, 0},
    };

    int[][] waveEnemies1 = new int[][]
    {
        new int[] { 0 },
        new int[] { 0, 0, 0},
        new int[] { 1, 1 },
        new int[] { 0, 0, 0, 1, 1},
        new int[] { 0, 0, 0, 1, 1, 1, 1},
        new int[] { 2, 2, 2},
        new int[] { 0, 0, 0, 0, 0, 1, 1, 2, 2 },
        new int[] { 2, 2, 2, 2, 2, 2},
        new int[] { 3, 3, 3, 3 },
        new int[] { 0, 0, 1, 1, 2, 2, 3, 3},
        new int[] { 0, 1, 2, 3, 0, 1, 2, 3, 0, 0, 1, 2, 3, 0, 1, 2, 3}
    };

    int[][] waveEnemies2 = new int[][]
    {
        new int[] { 0, 0 },
        new int[] { 0, 0, 0, 0, 0 },
        new int[] { 0, 0, 0, 0, 0, 0, 0, 1, 1 },
        new int[] { 1, 1, 2, 2 },
        new int[] { 1, 2, 3, 3, 0, 0, 0 },
        new int[] { 1, 1, 1, 1, 1, 1, 1, 1, 1 },
        new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 2},
        new int[] { 0, 0, 0, 0, 2, 2, 2, 2, 2 },
        new int[] { 2, 2, 2, 2, 2, 2, 2, 2, 2 },
        new int[] { 3, 3, 3, 3, 3, 3, 3, 3, 3 },
        new int[] { 3, 1, 2, 3, 3, 1, 2, 3, 0, 3, 1, 2, 3, 3, 1, 2, 3}
    };


    void Start()
    {
        //int[][][] waveOptions = new int[][][] { waveEnemies0, waveEnemies1, waveEnemies2 };
        //waveEnemies = waveOptions[waveSelector];
        waveText = GameObject.Find("WaveText").GetComponent<TextMeshProUGUI>();
        diff = FindObjectOfType<Difficulty>().diff;
        nextSpawnTime = 0;
        spawners = gameObject.GetComponentsInChildren<Transform>();
        currtime = 2;
        waveText = GameObject.Find("WaveText").GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
       /* if (Time.time > nextSpawnTime)
        {
            nextSpawnTime = Time.time + timeInBetween;

            // Because we have the enemy reference things in scene,
            // we duplicate a deactiated enemy and activate it.
            EnemyMovement newFoe = Instantiate(enemy, transform.position, Quaternion.identity);
            newFoe.gameObject.SetActive(true);
        } */
        if (currtime > timeInBetween) {
            int waveNumber = int.Parse(waveText.text.Substring(6)) + 1;
            waveText.text = "Wave: " + (waveNumber).ToString();
            currtime = 0;

            // Previously waveEnemies referred to an array comprised of waves comprised of enemies
            // Now this only refers to the enemies for a single wave
            // This wave will either be hard coded or will need to be generated
            string waveName = waveSelector.ToString() + "." + waveNumber.ToString();
            int[] waveEnemies;
            if (hardCodedEnemies.ContainsKey(waveName))
            {
                waveEnemies = hardCodedEnemies[waveName];
            }
            else
            {
                waveEnemies = GenerateWave(waveSelector, waveNumber); //waveSelector is like level for now
            }

            for (int i = 0; i < waveEnemies.Length; i++) {
                EnemyMovement newFoe = Instantiate(enemies[waveEnemies[i]], spawners[i%spawners.Length].position, Quaternion.identity);
                newFoe.gameObject.SetActive(true);
            } 
        }
        if (GameObject.FindGameObjectWithTag("Enemy") == null) {
            currtime += Time.deltaTime;
        }
    }

    // Generate at array of enemies, using level and wave as input
    int[] GenerateWave(int level, int wave)
    {
        List<int> waveEnemies = new List<int>();
        for (int i = 0; i < enemies.Length; i++)
        {
            // Use the parameter to decide how to weight level and wave
            // Adjust based on a difficulty scalar
            int numOfEnemy = Mathf.RoundToInt( diff * (
                  spawnParams[i]["log(level)"] * Mathf.Log10(level)
                + spawnParams[i]["log(wave)"] * Mathf.Log10(wave)
                + spawnParams[i]["level"] * level
                + spawnParams[i]["wave"] * wave
                + spawnParams[i]["constant"]));

            // Don't try to make negative numbers of enemies
            numOfEnemy = Mathf.Max(numOfEnemy, 0);
            
            // reformats it to align with existing code
            for (int j = 0; j < numOfEnemy; j++)
            {
                waveEnemies.Add(i);
            }
        }
        
        // If for some reason, no enemies are to be spawn, make one weak enemy
        if (waveEnemies.Count == 0)
        {
            waveEnemies.Add(0);
        }
        string result = "List contents: ";
        foreach (var item in waveEnemies)
        {
            result += item.ToString() + ", ";
        }
        Debug.Log(result);

        // We've been playing with this as a list to expand it, but elsewhere we need an array
        return waveEnemies.ToArray();
    }
}
