using UnityEngine;

/// <summary>
/// A very basic script made to demonstrate an easy way to have physics abiding movement.
/// Not necessarily the best way, just the least complex for learning purposes.
/// </summary>
public class SimpleMove3D : MonoBehaviour
{
    /// <summary>
    /// The speed modifier for player movement
    /// </summary>
    public float speed;

    /// <summary>
    /// A reference to the attached rigid body
    /// </summary>
    private Rigidbody m_Rigidbody;

    private void Start()
    {
        // Grab a reference to the rigid body attached to this game object
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        // Grab the forward of the camera, but don't use its y value
        Vector3 forward = Camera.main.transform.forward;
        forward.y = 0f;

        // Grab the right of the camera, but don't use its y value
        Vector3 right = Camera.main.transform.right;
        right.y = 0f;

        // Calculate the new velocity based on the sum horizontal and vertical key presses scaled by
        // the camera's right and forward values
        Vector3 newVelocity =
            Input.GetAxisRaw("Horizontal") * right.normalized +
            Input.GetAxisRaw("Vertical") * forward.normalized;

        // Scale velocity by speed
        newVelocity *= speed;

        // Set the velocity of the rigid body, but don't modify the y velocity
        m_Rigidbody.velocity = new Vector3(newVelocity.x, m_Rigidbody.velocity.y, newVelocity.z);
    }
}
