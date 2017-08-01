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
    }
}
