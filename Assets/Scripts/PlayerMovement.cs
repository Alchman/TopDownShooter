using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed;

    Rigidbody2D rb;
    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Rotate();
    }

    void FixedUpdate()
    {
        Move();
    }

    void Move()
    {

        float inputX = Input.GetAxis("Horizontal");
        float inputY = Input.GetAxis("Vertical");

        rb.velocity = new Vector2(inputX, inputY) * speed;
        anim.SetFloat("Speed", rb.velocity.magnitude);


        //move with updating transform directly
        //Vector3 newPosition = transform.position;
        //newPosition.x += speed * Time.deltaTime * inputX;
        //transform.position = newPosition;
    }

    void Rotate()
    {
        //get mouse position in world coordinates
        Vector2 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector2 direction = mouseWorldPosition - (Vector2)transform.position;
        transform.up = -direction;
    }
}
