﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Pool;
using System;

public class Player : MonoBehaviour
{
    public Action OnDeath = delegate { };

    public int health = 100;
    public float fireRate;
    public GameObject bulletPrefab;
    public Transform shootPosition;


    float nextFire;

    Animator anim;

    public void DoDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            OnDeath();
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetButton("Fire1") && nextFire <= 0)
        {
            LeanPool.Spawn(bulletPrefab, shootPosition.position, transform.rotation);
            nextFire = fireRate;
            anim.SetTrigger("Shoot");
        }

        if (nextFire > 0)
        {
            nextFire -= Time.deltaTime;
        }
    }

}
