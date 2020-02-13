using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] EnemyMovement enemy;
    [SerializeField] float timeInBetween;
    float nextSpawnTime;
    // Start is called before the first frame update
    float currtime;
    Transform[] spawners;
    GameObject currspawner;
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

            currtime = 0;
            for (int i = 0; i < spawners.Length; i++) {
                EnemyMovement newFoe = Instantiate(enemy, spawners[i].position, Quaternion.identity);
                newFoe.gameObject.SetActive(true);
            } 
        }
        if (GameObject.FindGameObjectWithTag("Enemy") == null) {
            currtime += Time.deltaTime;
        }
    }
}
