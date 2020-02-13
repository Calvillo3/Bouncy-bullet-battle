using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class EnemyMovement : ShooterMovement
{
    // For moving and pathfinding
    [SerializeField] PlayerMovement[] players;
    [SerializeField] AINavMeshGenerator nav;
    [SerializeField] float moveSpeed;
    [SerializeField] float maxSteering;
    [SerializeField] float squaredApproachDistance;
    PlayerMovement target;
    Pathfinder path;
    Vector2 lastSpot;
    Vector2[] steps;
    int nextStep;
    bool foundNextStep;
    CircleCollider2D coll;
    Vector2 velocity;

    // For shooting
    [SerializeField] float reloadTime;
    [SerializeField] Text scoreBoard;
    [SerializeField] float xBound;
    [SerializeField] float yBound;
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
        target = players[0];
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            // Lord have mercy on my soul for this. Will be fixed when we make scoring better.
            scoreBoard.text = (int.Parse(scoreBoard.text) + 1).ToString();
            Destroy(this.gameObject);
        }
        // Check if the player is in range
        if (CanShoot())
        {
            // This happens every frame, but the function sets its own timer
            ShootPeriodically();
            // Even if the player is in range, lets get closer. 
            // This helps prevent enemies from blocking other enemies.
            if((target.rb.position - rb.position).sqrMagnitude > squaredApproachDistance)
            {
                UpdatePath();
                // Sometimes enemies will have crowded off a player, cutting off a path to them
                // For now we will wait, but potentially we should do something else in the future
                if (steps == null) return;
                CalcNextStep();
                MoveWithSteering();
            }
        }
        else
        {
            PickClosestTarget();
            UpdatePath();
            // Sometimes enemies will have crowded off a player, cutting off a path to them
            // For now we will wait, but potentially we should do something else in the future
            if (steps == null) return;
            CalcNextStep();
            MoveWithSteering();
        }
    }

    // If a player is not in range we might want to see if another player is closer.
    // This uses distance "as the crow flies" and the actual path could be shorter
    // But we don't want to calculate A* every frame for every enemy.
    private void PickClosestTarget()
    {
        float maxDistance = Mathf.Infinity;
        foreach (PlayerMovement player in players)
        {
            float sqrDistance = (player.rb.position - rb.position).sqrMagnitude; // sqrMagnitude is faster than magnitude
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

    // We don't want to constantly shoot so we will simulate reloading
    private void ShootPeriodically()
    {
        Vector3 targetDir = target.rb.position - rb.position;
        float angle = -90f + Mathf.Atan2(targetDir.y, targetDir.x) * Mathf.Rad2Deg;
        // Frustatingly SetRotation seems to affect the next frame, so we want to be updating our aim
        // Even if we are only shooting infrequently
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
        // If we walk to our current destination will we be able to shoot our target?
        Vector2 disp = target.rb.position - lastSpot;
        RaycastHit2D hit = Physics2D.CircleCast(lastSpot, coll.radius, disp, disp.magnitude);
        if (hit.rigidbody != target.rb)
        {
            lastSpot = target.rb.position;
            steps = path.FindPath(rb.position, lastSpot);
            nextStep = 0;
        }
    }

    // A* will return a lot of nodes but we can skip a lot of those intermediary steps
    // Find out what is the last step that we can walk towards in a straight line
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
                if (disp.magnitude <= Time.deltaTime * moveSpeed)
                {
                    lastSpot = target.rb.position;
                    steps = path.FindPath(rb.position, lastSpot);
                    nextStep = 0;
                }
                foundNextStep = true;
            }
        }
    }

    // It looks better if the enemy has trouble immediately changing direction, thus steering behaviors
    private void MoveWithSteering()
    {
        Vector2 desired_velocity = (steps[nextStep] - rb.position).normalized * moveSpeed;
        Vector2 steering = Vector2.ClampMagnitude(desired_velocity - velocity, maxSteering);
        velocity = Vector2.ClampMagnitude(velocity + steering, moveSpeed);
        rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
        // If the enemy shoots as soon as they get into position, SetRotation won't have taken affect yet
        // Thus we need to build in some time for the rigidbody to aim towards the player
        reloadedAtTime = Time.time + reloadTime;
    }
}


