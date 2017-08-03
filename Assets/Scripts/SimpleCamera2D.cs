using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleCamera2D : MonoBehaviour
{
    public Transform target;
    public bool clampAngles = true;
    public Vector2 minPosition;
    public Vector2 maxPosition;
    public bool followY;

    private Vector3 offsetPosition;

    // Use this for initialization
    private void Start()
    {
        offsetPosition = transform.position;
    }

    // Update is called once per frame
    private void LateUpdate()
    {
        Vector3 targetPosition =
            new Vector3(target.position.x, followY ? target.position.y : 0f, target.position.z);


        Vector3 newPosition = targetPosition + offsetPosition;

        if (clampAngles)
            newPosition =
                new Vector3(
                    Mathf.Clamp(newPosition.x, minPosition.x, maxPosition.x),
                    Mathf.Clamp(newPosition.y, minPosition.y, maxPosition.y),
                    newPosition.z);

        transform.position = newPosition;
    }
}
