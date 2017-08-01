using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Collider2D), typeof(Rigidbody2D))]
public class AdvancedMove2D : MonoBehaviour
{
    private Rigidbody2D rigidbody;
    private Collider2D collider;
    private Animator animator;
    private SpriteRenderer renderer;

    public float jumpForce = 350f;
    public float maxJumpTime = 1.5f;
    private float jumpTime;

    public float groundedSpeed = 15f;
    public float airborneSpeed = 10f;
    public float maxSpeed = 5f;

    public float groundCheckHeight = 0.25f;

    private bool m_IsGrounded;
    private bool m_Jumping;

    private Vector2 originalCenterOfMass;
    private float originalRotationZ;

    private int groundedHash = Animator.StringToHash("Grounded");
    private int velocityHash = Animator.StringToHash("Velocity");

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
        renderer = GetComponent<SpriteRenderer>();

        originalCenterOfMass = rigidbody.centerOfMass;
        originalRotationZ = transform.localEulerAngles.z;
    }

    private void Update()
    {
        CheckGrounded();
        CheckJump();

        UpdateAnimator();
        UpdateRenderer();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.A) && rigidbody.velocity.x >= -maxSpeed)
        {
            if (m_IsGrounded)
                rigidbody.AddForce(-transform.right * groundedSpeed);
            else
                rigidbody.AddForce(-transform.right * airborneSpeed);
        }
        if (Input.GetKey(KeyCode.D) && rigidbody.velocity.x <= maxSpeed)
        {
            if (m_IsGrounded)
                rigidbody.AddForce(transform.right * groundedSpeed);
            else
                rigidbody.AddForce(transform.right * airborneSpeed);
        }
    }

    private void CheckJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && m_IsGrounded)
        {
            m_Jumping = true;
            m_IsGrounded = false;
        }
        if (Input.GetKey(KeyCode.Space) && m_Jumping)
        {
            rigidbody.velocity =
                new Vector2(rigidbody.velocity.x, jumpForce);

            jumpTime += Time.deltaTime;
        }
        if (Input.GetKeyUp(KeyCode.Space) || jumpTime > maxJumpTime && m_Jumping)
        {
            if (Input.GetKeyUp(KeyCode.Space))
                rigidbody.velocity =
                    new Vector2(rigidbody.velocity.x, rigidbody.velocity.y / 2f);

            m_Jumping = false;
            jumpTime = 0f;
        }
    }

    private void CheckGrounded()
    {
        var boxCollider = collider as BoxCollider2D;
        if (boxCollider == null)
            return;

        var rayMin =
            new Ray2D(
                transform.TransformPoint(
                    boxCollider.offset + new Vector2(0f, 0.01f) +
                        new Vector2(-boxCollider.size.x / 2f, -boxCollider.size.y / 2f)),
                -transform.up);
        var rayMax =
            new Ray2D(
                transform.TransformPoint(
                    boxCollider.offset + new Vector2(0f, 0.01f) +
                    new Vector2(boxCollider.size.x / 2f, -boxCollider.size.y / 2f)),
                -transform.up);

        m_IsGrounded =
            Physics2D.Raycast(rayMin.origin, rayMin.direction, groundCheckHeight) ||
            Physics2D.Raycast(rayMax.origin, rayMax.direction, groundCheckHeight);

        Debug.DrawLine(rayMin.origin, rayMin.origin + rayMin.direction * groundCheckHeight, Color.magenta);
        Debug.DrawLine(rayMax.origin, rayMax.origin + rayMax.direction * groundCheckHeight, Color.cyan);
    }

    private void UpdateAnimator()
    {
        if (animator == null)
            return;

        animator.SetBool(groundedHash, m_IsGrounded);
        animator.SetFloat(velocityHash, Mathf.Clamp(Mathf.Abs(rigidbody.velocity.x) / maxSpeed, 0f, 1f));
    }

    private void UpdateRenderer()
    {
        if (renderer == null)
            return;

        if (m_IsGrounded && Mathf.Abs(rigidbody.velocity.x) >= 0.1f)
            renderer.flipX = rigidbody.velocity.x < 0f;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (transform.parent == null &&
            collision.gameObject.GetComponent<Rigidbody2D>() &&
            collision.contacts.Any(contact => contact.normal.y > 0f))
        {
            var contactPoint = Vector2.zero;
            foreach (var contact in collision.contacts)
                contactPoint += contact.point;
            contactPoint /= collision.contacts.Length;

            var hits =
                Physics2D.RaycastAll(new Vector2(contactPoint.x, contactPoint.y + 0.25f), Vector2.down);

            var thisHit = hits.FirstOrDefault(hit => hit.collider == collision.collider);
            if (thisHit.normal.y <= 0.25f)
                return;

            transform.up = thisHit.normal;

            var offset =
                (Vector2)Vector3.Cross(thisHit.normal, Vector3.forward) *
                Vector2.Distance(contactPoint, transform.position) *
                (contactPoint.x > transform.position.x ? -1f : 1f);

            var position = contactPoint + offset;

            Debug.DrawLine(position, contactPoint, Color.red, 3f, false);

            transform.SetParent(collision.transform, true);

            transform.localPosition =
                new Vector3(
                    collision.transform.InverseTransformPoint(position).x,
                    collision.transform.InverseTransformPoint(position).y,
                    transform.localPosition.z);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (transform.parent != null && collision.gameObject.GetComponent<Rigidbody2D>())
        {
            transform.SetParent(null, true);

            transform.localEulerAngles =
                new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, originalRotationZ);
        }
    }
}
