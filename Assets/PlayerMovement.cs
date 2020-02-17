using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : ShooterMovement
{

    [SerializeField] float moveSpeed;
    [SerializeField] float turnSpeed;

    bool[] bulletType = {true, false, false}; //Holds array saying which bullet type is on, currently
                                        // {default ,AK, Shotgun}

    [SerializeField] float[] shootDelay;
    float currShotDelay;

    [SerializeField] int playerNum;
    bool readyToShoot = true;
    bool dead = false;

    Vector2 movement;

    float turn;

    float angle;
    float horizontal;
    float vertical;
    float prevangle;
    public int ammoCount;

    // Start is called before the first frame update
    void Start()
    {
        currShotDelay = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            // The player goes to heaven. 
            // Which is a box far away
            rb.position = 21.5f * Vector2.up;
            dead = true;
            //Destroy(this.gameObject);
        }

        if (Input.GetAxisRaw("Start") == 1)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        
        movement.x = Input.GetAxisRaw("HorizontalPlayer" + playerNum);
        movement.y = Input.GetAxisRaw("VerticalPlayer" + playerNum) * -1;
        movement = Vector2.ClampMagnitude(movement, 1);
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
       
        

        if (bulletType[0]) {
        if (Input.GetAxisRaw("Fire1Player" + playerNum) < 0.5 && currShotDelay >= shootDelay[0])
        {
            readyToShoot = true;
        }

        if (Input.GetAxisRaw("Fire1Player" + playerNum) >= 0.99 && readyToShoot)
        {
            readyToShoot = false;
            currShotDelay = 0;
            Fire();
        }
        }
        else if (bulletType[1]) {
            
            if (Input.GetAxisRaw("Fire1Player" + playerNum) >= 0.99 && currShotDelay >= shootDelay[1]) {
                currShotDelay = 0;
                Fire();
                ammoCount--;
                
            }
            if(ammoCount <= 0) {
                bulletType[0] = true;
                bulletType[1] = false;
                ammoCount = 0;
                currShotDelay = 0;
                return;
            }
        }
        else if (bulletType[2]) {
            if (ammoCount <= 0) {
                bulletType[0] = true;
                bulletType[2] = false;
                ammoCount = 0;
            }
            if (Input.GetAxisRaw("Fire1Player" + playerNum) < 0.5 && currShotDelay >= shootDelay[2])
            {
                readyToShoot = true;
            }
            if (Input.GetAxisRaw("Fire1Player" + playerNum) >= 0.99 && readyToShoot) {
                readyToShoot = false;
                currShotDelay = 0;
                ShotgunFire();
                ammoCount--;
            }
        }
        currShotDelay += Time.deltaTime;
    }
    public void RunAK() {
        bulletType[0] = false;
        bulletType[1] = true;
        ammoCount = 30;
    }
    public void RunShotgun() {
        bulletType[0] = false;
        bulletType[2] = true;
        ammoCount = 10;
    }

    void FixedUpdate()
    { 
        if (!dead)
        {
            rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        }
        //original//  rb.SetRotation(rb.rotation - turn * turnSpeed * Time.fixedDeltaTime);
       // if (angle*angle > 0.01f)
        //{
        //    rb.SetRotation(-angle);
        //}
       // rb.SetRotation(rb.rotation - angle * turnSpeed * Time.fixedDeltaTime);


    }
}
