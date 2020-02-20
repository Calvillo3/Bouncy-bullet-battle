using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMM : MonoBehaviour
{

    [SerializeField] float speed;
    [SerializeField] float bounds;
    [SerializeField] GameObject explosion;
    Vector2 direction;
    Rigidbody2D rb;
    Gradient grad;
    CircleCollider2D coll;
    

    int bounceCount;
    // Start is called before the first frame update
    void Start()
    {
        bounceCount = 0;
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<CircleCollider2D>();
        direction = rb.transform.rotation * Vector2.up;

        grad = new Gradient();
        grad.SetKeys(new GradientColorKey[] { new GradientColorKey(GetComponent<SpriteRenderer>().color, 0.0f), explosion.GetComponent<ParticleSystem>().colorOverLifetime.color.gradient.colorKeys[1] }, explosion.GetComponent<ParticleSystem>().colorOverLifetime.color.gradient.alphaKeys);
        GetComponent<TrailRenderer>().colorGradient = grad;
        
    }

    // Update is called once per frame
    void Update()
    {
         if (Mathf.Abs(rb.position.x) > bounds || Mathf.Abs(rb.position.x) > bounds)
        {
            Destroy(this.gameObject);
        }
    }
    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        Bounce();
    }

    void Bounce()
    {
        if (bounceCount >= 4) {
            Destroy(gameObject);
        }
        else {
           
            bounceCount++;
            RaycastHit2D hit = Physics2D.CircleCast(rb.position, coll.radius, direction, speed * Time.fixedDeltaTime, LayerMask.GetMask("Wall"));
            Vector2 normal = hit.normal;
            direction = direction - 2 * Vector2.Dot(direction, normal) * normal;
            rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime);
        }
        var col = explosion.GetComponent<ParticleSystem>().colorOverLifetime;
        col.color = grad;
        Instantiate(explosion, transform.position, transform.rotation).SetActive(true);

    }
}
