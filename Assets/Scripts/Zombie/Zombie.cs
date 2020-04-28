﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    [Header("AI config")]
    public float followDistance;
    public float attackDistance;

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

    ZombieMovement movement;
    Animator anim;
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();

        movement = GetComponent<ZombieMovement>();
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
                    ChangeState(ZombieStates.MOVE);
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
                movement.StopMovement();
                break;
            case ZombieStates.MOVE:
                movement.enabled = true;
                break;
            case ZombieStates.ATTACK:
                movement.enabled = false;
                movement.StopMovement();
                break;
        }
    }

    public void DoDamageToPlayer()
    {
        player.DoDamage(damage);
    }

    void Rotate()
    {
        Vector2 direction = player.transform.position - transform.position;
        transform.up = -direction;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, followDistance);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackDistance);
    }
}