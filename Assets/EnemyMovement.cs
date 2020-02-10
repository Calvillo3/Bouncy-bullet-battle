using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class EnemyMovement : ShooterMovement
{
    [SerializeField] PlayerMovement[] players;
    [SerializeField] AINavMeshGenerator nav;
    [SerializeField] float moveSpeed;
    [SerializeField] float maxSteering;
    [SerializeField] float reloadTime;
    [SerializeField] Text scoreBoard; 
    [SerializeField] float xBound;
    [SerializeField] float yBound;
    [SerializeField] float squaredApproachDistance;

    PlayerMovement target;
    Pathfinder path;
    Vector2 lastSpot;
    Vector2[] steps;
    int nextStep;
    bool foundNextStep;
    SpriteRenderer ren;

    CircleCollider2D coll;
    Vector2 velocity;

    float reloadedAtTime;
    // Start is called before the first frame update
    void Start()
    {
        lastSpot = transform.position;
        path = nav.pathfinder;
        foundNextStep = false;
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<CircleCollider2D>();
        velocity = Vector2.zero;
        reloadedAtTime = 0;
        ren = GetComponent<SpriteRenderer>();

        float maxDistance = Mathf.Infinity;
        target = players[0];
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            scoreBoard.text = (int.Parse(scoreBoard.text) + 1).ToString();
            Destroy(this.gameObject);
        }
        if (CanShoot())
        {
            ShootPeriodically();
            if((target.rb.position - rb.position).sqrMagnitude > squaredApproachDistance)
            {
                UpdatePath();
                if (steps == null) return;
                CalcNextStep();
                MoveWithSteering();
            }
        }
        else
        {
            PickClosestTarget();
            UpdatePath();
            if (steps == null) return;
            CalcNextStep();
            MoveWithSteering();
        }
    }

    private void PickClosestTarget()
    {
        float maxDistance = Mathf.Infinity;
        foreach (PlayerMovement player in players)
        {
            float sqrDistance = (player.rb.position - rb.position).sqrMagnitude;
            if (sqrDistance < maxDistance)
            {
                maxDistance = sqrDistance;
                target = player;
            }
        }
    }
    private bool CanShoot()
    {
        // It seems kinda unfair to shoot off screen
        if (Mathf.Abs(transform.position.x) < xBound  && Mathf.Abs(transform.position.y) < yBound)
        {
            // Is the player in range? Can we get from the player to me?
            Vector2 disp = rb.position - target.rb.position;
            RaycastHit2D hit = Physics2D.CircleCast(target.rb.position, coll.radius / 2, disp, disp.magnitude, LayerMask.GetMask("Wall", "Enemy"));
            return (hit.collider == coll || !hit);
        }
        else return false;
    }

    private void ShootPeriodically()
    {
        Vector3 targetDir = target.rb.position - rb.position;

        float angle = -90f + Mathf.Atan2(targetDir.y, targetDir.x) * Mathf.Rad2Deg;
        rb.SetRotation(angle);

        if (Time.time > reloadedAtTime)
        {
            reloadedAtTime = Time.time + reloadTime;
            Fire();
        }
    }

    private void UpdatePath()
    {
        // Do we need to update our path?
        Vector2 disp = target.rb.position - lastSpot;
        RaycastHit2D hit = Physics2D.CircleCast(lastSpot, coll.radius, disp, disp.magnitude);
        if (hit.rigidbody != target.rb)
        {
            lastSpot = target.rb.position;
            steps = path.FindPath(rb.position, lastSpot);
            nextStep = 0;
        }
    }

    private void CalcNextStep()
    {
        foundNextStep = false;
        while (nextStep < (steps.Length - 1) && !foundNextStep)
        {
            Vector2 disp = rb.position - steps[nextStep + 1];
            RaycastHit2D hit = Physics2D.CircleCast(steps[nextStep + 1], coll.radius, disp, disp.magnitude);
            if (hit.collider == coll || !hit)
            {
                nextStep++;
            }
            else
            {
                foundNextStep = true;
            }
        }
    }

    private void MoveWithSteering()
    {
        Vector2 desired_velocity = (steps[nextStep] - rb.position).normalized * moveSpeed;
        Vector2 steering = Vector2.ClampMagnitude(desired_velocity - velocity, maxSteering);
        velocity = Vector2.ClampMagnitude(velocity + steering, moveSpeed);
        rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
        reloadedAtTime = Time.time + reloadTime;
    }
}


