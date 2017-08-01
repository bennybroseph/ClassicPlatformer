using UnityEngine;

/// <summary>
/// A very basic class to be used to get rotation from the mouse and follow a target.
/// It doesn't use particularly advanced code on purpose and is certainly not the best way, but rather
/// the least complex.
/// </summary>
public class SimpleCamera3D : MonoBehaviour
{
    /// <summary>
    /// The transform set here in the inspector will act as the focal point for the camera.
    /// This game object's position will be set to the target's position every frame.
    /// </summary>
    public Transform target;

    /// <summary>
    /// The maximum amount of rotation either positive or negative allowed on the x and y axis
    /// </summary>
    public Vector2 maxRotation = new Vector2(15f, 360f);
    /// <summary>
    /// A multiplier for the amount that the camera rotates based on mouse movement
    /// </summary>
    public Vector2 sensitivity = new Vector2(2f, 2f);

    /// <summary>
    /// A point of reference to use when rotating. Set to the current rotation when the game starts
    /// </summary>
    private Quaternion m_OriginalRotation;
    /// <summary>
    /// The amount of extra rotation to add to the original rotation of the object
    /// </summary>
    private Vector2 m_CurrentRotation;

    private void Start()
    {
        // Lock the cursor and make it invisible
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Grab a reference to the initial rotation
        m_OriginalRotation = transform.rotation;
    }

    private void Update()
    {
        // Add the current mouse x and y delta and add it to the current rotation after applying the
        // sensitivity multiplier
        m_CurrentRotation +=
            new Vector2(
                Input.GetAxis("Mouse Y") * sensitivity.x,
                Input.GetAxis("Mouse X") * sensitivity.y);

        ClampRotation();    // Clamp the rotational values

        // Put the original rotation and the current rotation together and set this transform's rotation
        // equal to that value
        transform.rotation =
            m_OriginalRotation * Quaternion.Euler(m_CurrentRotation);
    }

    // Adds this function to the right-click menu so it can be run outside of play mode without the use
    // of an editor script(for simplicity)
    [ContextMenu("Move To Target")]
    private void LateUpdate()
    {
        // If no target was not set in the inspector, do nothing
        if (target == null)
            return;

        // Set this transform's position to the target's position
        transform.position = target.position;
    }

    /// <summary>
    /// Clamps the angle to make sure it never goes above 360 in either positive or negative values.
    /// Also makes sure that the max rotational values are adhered to in one clean line
    /// </summary>
    private void ClampRotation()
    {
        m_CurrentRotation =
            new Vector2(
                Mathf.Clamp(m_CurrentRotation.x % 360f, -maxRotation.x, maxRotation.x),
                Mathf.Clamp(m_CurrentRotation.y % 360f, -maxRotation.y, maxRotation.y));
    }
}
