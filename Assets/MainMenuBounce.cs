using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuBounce : MonoBehaviour
{

    Color32[] colorArray = {new Color32(0 ,228, 255, 255), new Color32(0 ,255, 40, 255), new Color32(255, 78, 0, 255)};
    [SerializeField] float timeBulletSpawn;
    [SerializeField] Tethers tetherlist;
    Transform[] tethertracker;
    [SerializeField] BulletMM prefab;
    float timer;
    int randomtether;
    int randomcolor;
    int randomangle;
    Vector3 angle;
    void Start()
    {
        tethertracker = tetherlist.GetComponentsInChildren<Transform>();
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (timer >= timeBulletSpawn) {
            //do stuff
            randomtether = Random.Range(0, colorArray.Length);
            randomcolor = Random.Range(0, colorArray.Length);
            randomangle = Random.Range(0, 359);
            
            angle = new Vector3(0, 0, randomangle);
            BulletMM pewpew = Instantiate(prefab, tethertracker[randomcolor].position, Quaternion.Euler(angle));
            pewpew.GetComponent<SpriteRenderer>().color = colorArray[randomcolor];
            timer = 0;
        }
        else {
            timer += Time.deltaTime;
        }
    }
}
