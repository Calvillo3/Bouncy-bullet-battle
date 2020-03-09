using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlowParticleScript : MonoBehaviour
{

    public GameObject owner;
    public int positionInOwnerTrail;
    float separationValue;
    float speed;

    [SerializeField] float timeAlive;
    float timeToKill;

    // Start is called before the first frame update
    void Start()
    {
        positionInOwnerTrail = 0;
        separationValue = 0.1f;
        speed = 7.0f;
        timeToKill = Time.time + timeAlive;
    }

    // Update is called once per frame
    void Update()
    {
        if (owner)
        {
            if(owner.tag == "Player")
            {
                if((transform.position - owner.transform.position).magnitude > separationValue * positionInOwnerTrail)
                {
                    transform.position += (owner.transform.position - transform.position) * Time.deltaTime * speed * (1.0f - ((float)(positionInOwnerTrail) / (1.0f + (float)(positionInOwnerTrail))));
                }
            }
        }
        else if (Time.time > timeToKill)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!owner && collision.gameObject.tag == "Player")
        {
            Debug.Log("claimed by " + collision.gameObject.name);
            collision.gameObject.GetComponent<PlayerMovement>().ClaimParticle(this);
            positionInOwnerTrail = collision.gameObject.GetComponent<PlayerMovement>().ParticleCount();
            owner = collision.gameObject;
        }
    }
}
