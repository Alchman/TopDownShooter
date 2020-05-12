using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;

    Rigidbody2D rb;

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        rb.velocity = -transform.up * speed;
    }

    private void OnBecameInvisible()
    {
        Lean.Pool.LeanPool.Despawn(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Lean.Pool.LeanPool.Despawn(gameObject);
    }

}
