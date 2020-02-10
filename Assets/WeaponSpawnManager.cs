using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSpawnManager : MonoBehaviour
{

    private float spawnTimer;
    private float nextSpawn;
    int weaponCount;
    [SerializeField] GameObject weaponprefab;
    [SerializeField] int weaponLimit;
    [SerializeField] float upperBound;
    [SerializeField] float lowerBound;
    [SerializeField] float despawnTime;
    [SerializeField] GameObject tetherlist;
    Transform[] tethertracker;

    // Start is called before the first frame update
    void Start()
    {
        spawnTimer = 0;
        weaponCount = 0;
        tethertracker = tetherlist.GetComponentsInChildren<Transform>();

    }

    // Update is called once per frame
    void Update()
    {
        spawnTimer += Time.deltaTime;
        if (CheckSpawn()) {
            SpawnPickup(Findspawner());
        }

    }

    bool CheckSpawn() {
        nextSpawn = Random.Range(lowerBound, upperBound);
        if (spawnTimer >= nextSpawn && weaponCount < weaponLimit) {
            return true;
        }
        else if (spawnTimer >= nextSpawn && weaponCount >= weaponLimit) {
            spawnTimer = 0;
            return false;
        }
        else return false;
    }

    Transform Findspawner() {
        int randomspot = Random.Range(0, tethertracker.Length - 1);
        return tethertracker[randomspot];
    }

    void SpawnPickup (Transform tetherlocation) {
        GameObject weaponpickup = Instantiate(weaponprefab, tetherlocation.position, tetherlocation.rotation);
        spawnTimer = 0;
        weaponCount++;
    }

}
