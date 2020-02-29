using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float bounds;
    [SerializeField] GameObject explosion;
    CircleCollider2D coll;
    Rigidbody2D rb;

    GameObject shooter;
    bool escapedShooter;
    Vector2 direction;
    Vector2 lastPos;
    int damage;
    int bounceCount;

    Gradient grad;

    // Start is called before the first frame update
    void Start()
    {
        bounceCount = 0;
        coll = GetComponent<CircleCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        lastPos = Mathf.Infinity * Vector2.one;

        escapedShooter = false;
        damage = 1;

        rb.transform.rotation = Quaternion.AngleAxis(rb.rotation, Vector3.forward);
        direction = rb.transform.rotation * Vector2.up;

        grad = new Gradient();
        grad.SetKeys(new GradientColorKey[] { new GradientColorKey(GetComponent<SpriteRenderer>().color, 0.0f), explosion.GetComponent<ParticleSystem>().colorOverLifetime.color.gradient.colorKeys[1] }, explosion.GetComponent<ParticleSystem>().colorOverLifetime.color.gradient.alphaKeys);
        GetComponent<TrailRenderer>().colorGradient = grad;
    }

    // Update is called once per frame
    void Update()
    {
        // This is so we aren't wasting memory handling bullets off screen
        if (Mathf.Abs(rb.position.x) > bounds || Mathf.Abs(rb.position.x) > bounds)
        {
            Destroy(this.gameObject);
        }
    }

    private void FixedUpdate()
    {
        if ((lastPos - rb.position).magnitude / (speed * Time.fixedDeltaTime) < .9)
        {
            Poof();
            Destroy(this.gameObject);
            return;
        }
        rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime);
        lastPos = rb.position;
    }

    //Guns that shoot multiple bullets at once should not destroy eachother
    public void BulletClaim(Bullet pew) {
        Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), pew.gameObject.GetComponent<Collider2D>());
        StartCoroutine(EnableCollision(gameObject.GetComponent<Collider2D>(), pew.gameObject.GetComponent<Collider2D>()));
    }

    private IEnumerator EnableCollision(Collider2D col1, Collider2D col2) {
        
        yield return new WaitForSeconds (1.5f);
        if(col1 == null | col2 == null) {
            yield break;
        }
        Physics2D.IgnoreCollision(col1, col2, false);
        
    }

    public void Claim(GameObject player)
    {
        shooter = player;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<ShooterMovement>() != null)
        {
            // This is too avoid hitting ourselves once we fire it
            // Technically if that happens the bullet should still be a trigger,
            // But this is an extra precaution. 
            if (collision.gameObject != shooter || escapedShooter)
            {
                collision.gameObject.GetComponent<ShooterMovement>().TakeDamage(damage, shooter);
                Destroy(this.gameObject);
            }
        }
        else if (collision.gameObject.GetComponent<Bullet>() != null)
        {
            Poof();
            Destroy(collision.gameObject);
            Destroy(this.gameObject);
        }
        // We will need to add a case for hitting a zombie.
        // Though that should look similar to hitting a player.
        else
        {
            Bounce();
        }
    }

    void OnTriggerExit2D(UnityEngine.Collider2D collision)
    {
        if (collision.gameObject == shooter && !escapedShooter)
        {
            escapedShooter = true;
            coll.isTrigger = false;
        }
    }
    //This is for the case where a player stupidly stands in front of a wall and shoots
    void OnTriggerEnter2D(UnityEngine.Collider2D collision)
    {
        
        if (collision.gameObject != shooter)
        {
            string layerName = LayerMask.LayerToName(collision.gameObject.layer);
            if (!escapedShooter)
            {
                if (layerName == "Enemy")
                {
                    collision.gameObject.GetComponent<ShooterMovement>().TakeDamage(damage, shooter);
                    Destroy(this.gameObject);
                }
                // For the cruel case where you go right up against your ally and shoot
                if (layerName == "Player")
                {
                    PlayerMovement playerHit = collision.gameObject.GetComponent<PlayerMovement>();
                    if (playerHit != shooter)
                    {
                        playerHit.TakeDamage(damage, shooter);
                        Destroy(this.gameObject);
                    }
                }
                if (layerName == "Wall")
                {
                    shooter.GetComponent<ShooterMovement>().TakeDamage(damage, shooter);
                    Destroy(this.gameObject);
                }
                if (layerName == "TransparentFX")
                {
                    // Do nothing if you hit a ghost
                }

            }
            // Unity seems to update the collider into not being a trigger on a delay
            // So in this case, it means we hit a wall and should bounce as if we were not a trigger
            else if (layerName == "Wall")
            {
                Bounce();
            }
        }
    }

    // Note that bouncing is easier since bullets are spherical
    // If we made them anything else, we'd have to worry about 
    // how they are oriented when the bounce.
    void Bounce()
    {
        if (bounceCount >= 4) {
            Destroy(gameObject);
        }
        else {
            lastPos = Mathf.Infinity * Vector2.one;
            bounceCount++;
            RaycastHit2D hit = Physics2D.CircleCast(rb.position, coll.radius, direction, speed * Time.fixedDeltaTime, LayerMask.GetMask("Wall"));
            Vector2 normal = hit.normal;
            direction = direction - 2 * Vector2.Dot(direction, normal) * normal;
            rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime);
        }
        Poof();

    }

    void Poof()
    {
        var col = explosion.GetComponent<ParticleSystem>().colorOverLifetime;
        col.color = grad;
        Instantiate(explosion, transform.position, transform.rotation).SetActive(true);
    }
}

