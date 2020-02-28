using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : ShooterMovement
{

    [SerializeField] float moveSpeed;
    [SerializeField] float turnSpeed;
    GameObject explosion;
    public bool[] bulletType = {true, false, false}; //Holds array saying which bullet type is on, currently
                                        // {default ,AK, Shotgun}

    [SerializeField] float[] shootDelay;
    float currShotDelay;

    [SerializeField] Ghost ghost;
    [SerializeField] int playerNum;
    bool readyToShoot = true;
    bool dead = false;
    float maxHealth;
    GameObject shield;
    CircleCollider2D coll;

    private List<GlowParticleScript> claimedParticles = new List<GlowParticleScript>();

    [SerializeField] float invincibleIncrement;

    public void AddCircle()
    {
        shield.SetActive(true);
        health = 2;
    }

    public void RemoveCircle()
    {
        shield.SetActive(false);
    }

    [SerializeField] float blinkPeriod;
    bool invincible = false;
    float invincibleUntilTime;
    float nextBlink;
    SpriteRenderer ren;

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
        maxHealth = health;
        currShotDelay = 0;
        invincibleUntilTime = 0;
        nextBlink = 0;
        ren = GetComponent<SpriteRenderer>();
        coll = GetComponent<CircleCollider2D>();

        shield = transform.Find("Shield").gameObject;
        shield.SetActive(false);

        UnityEngine.Object[] allParticles = Resources.FindObjectsOfTypeAll(typeof(GameObject));
        for (int i = 0; i < allParticles.Length; i++)
        {
            if (allParticles[i].name == "PlayerExplosion")
            {
                explosion = (GameObject)allParticles[i];
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetAxisRaw("Start") == 1)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if (dead) { return; }

        if (invincible)
        {
            if (Time.time > invincibleUntilTime)
            {
                invincible = false;
            }
            else
            {
                health = maxHealth;
                Blink();
            }
        }
        if(health <= 1 && shield.activeSelf)
        {
            shield.SetActive(false);
        }
        if (health <= 0 && !invincible && !dead)
        {
            Die();
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
        for (int i = 0; i < bulletType.Length; i++) {
            bulletType[i] = false;
        }
        bulletType[1] = true;
        ammoCount = 30;
    }
    public void RunShotgun() {
         for (int i = 0; i < bulletType.Length; i++) {
            bulletType[i] = false;
        }
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

    public void Revive()
    {
        dead = false;
        invincible = true;
        invincibleUntilTime = Time.time + invincibleIncrement; 
        health = maxHealth;
    }

    public void ClaimParticle(GlowParticleScript newPart)
    {
        claimedParticles.Add(newPart);
        Collider2D[] newColls = Physics2D.OverlapCircleAll(rb.position, coll.radius);
        foreach (Collider2D newColl in newColls)
        {
            NextLevelPortal potentialPortal = newColl.gameObject.GetComponent<NextLevelPortal>();
            if (potentialPortal)
            {
                potentialPortal.CheckForPlayersInside();
            }
        }
    }

    public void LoseParticle()
    {
        GlowParticleScript oldParticle = claimedParticles[0];
        claimedParticles.RemoveAt(0);
        Destroy(oldParticle.gameObject);
    }

    public int ParticleCount()
    {
        return claimedParticles.Count;
    }

    private void Die()
    {
        // Explode!
        Gradient grad = new Gradient();
        grad.SetKeys(new GradientColorKey[] { new GradientColorKey(GetComponent<SpriteRenderer>().color, 0.0f), explosion.GetComponent<ParticleSystem>().colorOverLifetime.color.gradient.colorKeys[1] }, explosion.GetComponent<ParticleSystem>().colorOverLifetime.color.gradient.alphaKeys);
        var col = explosion.GetComponent<ParticleSystem>().colorOverLifetime;
        col.color = grad;
        Instantiate(explosion, transform.position, transform.rotation).SetActive(true);

        // Put a ghost where we died and put the player in "heaven," a box far away, where the ghost was
        Vector2 ghostPos = ghost.gameObject.transform.position;
        ghost.gameObject.transform.position = rb.position;
        ghost.gameObject.SetActive(true);
        rb.position = ghostPos;

        // Remove all the glow particles on the player
        while (ParticleCount() > 0)
        {
            LoseParticle();
        }

        dead = true;
    }


    void Blink()
    {
        if (Time.time > nextBlink)
        {
            ren.enabled = !ren.enabled;
            nextBlink = Time.time + blinkPeriod;
        }
    }
}
