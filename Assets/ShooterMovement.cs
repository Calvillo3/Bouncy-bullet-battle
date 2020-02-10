using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ShooterMovement : MonoBehaviour
{
    [SerializeField] Bullet bullet;
    public float health;
    public Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    protected void Fire()
    {
        // Shoots a bullet in the direction the player is facing
        Bullet pewpew = Instantiate(bullet, rb.position, rb.transform.rotation);
        // We need to tell the bullet we shot it so it doesn't hurt the player... at first
        pewpew.Claim(this.gameObject);
    }
    public void TakeDamage(int amount)
    {
        health -= amount;
    }
}


