using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] float moveSpeed;
    [SerializeField] float turnSpeed;
    [SerializeField] Bullet bullet;

    [SerializeField] int playerNum;

    public Rigidbody2D rb;
    bool readyToShoot = true;

    Vector2 movement;

    float turn;

    float angle;
    float horizontal;
    float vertical;
    float prevangle;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
        movement.x = Input.GetAxisRaw("HorizontalPlayer" + playerNum);
        movement.y = Input.GetAxisRaw("VerticalPlayer" + playerNum) * -1;
        /*
        turn = Input.GetAxis("HorizontalJoyRPlayer" + playerNum);
        */
        horizontal = Input.GetAxis("HorizontalJoyRPlayer" + playerNum) * turnSpeed * Time.fixedDeltaTime;
        vertical = Input.GetAxis("VerticalJoyRPlayer" + playerNum) *  turnSpeed * Time.fixedDeltaTime;

        if ( vertical*vertical + horizontal*horizontal > .05f ) 
        {
            angle = Mathf.Atan2(-horizontal, -vertical) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }
    
        // rb.rotation = Quaternion.Euler(new Vector3(0, angle, 0));


        if (Input.GetAxisRaw("Fire1Player" + playerNum) < 0.5)
        {
            readyToShoot = true;
        }

        if (Input.GetAxisRaw("Fire1Player" + playerNum) >= 0.99 && readyToShoot)
        {
            readyToShoot = false;
            // Shoots a bullet in the direction the player is facing
            Bullet pewpew = Instantiate(bullet,rb.position,rb.transform.rotation);
            // We need to tell the bullet we shot it so it doesn't hurt the player... at first
            pewpew.Claim(this.gameObject);
        }
    }

    void FixedUpdate()
    { 
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        //original//  rb.SetRotation(rb.rotation - turn * turnSpeed * Time.fixedDeltaTime);
       // if (angle*angle > 0.01f)
        //{
        //    rb.SetRotation(-angle);
        //}
       // rb.SetRotation(rb.rotation - angle * turnSpeed * Time.fixedDeltaTime);


    }

    public void TakeDamage(int amount)
    {
        Debug.Log("Ow! at " + Time.time + " seconds.");
        //TODO Give the player health which ticks down after getting hit.
    }
}
