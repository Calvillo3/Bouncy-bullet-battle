using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class NextLevelPortal : MonoBehaviour
{
    float timeStarted;
    PlayerMovement playerInside;
    [SerializeField] float depositTime;
    [SerializeField] Image progress;
    [SerializeField] int particlesNeeded;
    [SerializeField] TextMeshPro display;
    CircleCollider2D coll;

    GameStateData gameStateData;
    [SerializeField] GameObject compRoundEndScreen;
    // Start is called before the first frame update
    void Start()
    {
        timeStarted = Mathf.Infinity;
        coll = GetComponent<CircleCollider2D>();
        display.text = particlesNeeded.ToString();
        gameStateData = GameObject.FindObjectOfType<GameStateData>();
    }

    // Update is called once per frame
    void Update()
    {
        float percentDone = Mathf.Clamp((Time.time - timeStarted) / depositTime, 0, 1);
        progress.fillAmount = percentDone;
        // When we have waited long enough, destroy the particle, start over, and decrement the particles needed
        if (percentDone == 1)
        {
            // Deposit the particle
            playerInside.LoseParticle();
            particlesNeeded--;
            if (particlesNeeded == 0)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
            display.text = particlesNeeded.ToString();
            // Check if the player can still deposit more
            if (playerInside.ParticleCount() > 0)
            {
                timeStarted = Time.time;
            }
            else
            {
                playerInside = null;
                timeStarted = Mathf.Infinity;
                CheckForPlayersInside(); // In case a different player inside does have particles
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // We are making a seperate function that is also referenced by the case
        // When 1 player leaves and the other player is still there with a particle
        // Or the particle is picked up by the player inside the portal
        ProcessCollider(collision);
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        PlayerMovement playerLeaving = collision.gameObject.GetComponent<PlayerMovement>();
        if (playerLeaving == playerInside)
        {
            timeStarted = Mathf.Infinity;
            playerInside = null;
            CheckForPlayersInside(); // In case a different player inside does have particles
        }
    }

    private void ProcessCollider(Collider2D collision)
    {
        // Don't restart if a player is already depositing
        if (playerInside == null)
        {
            // Check if the thing coming in is actually a player and has particles to deposit
            PlayerMovement potentialPlayer = collision.gameObject.GetComponent<PlayerMovement>();
            if (potentialPlayer)
            {
                if (potentialPlayer.ParticleCount() > 0)
                {
                    timeStarted = Time.time;
                    // If a player is inside without particles, they don't really count as inside for this
                    playerInside = potentialPlayer;
                }
            }
        }
    }

    public void CheckForPlayersInside()
    {
        // Sadly coll.raidus is off if the scale is altered. We will use the x value, but the y value should be the same since circle
        Collider2D[] potentialPlayers = Physics2D.OverlapCircleAll(transform.position, coll.radius * transform.localScale.x);
        foreach (Collider2D potentialPlayer in potentialPlayers)
        {
            ProcessCollider(potentialPlayer);
        }
    }
}
