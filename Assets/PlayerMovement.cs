using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : ShooterMovement
{

    [SerializeField] float moveSpeed;
    [SerializeField] float turnSpeed;

    [SerializeField] int playerNum;
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
        if (health <= 0)
        {
            // Right now destorying a single player causes a crash...
            // This should be fixed, but we need to decide if the game should end
            // After one player dies, or after both players die
            Destroy(this.gameObject);
        }

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
            Fire();
        }
    }

    void FixedUpdate()
    { 
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        rb.SetRotation(rb.rotation - turn * turnSpeed * Time.fixedDeltaTime);
    }
}
