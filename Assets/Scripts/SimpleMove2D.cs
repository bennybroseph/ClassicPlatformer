using UnityEngine;

[RequireComponent(typeof(Collider), typeof(Rigidbody))]
public class SimpleMove2D : MonoBehaviour
{
    public Rigidbody rigidbody;
    public Collider collider;

    public float jumpForce = 350f;

    public float groundedSpeed = 15f;
    public float airborneSpeed = 10f;
    public float maxSpeed = 5f;

    public float groundCheckHeight = 0.25f;

    private bool m_IsGrounded;

    private void Update()
    {
        var rayMin =
            new Ray(
                new Vector3(
                    collider.bounds.min.x,
                    collider.bounds.min.y + 0.1f,
                    collider.bounds.min.z),
                -transform.up);
        var rayMax =
            new Ray(
                new Vector3(
                    collider.bounds.max.x,
                    collider.bounds.min.y + 0.1f,
                    collider.bounds.max.z),
                -transform.up);

        m_IsGrounded =
            Physics.Raycast(rayMin, groundCheckHeight) || Physics.Raycast(rayMax, groundCheckHeight);

        Debug.DrawLine(rayMin.origin, rayMin.origin + rayMin.direction * groundCheckHeight, Color.cyan);
        Debug.DrawLine(rayMax.origin, rayMax.origin + rayMax.direction * groundCheckHeight, Color.cyan);
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Space) && m_IsGrounded)
        {
            rigidbody.AddForce(Vector3.up * jumpForce);
            m_IsGrounded = false;
        }

        if (Input.GetKey(KeyCode.A) && rigidbody.velocity.x >= -maxSpeed)
        {
            if (m_IsGrounded)
                rigidbody.AddForce(Vector3.left * groundedSpeed);
            else
                rigidbody.AddForce(Vector3.left * airborneSpeed);
        }
        if (Input.GetKey(KeyCode.D) && rigidbody.velocity.x <= maxSpeed)
        {
            if (m_IsGrounded)
                rigidbody.AddForce(Vector3.right * groundedSpeed);
            else
                rigidbody.AddForce(Vector3.right * airborneSpeed);
        }
    }
}
