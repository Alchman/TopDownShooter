using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieMovement : MonoBehaviour
{
    public float speed;

    //other gameObjects
    Player player;

    //components on the same gameObject
    Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 direction = player.transform.position - transform.position;
        rb.velocity = direction.normalized * speed;
    }

    public void StopMovement()
    {
        rb.velocity = Vector2.zero;
    }
}
