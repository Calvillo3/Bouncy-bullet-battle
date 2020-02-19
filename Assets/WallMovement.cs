using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallMovement : MonoBehaviour
{
    Transform transform;
    [SerializeField] float RPM;
    float incrementAngle;
    // Start is called before the first frame update
    void Start()
    {
        incrementAngle = RPM / 60 * 360 * Time.fixedDeltaTime; 
        transform = GetComponent<Transform>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.RotateAround(transform.position, Vector3.forward, incrementAngle);
    }
}
