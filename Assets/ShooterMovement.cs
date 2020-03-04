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
    public GameObject lastShooter;

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
        pewpew.GetComponent<SpriteRenderer>().color = this.gameObject.GetComponent<SpriteRenderer>().color;
        
        // We need to tell the bullet we shot it so it doesn't hurt the shooter... at first
        pewpew.Claim(this.gameObject);
    }

    protected void TripleFire()
    {
        for (int i = -1; i < 2; i++)
        {
            Bullet pewpew = Instantiate(bullet, rb.position, rb.transform.rotation * Quaternion.AngleAxis(10 * i, rb.transform.forward));
            pewpew.GetComponent<SpriteRenderer>().color = this.gameObject.GetComponent<SpriteRenderer>().color;

            // We need to tell the bullet we shot it so it doesn't hurt the shooter... at first
            pewpew.Claim(this.gameObject);
        }
    }

    protected void ShotgunFire() {
        Vector3 rot1 = rb.transform.rotation.eulerAngles;
        rot1 = new Vector3(rot1.x, rot1.y, rot1.z + 15);
        Vector3 rot2 = rb.transform.rotation.eulerAngles;
        rot2 = new Vector3(rot2.x, rot2.y, rot2.z - 15);
        Bullet pewpew1 = Instantiate(bullet, rb.position, Quaternion.Euler(rot1));
        Bullet pewpew2 = Instantiate(bullet, rb.position, rb.transform.rotation);
        Bullet pewpew3 = Instantiate(bullet, rb.position, Quaternion.Euler(rot2));
        pewpew1.GetComponent<SpriteRenderer>().color = this.gameObject.GetComponent<SpriteRenderer>().color;
        pewpew2.GetComponent<SpriteRenderer>().color = this.gameObject.GetComponent<SpriteRenderer>().color;
        pewpew3.GetComponent<SpriteRenderer>().color = this.gameObject.GetComponent<SpriteRenderer>().color; 
        pewpew1.Claim(this.gameObject);
        pewpew2.Claim(this.gameObject);
        pewpew3.Claim(this.gameObject);
        pewpew1.BulletClaim(pewpew2);
        pewpew1.BulletClaim(pewpew3);
        pewpew2.BulletClaim(pewpew3);
    }

    // I'm not perfectly consistent or sure about if this should be an int or a float... 
    public void TakeDamage(int amount, GameObject lastShooter, string shooterTag)
    {
        // No friendly fire in competitive
        if (GetComponent<PlayerMovement>() && tag == shooterTag)
        {
            // We have to check this last pars to avoid errors
            if (lastShooter != gameObject && GetComponent<PlayerMovement>().comp)
            {
                return;
            }
        }
        else {
        health -= amount;
        this.lastShooter = lastShooter;
        }
    }
}


