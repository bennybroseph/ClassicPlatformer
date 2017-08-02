using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleBackground2D : MonoBehaviour
{
    public float speed = 15f;

    private Vector3 previousPosition;
    // Use this for initialization
    void Start()
    {
        previousPosition = Camera.main.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.localPosition -= (Camera.main.transform.position - previousPosition) * speed;

        previousPosition = Camera.main.transform.position;

        var viewPort = Camera.main.WorldToViewportPoint(transform.position);
        if (viewPort.x > 1.7f || viewPort.x < -0.7f)
            transform.localPosition = new Vector3(0f, transform.localPosition.y);
    }
}
