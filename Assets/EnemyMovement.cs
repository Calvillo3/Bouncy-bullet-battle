﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using TMPro;

public class EnemyMovement : ShooterMovement
{
    // For moving and pathfinding
    [SerializeField] PlayerMovement[] players;
    [SerializeField] AINavMeshGenerator nav;
    [SerializeField] float moveSpeed;
    [SerializeField] float maxSteering;
    [SerializeField] float squaredApproachDistance;
    [SerializeField] bool tripleFire;
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
    
    [SerializeField] TextMeshProUGUI p1ScoreBoard;
    [SerializeField] TextMeshProUGUI p2ScoreBoard;
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
            if (lastShooter.name == "Player 1")
            {
                p1ScoreBoard.text = (int.Parse(p1ScoreBoard.text) + 1).ToString();
            } else if (lastShooter.name == "Player 2")
            {
                p2ScoreBoard.text = (int.Parse(p2ScoreBoard.text) + 1).ToString();
            }
            
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
            if (tripleFire)
            {
                TripleFire();
            }
            else
            {
                Fire();
            }
        }
    }

    private void UpdatePath()
    {
        // Do we need to update our path? 
        // If we walk to our current destination will we be able to shoot our target?
        Vector2 disp = target.rb.position - lastSpot;
        if (disp.magnitude > 1 || (steps == null))
        {
            RaycastHit2D hit = Physics2D.CircleCast(lastSpot, coll.radius, disp, disp.magnitude + 1);
            if (hit.rigidbody != target.rb || (steps == null))
            {
                lastSpot = target.rb.position;
                steps = path.FindPath(rb.position, lastSpot);
                nextStep = 0;
            }
        }
    }

    // A* will return a lot of nodes but we can skip a lot of those intermediary steps
    // Find out what is the last step that we can walk towards in a straight line
    private void CalcNextStep()
    {
        // If we don't have a path, just sit still
        if (steps == null) return;

        foundNextStep = false;
        while (nextStep < (steps.Length - 1) && !foundNextStep)
        {
            Vector2 disp = rb.position - steps[nextStep + 1];
            //RaycastHit2D hit = Physics2D.CircleCast(steps[nextStep + 1], coll.radius / 2, disp, disp.magnitude);
            // Technically by using a linecast instead of a raycast we might try to move into a space partially blocked
            // But most of the time the rigidbody mechanics will handle this and just push the enemy along
            // This will cause problems if we have a narrow path with a small opening, but let's not have those
            RaycastHit2D hit = Physics2D.Linecast(steps[nextStep + 1], rb.position);
            if (hit.collider == coll || !hit)
            {
                nextStep++;
            }
            else
            {
                // If the next space is extremely close yet we cannot get there, find a new path
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
        Vector2 desiredVelocity = Vector2.zero;
        // Hopefully we can go towards the player, but if not at least move away from a wall
        if (steps != null)
        {
            // If the next destination is very close, we don't desire to overshoot it, though we may with steering
            desiredVelocity = (steps[nextStep] - rb.position) / Mathf.Max(1, (steps[nextStep] - rb.position).magnitude) * moveSpeed;
        }
        
        // If we aren't moving that fast, let's use our additional speed to move away from other colliders
        if (desiredVelocity.magnitude + 1 < moveSpeed)
        {
            desiredVelocity += MovementAway(desiredVelocity);
        }

        Vector2 steering = Vector2.ClampMagnitude(desiredVelocity - velocity, maxSteering);
        velocity = Vector2.ClampMagnitude(velocity + steering, moveSpeed);


        rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
        // If the enemy shoots as soon as they get into position, SetRotation won't have taken affect yet
        // Thus we need to build in some time for the rigidbody to aim towards the player
        reloadedAtTime = Time.time + reloadTime;
    }

    // Use remaining movement to move away from other colliders
    Vector2 MovementAway(Vector2 oldVelocity)
    {
        Collider2D[] closeColls = Physics2D.OverlapCircleAll(rb.position, coll.radius * 2);
        if (closeColls.Length > 1)
        {
            Vector2 minDist = Vector2.one * Mathf.Infinity;
            foreach (Collider2D other_coll in closeColls)
            {
                if (other_coll != coll)
                {
                    Vector2 closestPoint = other_coll.ClosestPoint(rb.position);
                    Vector2 newDist = rb.position - closestPoint;
                    if (newDist.sqrMagnitude < minDist.sqrMagnitude)
                    {
                        minDist = newDist;
                    }
                }
            }
            return Vector2.ClampMagnitude(minDist.normalized * moveSpeed, moveSpeed - oldVelocity.magnitude);
        }
        else
        {
            return Vector2.zero;
        }
    }
}


