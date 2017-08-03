using UnityEngine;

public class SimpleBackground2D : MonoBehaviour
{
    public float speed = 15f;
    public Vector2 offset;

    private Vector3 previousPosition;
    // Use this for initialization
    void Start()
    {
        previousPosition = Camera.main.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        var newPosition =
            transform.localPosition - (Camera.main.transform.position - previousPosition) * speed;
        if (newPosition.y > 0f)
            newPosition.y = 0f;

        transform.localPosition = newPosition;

        previousPosition = Camera.main.transform.position;

        var viewPort = Camera.main.WorldToViewportPoint(transform.position);
        if (viewPort.x > 1.5f + offset.x || viewPort.x < -0.5f - offset.x)
            transform.localPosition = new Vector3(0f, transform.localPosition.y);
    }
}
