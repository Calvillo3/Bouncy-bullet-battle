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
    float nextSpawnTime;
    // Start is called before the first frame update
    float currtime;
    Transform[] spawners;
    GameObject currspawner;
    int[][] waveEnemies = new int[][]
    {
        new int[] { 0 },
        new int[] { 0, 0, 0, 0, 0 },
        new int[] { 0, 0, 0, 0, 0, 0, 0, 1, 1 },
        new int[] { 1, 1, 2, 2 },
        new int[] { 1, 2, 3, 3, 0, 0, 0 },
        new int[] { 1, 1, 1, 1, 1, 1, 1, 1, 1 },
        new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        new int[] { 0, 0, 0, 0, 2, 2, 2, 2, 2 },
        new int[] { 2, 2, 2, 2, 2, 2, 2, 2, 2 },
        new int[] { 3, 3, 3, 3, 3, 3, 3, 3, 3 },
        new int[] { 0, 1, 2, 3, 0, 1, 2, 3, 0, 0, 1, 2, 3, 0, 1, 2, 3}
    };
    void Start()
    {
        nextSpawnTime = 0;
        spawners = gameObject.GetComponentsInChildren<Transform>();
        currtime = 2;
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
            int waveNumber = int.Parse(waveText.text.Substring(6));
            waveText.text = "Wave: " + (waveNumber + 1).ToString();
            currtime = 0;
            // if they've reached the last wave, just repeat it till they lose lol
            waveNumber = Mathf.Min(waveNumber, waveEnemies.Length - 1);
            for (int i = 0; i < waveEnemies[waveNumber].Length; i++) {
                EnemyMovement newFoe = Instantiate(enemies[waveEnemies[waveNumber][i]], spawners[i%spawners.Length].position, Quaternion.identity);
                newFoe.gameObject.SetActive(true);
            } 
        }
        if (GameObject.FindGameObjectWithTag("Enemy") == null) {
            currtime += Time.deltaTime;
        }
    }
}
