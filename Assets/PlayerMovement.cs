﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class PlayerMovement : ShooterMovement
{
    float prevSpeed;
    [SerializeField] float dashSpeed;
    [SerializeField] float moveSpeed;
    [SerializeField] float turnSpeed;
    GameObject explosion;
    public bool[] bulletType = {true, false, false}; //Holds array saying which bullet type is on, currently
                                        // {default ,AK, Shotgun}

    [SerializeField] float[] shootDelay;
    float currShotDelay;

    [SerializeField] Ghost ghost;
    [SerializeField] public int playerNum;
    bool readyToShoot = true;
    bool dead = false;
    float maxHealth;
    CircleCollider2D coll;
    [SerializeField] public Boolean comp;
    string mode;

    public bool gameOver = false;

    GameObject pauseMenuInScene;
    [SerializeField] GameObject pauseMenuPrefab;
    [SerializeField] GameObject afterActionReportPrefab;
    [SerializeField] GameObject shield;
    [SerializeField] GameObject shieldMask;

    [SerializeField] GameObject playerArrow;

    private List<GlowParticleScript> claimedParticles = new List<GlowParticleScript>();

    [SerializeField] float invincibleIncrement;
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

    [SerializeField] AudioClip soundEffect;

    [SerializeField] GameStateData defaultGameStateData;

    bool dashActive;
    bool dashReady;
    float dashTime;
    [SerializeField] float dashtTimeLimit;

    Gradient grad;
    GameStateData gameStateObject;

    // Other gameObjects might need need these default values
    private void Awake()
    {
        gameStateObject = FindObjectOfType<GameStateData>();
        if (!gameStateObject)
        {
            gameStateObject = Instantiate(defaultGameStateData);
            if(comp)
            {
                gameStateObject.mode = "Comp";
            }
        }
        mode = gameStateObject.mode;
    }
    // Start is called before the first frame update
    void Start()
    {
        // So the game doesn't start paused
        grad = new Gradient();
        grad.SetKeys(new GradientColorKey[] { new GradientColorKey(GetComponent<SpriteRenderer>().color, 0.0f)}, new GradientAlphaKey[] {new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(0.0f, 1.0f)});
        GetComponent<TrailRenderer>().colorGradient = grad;
        GetComponent<TrailRenderer>().enabled = false;
        prevSpeed = moveSpeed;
        dashTime = 0;
        dashReady = false;
        dashActive = false;
        Time.timeScale = 1.0f;
        maxHealth = health;
        currShotDelay = 0;
        invincibleUntilTime = 0;
        nextBlink = 0;
        ren = GetComponent<SpriteRenderer>();
        coll = GetComponent<CircleCollider2D>();
        RemoveCircle();

        UnityEngine.Object[] allParticles = Resources.FindObjectsOfTypeAll(typeof(GameObject));
        for (int i = 0; i < allParticles.Length; i++)
        {
            if (allParticles[i].name == "PlayerExplosion")
            {
                explosion = (GameObject)allParticles[i];
            }
        }

        if (mode == "Single" && playerNum == 2)
        {
            health = 0;
            dead = true;
            Vector2 ghostPos = ghost.gameObject.transform.position;
            rb.position = ghostPos;
            GameObject.Find("BlueSkull").GetComponent<SpriteRenderer>().enabled = false;
            GameObject.Find("BlueAmmo").GetComponent<TextMeshProUGUI>().enabled = false;
            GameObject.Find("BlueScore").GetComponent<TextMeshProUGUI>().enabled = false;
            GameObject.Find("BlueGun").GetComponent<Image>().enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {

        
        if (!gameOver && Input.GetKeyDown("joystick " + playerNum + " button 7" ))
        {

            if(Time.timeScale < 1.0f)
            {
                Time.timeScale = 1.0f;
                Destroy(pauseMenuInScene);
            }
            else
            {
                pauseMenuInScene = GameObject.Instantiate(pauseMenuPrefab);
                pauseMenuInScene.GetComponent<Canvas>().renderMode = RenderMode.WorldSpace;
                pauseMenuInScene.GetComponent<Canvas>().worldCamera = Camera.main;
                if(SceneManager.GetActiveScene().name == "Level 1")
                {
                    pauseMenuInScene.transform.localScale *= 1.2f;
                }
                Time.timeScale = 0.0f;
            }
            // Time.timeScale = Time.timeScale < 1.0f ? 1.0f : 0.0f;
            
        }

        if (dead) { return; }
        if (Time.timeScale == 0) { return; }

        if (invincible)
        {
            if (Time.time > invincibleUntilTime)
            {
                invincible = false;
                ren.enabled = true;
            }
            else
            {
                health = maxHealth;
                Blink();
            }
        }
        if(health <= 1 && shield.activeSelf)
        {
            RemoveCircle();
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

        if (dashActive) {
            dashTime += Time.deltaTime;
            if(dashTime >= dashtTimeLimit) {
                dashActive = false;
                dashTime = 0;
                moveSpeed = prevSpeed;
                GetComponent<TrailRenderer>().enabled = false;
            }
        }

        // rb.rotation = Quaternion.Euler(new Vector3(0, angle, 0));
        if (dashReady) {
            if (Input.GetAxisRaw("Dash1Player" + playerNum) >= 0.99) {
                dashReady = false;
                dashActive = true;
                GetComponent<TrailRenderer>().enabled = true;
                moveSpeed = dashSpeed;
                playerArrow.GetComponent<SpriteRenderer>().color = Color.black;
            }
        }

        if (bulletType[0])
        {
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
        else if (bulletType[1])
        {

            if (Input.GetAxisRaw("Fire1Player" + playerNum) >= 0.99 && currShotDelay >= shootDelay[1])
            {
                currShotDelay = 0;
                Fire();
                ammoCount--;

            }
            if (ammoCount <= 0)
            {
                bulletType[0] = true;
                bulletType[1] = false;
                ammoCount = 0;
                currShotDelay = 0;
                return;
            }
        }
        else if (bulletType[2])
        {
            if (ammoCount <= 0)
            {
                bulletType[0] = true;
                bulletType[2] = false;
                ammoCount = 0;
            }
            if (Input.GetAxisRaw("Fire1Player" + playerNum) < 0.5 && currShotDelay >= shootDelay[2])
            {
                readyToShoot = true;
            }
            if (Input.GetAxisRaw("Fire1Player" + playerNum) >= 0.99 && readyToShoot)
            {
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
            PortalComp potentialCompPortal = newColl.gameObject.GetComponent<PortalComp>();
            if (potentialCompPortal)
            {
                potentialCompPortal.CheckForPlayersInside();
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
        GetComponent<TrailRenderer>().enabled = false;
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

        FindObjectOfType<AudioSource>().PlayOneShot(soundEffect);

        // Remove all the glow particles on the player
        while (ParticleCount() > 0)
        {
            LoseParticle();
        }

        dead = true;
        gameOver = true;
        PlayerMovement[] players = FindObjectsOfType<PlayerMovement>();
        if (mode != "Comp") 
        {
            for (int i = 0; i < players.Length; i++)
            {
                if (!players[i].dead)
                {
                    gameOver = false;
                }
            }
        
            if (gameOver && FindObjectOfType<GameStateData>())
            { 
                if (mode != "Tutorial")
                {
                    GameObject afterActionReport = Instantiate(afterActionReportPrefab);
                    afterActionReport.GetComponent<Canvas>().renderMode = RenderMode.WorldSpace;
                    afterActionReport.GetComponent<Canvas>().worldCamera = Camera.main;
                    if (SceneManager.GetActiveScene().name == "Level 1")
                    {
                        afterActionReport.transform.localScale *= 1.2f;
                    }
                    GameObject.Find("AfterActionWaveCount").GetComponent<TextMeshProUGUI>().SetText(GameObject.Find("WaveText").GetComponent<TextMeshProUGUI>().text);
                    GameObject.Find("AfterActionGreenKills").GetComponent<TextMeshProUGUI>().SetText("Kills: " + gameStateObject.p1Kills);
                    GameObject.Find("AfterActionBlueKills").GetComponent<TextMeshProUGUI>().SetText("Kills: " + gameStateObject.p2Kills);
                }
            }
        }
        else
        {
            gameOver = false;
        }
    }


    void Blink()
    {
        if (Time.time > nextBlink)
        {
            ren.enabled = !ren.enabled;
            nextBlink = Time.time + blinkPeriod;
        }
    }

    public void DashReady() {
        //change arrow color on sprite
        //set bool value to dashready
        dashReady = true;
        playerArrow.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 0.0f);
        // playerArrow.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 255);
    }


    public void AddCircle()
    {
        shield.SetActive(true);
        shieldMask.SetActive(true);
        health = 2;
    }

    public void RemoveCircle()
    {
        shield.SetActive(false);
        shieldMask.SetActive(false);
    }

}
