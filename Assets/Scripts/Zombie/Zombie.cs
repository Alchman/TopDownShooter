﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Pathfinding;

public class Zombie : MonoBehaviour
{
    AIPath aiPath;


    public Action onHealthChanged = delegate { };


    public int health = 100;
    public int maxHealth = 100;

    [Header("AI config")]
    public float followDistance;
    public float attackDistance;
    public float searchAngle = 45;

    [Header("Attack config")]
    public float attackRate;
    public int damage;


    float nextAttack;

    enum ZombieStates
    {
        STAND,
        MOVE,
        ATTACK
    }

    ZombieStates activeState;

    Player player;

    AIPath movement;
    Animator anim;
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();

        // movement = GetComponent<ZombieMovement>();
        movement = GetComponent<AIPath>();
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();

        ChangeState(ZombieStates.STAND);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        UpdateState();

        anim.SetFloat("Speed", rb.velocity.magnitude);
    }

    void UpdateState()
    {
        if (player == null)
        {
            //return to start point
            return;
        }

        float distance = Vector2.Distance(transform.position, player.transform.position);

        switch (activeState)
        {
            case ZombieStates.STAND:
                if (distance <= followDistance)
                {
                    LayerMask layerMask = LayerMask.GetMask("Walls");
                    Vector2 direction = player.transform.position - transform.position;

                    RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, distance, layerMask);

                    if (hit.collider == null)
                    {
                        ChangeState(ZombieStates.MOVE);
                    }
                }
                //check field of view
                break;
            case ZombieStates.MOVE:
                if (distance <= attackDistance)
                {
                    ChangeState(ZombieStates.ATTACK);
                }
                Rotate();
                break;
            case ZombieStates.ATTACK:
                if (distance > attackDistance)
                {
                    anim.SetTrigger("Move");
                    ChangeState(ZombieStates.MOVE);
                }
                Rotate();

                nextAttack -= Time.fixedDeltaTime;
                if(nextAttack <= 0)
                {
                    anim.SetTrigger("Shoot");

                    nextAttack = attackRate;
                }


                break;
        }
    }

    void ChangeState(ZombieStates newState)
    {
        activeState = newState;
        switch (activeState)
        {
            case ZombieStates.STAND:
                movement.enabled = false;
                //movement.StopMovement();
                break;
            case ZombieStates.MOVE:
                movement.enabled = true;
                break;
            case ZombieStates.ATTACK:
                movement.enabled = false;
               // movement.StopMovement();
                break;
        }
    }

    public void DoDamageToPlayer()
    {
        player.DoDamage(damage);
    }

    public void DoDamage(int damage)
    {
        health -= damage;

        onHealthChanged();

        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    void Rotate()
    {
        Vector2 direction = player.transform.position - transform.position;
        transform.up = -direction;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        DamageDealer damageDealer = collision.GetComponent<DamageDealer>();
        if (damageDealer != null)
        {
            DoDamage(damageDealer.damage);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, followDistance);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackDistance);

        Gizmos.color = Color.magenta;
        Vector3 lookDirection = -transform.up;
        Gizmos.DrawRay(transform.position, lookDirection * followDistance);

        //Quaternion rotation = Quaternion.AngleAxis(searchAngle, Vector3.forward);


        Gizmos.color = Color.yellow;
        Vector3 v1 = Quaternion.AngleAxis(searchAngle, Vector3.forward) * lookDirection;
        Vector3 v2 = Quaternion.AngleAxis(-searchAngle, Vector3.forward) * lookDirection;

        Gizmos.DrawRay(transform.position, v1 * followDistance);
        Gizmos.DrawRay(transform.position, v2 * followDistance);

    }
}
