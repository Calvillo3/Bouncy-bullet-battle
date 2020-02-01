using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] float moveSpeed;
    [SerializeField] float turnSpeed;
    [SerializeField] Bullet bullet;

    public Rigidbody2D rb;

    Vector2 movement;

    float turn;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        turn = Input.GetAxis("HorizontalJoyR");

        if(Input.GetButtonDown("Fire1"))
        {
            // Shoots a bullet in the direction the player is facing
            Bullet pewpew = Instantiate(bullet,rb.position,rb.transform.rotation);
            // We need to tell the bullet we shot it so it doesn't hurt the player... at first
            pewpew.Claim(this.gameObject);
        }
    }

    void FixedUpdate()
    { 
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        rb.SetRotation(rb.rotation - turn * turnSpeed * Time.fixedDeltaTime);
    }

    public void TakeDamage(int amount)
    {
        Debug.Log("Ow! at " + Time.time + " seconds.");
        //TODO Give the player health which ticks down after getting hit.
    }
}
