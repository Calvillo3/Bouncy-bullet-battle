using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSpawnManager : MonoBehaviour
{

    private float spawnTimer;
    private float nextSpawn;
    int weaponCount;
    [SerializeField] GameObject[] weaponprefabs;
    [SerializeField] float[] spawnChance;
    [SerializeField] int weaponLimit;
    [SerializeField] float upperBound;
    [SerializeField] float lowerBound;
    [SerializeField] float despawnTime;
    [SerializeField] GameObject tetherlist;
    Transform[] tethertracker;
    bool rotate = true;
    

    // Start is called before the first frame update
    void Start()
    {
        rotate = true;
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

    GameObject RollPickupType() {
        float value = Random.Range(0f, 1f);
        GameObject obj = weaponprefabs[0];
        for (int i = 0; i < spawnChance.Length; i++) {
            if (value <= spawnChance[i]) {
                return obj = weaponprefabs[i];
            }
        }
        return obj;
    }

    void SpawnPickup (Transform tetherlocation) {
        GameObject weaponpickup;
        if (rotate)
        {
            weaponpickup = Instantiate(RollPickupType(), tetherlocation.position, tetherlocation.rotation * Quaternion.Euler(0, 0, 45));
        }
        else
        {
            weaponpickup = Instantiate(RollPickupType(), tetherlocation.position, tetherlocation.rotation * Quaternion.Euler(0, 0, 0));
            rotate = true;
        }
        weaponpickup.SetActive(true);
        
        spawnTimer = 0;
        weaponCount++;
    }

    public void PickedUp(GameObject obj) {
        weaponCount--;
        Destroy(obj);  
        spawnTimer = 0;
    }

}
