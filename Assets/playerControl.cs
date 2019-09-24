using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerControl : MonoBehaviour
{
    [Range(0.0f, 10.0f)]
    public float horizontalSpeed;
    [Range(1.0f, 50.0f)]
    public float moveSpeed;
    [Range(100.0f, 1000.0f)]
    public float jumpForce;

    public int grounded = 0;

    void Start()
    {
        
    }

    void Update()
    {
        float h = horizontalSpeed * Input.GetAxis("Mouse X");
        transform.Rotate(0, h, 0);

        RaycastHit hit;

        if (Physics.Raycast(transform.position, -transform.up, out hit, Mathf.Infinity))
        {
            if (hit.distance < 1.2 && grounded == 2)
            {
                grounded = 0;
            }
        }
    }

    void FixedUpdate()
    {
        Rigidbody rb = GetComponent<Rigidbody>();

        if (Input.GetKey(KeyCode.W))
        {
            rb.AddForce(transform.forward * moveSpeed);
        }
        if (Input.GetKey(KeyCode.A))
        {
            rb.AddForce(-transform.right * moveSpeed);
        }
        if (Input.GetKey(KeyCode.S))
        {
            rb.AddForce(-transform.forward * moveSpeed);
        }
        if (Input.GetKey(KeyCode.D))
        {
            rb.AddForce(transform.right * moveSpeed);
        }
        if (Input.GetKey(KeyCode.Space) && grounded == 0)
        {
            grounded = 1;
            rb.AddForce(transform.up * jumpForce);
        }
        if (!Input.GetKey(KeyCode.Space) && grounded == 1)
        {
            grounded = 2;
        }
        float temp = rb.velocity.y;
        rb.velocity -= new Vector3(0, temp, 0);
        if (rb.velocity.magnitude > 5)
        {
            rb.velocity = rb.velocity.normalized * 5;
        }
        rb.velocity += new Vector3(0, temp, 0);
    }
}