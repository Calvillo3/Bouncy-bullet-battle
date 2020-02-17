using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AKPickup : MonoBehaviour
{
    GameObject lst;
    void Start() {
        lst = GameObject.Find("/Tethers");
    }

    void Update() {
        transform.Rotate(0, 0, 100 * Time.deltaTime);
    }
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D col) {
        if(col.gameObject.tag == "Player") {
        lst.GetComponent<WeaponSpawnManager>().PickedUp(gameObject);
        col.gameObject.GetComponent<PlayerMovement>().RunAK();
        }
    }
}
