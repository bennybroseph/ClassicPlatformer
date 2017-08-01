using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public Rigidbody rb;

    public float speed;

    public float maxTime = 5f;
    private float currentTime;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            Vector3 newVelocity = Camera.main.transform.forward * speed;
            newVelocity.y = rb.velocity.y;

            rb.velocity = newVelocity;
        }
        if (Input.GetKey(KeyCode.A))
        {
            Vector3 newVelocity = -Camera.main.transform.right * speed;
            newVelocity.y = rb.velocity.y;

            rb.velocity = newVelocity;
        }
        if (Input.GetKey(KeyCode.S))
        {
            Vector3 newVelocity = -Camera.main.transform.forward * speed;
            newVelocity.y = rb.velocity.y;

            rb.velocity = newVelocity;
        }
        if (Input.GetKey(KeyCode.D))
        {
            Vector3 newVelocity = Camera.main.transform.right * speed;
            newVelocity.y = rb.velocity.y;

            rb.velocity = newVelocity;
        }

        if (Input.GetKey(KeyCode.Space))
        {
            //transform.eulerAngles = transform.eulerAngles + new Vector3(1f, 0f, 0f);
            rb.AddTorque(0f, 25f, 0f);
        }

        currentTime = currentTime + Time.deltaTime;
        if (currentTime >= maxTime)
        {
            //Do the thing
            rb.AddForce(0f, 2000f, 0f);
            currentTime = 0f;
        }
    }
}
