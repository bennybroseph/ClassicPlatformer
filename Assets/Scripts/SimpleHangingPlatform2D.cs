using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleHangingPlatform2D : MonoBehaviour
{
    public Rigidbody2D rigidbody2D;

    public bool waitForCollision;

    // Use this for initialization
    private void Start()
    {
        if (waitForCollision)
            rigidbody2D.isKinematic = true;
    }

    private void OnCollisionEnter2D(Collision2D collision2D)
    {
        if (!waitForCollision)
            return;

        rigidbody2D.isKinematic = false;
        waitForCollision = false;
    }
}
