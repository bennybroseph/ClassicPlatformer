using UnityEngine;

public class SimpleHangingPlatform2D : MonoBehaviour
{
    public bool waitForCollision;
    public float speed = 2f;
    public float maxSpeed = 10f;

    private Vector3 m_Velocity;
    // Use this for initialization
    private void Start()
    {

    }

    private void Update()
    {
        if (waitForCollision)
            return;

        m_Velocity += Vector3.left * speed * Time.deltaTime;
        m_Velocity = Vector3.ClampMagnitude(m_Velocity, maxSpeed);

        transform.localPosition += m_Velocity * Time.deltaTime;
    }

    private void OnCollisionEnter2D(Collision2D collision2D)
    {
        if (!waitForCollision)
            return;

        waitForCollision = false;
    }
}
