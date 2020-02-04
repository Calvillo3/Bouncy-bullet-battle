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

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        movement.x = Input.GetAxisRaw("HorizontalPlayer" + playerNum);
        movement.y = Input.GetAxisRaw("VerticalPlayer" + playerNum) * -1;

        turn = Input.GetAxis("HorizontalJoyRPlayer" + playerNum);

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
        rb.SetRotation(rb.rotation - turn * turnSpeed * Time.fixedDeltaTime);
    }

    public void TakeDamage(int amount)
    {
        Debug.Log("Ow! at " + Time.time + " seconds.");
        //TODO Give the player health which ticks down after getting hit.
    }
}
