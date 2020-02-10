using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// There is some overlap between player and enemies for this class to exist
// They will handle their updates but share shooting and being shot
public class ShooterMovement : MonoBehaviour
{
    [SerializeField] Bullet bullet;
    public float health;
    public Rigidbody2D rb; // This doesn't need to be serialized, but does need to be public

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
        // Shoots a bullet in the direction the shooter is facing
        Bullet pewpew = Instantiate(bullet, rb.position, rb.transform.rotation);
        // We need to tell the bullet we shot it so it doesn't hurt the shooter... at first
        pewpew.Claim(this.gameObject);
    }

    // I'm not perfectly consistent or sure about if this should be an int or a float... 
    public void TakeDamage(int amount)
    {
        health -= amount;
    }
}


