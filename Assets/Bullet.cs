using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float bounds;

    CircleCollider2D coll;
    Rigidbody2D rb;

    GameObject shooter;
    bool escapedShooter;
    Vector2 direction;
    int damage;

    // Start is called before the first frame update
    void Start()
    {
        coll = GetComponent<CircleCollider2D>();
        rb = GetComponent<Rigidbody2D>();

        escapedShooter = false;
        damage = 1;

        rb.transform.rotation = Quaternion.AngleAxis(rb.rotation, Vector3.forward);
        direction = rb.transform.rotation * Vector2.up;
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
        rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime);
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
                collision.gameObject.GetComponent<ShooterMovement>().TakeDamage(damage);
                Destroy(this.gameObject);
            }
        }
        else if (collision.gameObject.GetComponent<Bullet>() != null)
        {
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
            if (!escapedShooter)
            {
                string layerName = LayerMask.LayerToName(collision.gameObject.layer);
                if (layerName == "Enemy" || layerName == "Player")
                {
                    collision.gameObject.GetComponent<ShooterMovement>().TakeDamage(damage);
                    Destroy(this.gameObject);
                }
                if (layerName == "Wall")
                {
                    Destroy(this.gameObject);
                }
                if (layerName == "TransparentFX")
                {
                    // Do nothing if you hit a ghost
                }
            }
            // Unity seems to update the collider into not being a trigger on a delay
            // So in this case, it means we hit a wall and should bounce as if we were not a trigger
            else
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
        RaycastHit2D hit = Physics2D.CircleCast(rb.position, coll.radius, direction, speed * Time.fixedDeltaTime, LayerMask.GetMask("Wall"));
        Vector2 normal = hit.normal;
        direction = direction - 2 * Vector2.Dot(direction, normal) * normal;
        rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime);
    }
}

