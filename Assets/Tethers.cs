using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tethers : MonoBehaviour
{
    [SerializeField] GameObject tethers;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    void OnDrawGizmos() {
        Gizmos.color = Color.magenta;
        foreach (Transform tether in tethers.GetComponentsInChildren<Transform>()) {
            Gizmos.DrawSphere(tether.transform.position, .15f);
        }
    }

    // Update is called once per frame
    void Update()
    {
    
    }

}
   
