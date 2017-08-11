using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Collider2D), typeof(Rigidbody2D))]
public class AdvancedCharacter2D : MonoBehaviour
{
    private Rigidbody2D m_Rigidbody;
    private Collider2D m_Collider;
    private Animator m_Animator;
    private SpriteRenderer m_Renderer;

    [SerializeField]
    private SimpleBullet2D m_BulletPrefab;

    [SerializeField]
    private Transform m_GroundedShotPoint;
    [SerializeField]
    private Transform m_JumpingShotPoint;

    private int m_Direction;

    [SerializeField, Space]
    private float m_JumpForce = 5f;
    [SerializeField]
    private float m_MaxJumpTime = 0.25f;

    private float m_JumpTime;

    [SerializeField]
    private float m_GroundedSpeed = 8f;
    [SerializeField]
    private float m_AirborneSpeed = 4f;
    [SerializeField]
    private float m_MaxSpeed = 5f;

    public float groundCheckHeight = 0.05f;

    private bool m_IsGrounded;
    private bool m_Shooting;
    private bool m_Crouching;

    private bool m_Jumping;

    private float m_ShootTime;
    [SerializeField]
    private float m_MaxShootTime = 0.5f;
    private Coroutine m_StopShootingCoroutine;

    private readonly int m_GroundedHash = Animator.StringToHash("Grounded");
    private readonly int m_CrouchingHash = Animator.StringToHash("Crouching");
    private readonly int m_VelocityHash = Animator.StringToHash("Velocity");

    public bool isGrounded { get { return m_IsGrounded; } }

    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_Collider = GetComponent<Collider2D>();
        m_Animator = GetComponent<Animator>();
        m_Renderer = GetComponent<SpriteRenderer>();
    }

    public void Move(float h, bool shoot, bool crouch, bool jump)
    {
        if (Mathf.Abs(h) > 0f)
            m_Direction = (int)h;

        CheckGrounded();

        if (m_IsGrounded)
            HandleGroundedMovement(h, crouch, jump);
        else
            HandleAirborneMovement(h, jump);

        if (!shoot || m_Crouching)
            return;

        var newBullet =
            Instantiate(
                m_BulletPrefab,
                m_IsGrounded ? m_GroundedShotPoint.position : m_JumpingShotPoint.position,
                Quaternion.identity);

        newBullet.parent = gameObject;
        newBullet.direction = m_Direction == 1 ? transform.right : -transform.right;

        m_ShootTime = 0f;
        if (m_StopShootingCoroutine == null)
        {
            m_Shooting = true;
            m_StopShootingCoroutine = StartCoroutine(StopShooting());
        }
    }

    private void HandleGroundedMovement(float h, bool crouch, bool jump)
    {
        if (crouch && Mathf.Abs(m_Rigidbody.velocity.x) >= m_MaxSpeed / 2f)
            m_Crouching = true;
        else if (!crouch || Mathf.Abs(m_Rigidbody.velocity.x) <= 0.01f)
            m_Crouching = false;

        if (crouch)
            return;

        if (m_Rigidbody.velocity.x <= m_MaxSpeed && m_Rigidbody.velocity.x >= -m_MaxSpeed)
            m_Rigidbody.AddForce(h * transform.right * m_GroundedSpeed);

        if (!jump)
            return;

        if (!m_Jumping)
            m_Jumping = true;

        m_Rigidbody.velocity =
            new Vector2(m_Rigidbody.velocity.x, m_JumpForce);

        m_JumpTime += Time.fixedDeltaTime;
    }

    private void HandleAirborneMovement(float h, bool jump)
    {
        if (m_Rigidbody.velocity.x <= m_MaxSpeed && m_Rigidbody.velocity.x >= -m_MaxSpeed)
            m_Rigidbody.AddForce(h * transform.right * m_AirborneSpeed);

        if (jump && m_Jumping)
        {
            m_Rigidbody.velocity =
                new Vector2(m_Rigidbody.velocity.x, m_JumpForce);

            m_JumpTime += Time.fixedDeltaTime;
        }
        if ((!jump || m_JumpTime > m_MaxJumpTime) && m_Jumping)
        {
            if (!jump)
                m_Rigidbody.velocity =
                    new Vector2(m_Rigidbody.velocity.x, m_Rigidbody.velocity.y / 2f);

            m_Jumping = false;
            m_JumpTime = 0f;
        }
    }

    private void Update()
    {
        UpdateAnimator();
        UpdateRenderer();

        DrawRaycasts();
    }

    private void CheckGrounded()
    {
        m_IsGrounded = false;
        foreach (var ray2D in GetRay2Ds())
        {
            var raycastHit2D = Physics2D.Raycast(ray2D.origin, ray2D.direction, 2f * groundCheckHeight);
            if (!raycastHit2D || raycastHit2D.collider.gameObject == gameObject ||
                raycastHit2D.collider.isTrigger)
                continue;

            m_IsGrounded |= raycastHit2D;
        }
    }

    private void UpdateAnimator()
    {
        if (m_Animator == null)
            return;

        m_Animator.SetBool(m_GroundedHash, m_IsGrounded);
        m_Animator.SetLayerWeight(1, m_Shooting ? 1f : 0f);
        m_Animator.SetBool(m_CrouchingHash, m_Crouching);

        m_Animator.SetFloat(
            m_VelocityHash, Mathf.Clamp(Mathf.Abs(m_Rigidbody.velocity.x) / m_MaxSpeed, 0f, 1f));
    }

    private void UpdateRenderer()
    {
        if (m_Renderer == null)
            return;

        var flipX = m_Direction < 0;
        if (m_Renderer.flipX != flipX)
        {
            m_Renderer.flipX = flipX;
            foreach (Transform child in transform)
                child.transform.localPosition =
                    new Vector2(-child.transform.localPosition.x, child.transform.localPosition.y);
        }
    }

    private void DrawRaycasts()
    {
        foreach (var ray2D in GetRay2Ds())
            Debug.DrawLine(
                ray2D.origin, ray2D.origin + ray2D.direction * (2f * groundCheckHeight), Color.cyan);
    }

    private void OnCollisionEnter2D(Collision2D collision2D)
    {
        CheckForRigidbody(collision2D);
    }
    private void OnCollisionStay2D(Collision2D collision2D)
    {
        CheckForRigidbody(collision2D);
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (transform.parent != null && collision.gameObject.GetComponent<Rigidbody2D>())
        {
            transform.SetParent(null, true);

            transform.up = -Physics2D.gravity;
        }
    }

    private void CheckForRigidbody(Collision2D collision2D)
    {
        if (transform.parent != null || collision2D.collider.attachedRigidbody == null)
            return;

        var contactPoint = Vector2.zero;
        foreach (var contact in collision2D.contacts)
            contactPoint += contact.point;
        contactPoint /= collision2D.contacts.Length;

        var hits =
            Physics2D.RaycastAll(
                new Vector2(contactPoint.x, contactPoint.y + groundCheckHeight), Vector2.down);

        var thisHit = hits.FirstOrDefault(hit => hit.collider == collision2D.collider);
        if (thisHit.normal.y <= 0.25f)
            return;

        transform.up = thisHit.normal;

        var offset =
            (Vector2)Vector3.Cross(thisHit.normal, Vector3.forward) *
            Vector2.Distance(contactPoint, transform.position) *
            (contactPoint.x > transform.position.x ? -1f : 1f);

        var position = contactPoint + offset;

        Debug.DrawLine(position, contactPoint, Color.red, 3f, false);

        transform.SetParent(collision2D.collider.attachedRigidbody.transform, true);

        transform.localPosition =
            new Vector3(
                collision2D.transform.InverseTransformPoint(position).x,
                collision2D.transform.InverseTransformPoint(position).y,
                transform.localPosition.z);
    }

    private IEnumerator StopShooting()
    {
        while (m_ShootTime < m_MaxShootTime)
        {
            m_ShootTime += Time.deltaTime;
            yield return null;
        }

        m_Shooting = false;
        m_StopShootingCoroutine = null;
    }

    private IEnumerable<Ray2D> GetRay2Ds()
    {
        yield return new Ray2D(transform.TransformPoint(new Vector2(0f, groundCheckHeight)), -transform.up);

        var boxCollider = m_Collider as BoxCollider2D;
        if (boxCollider == null)
            yield break;

        yield return
            new Ray2D(
                transform.TransformPoint(
                    boxCollider.offset + new Vector2(0f, groundCheckHeight) +
                    new Vector2(-boxCollider.size.x / 2f, -boxCollider.size.y / 2f)),
                -transform.up);
        yield return
            new Ray2D(
                transform.TransformPoint(
                    boxCollider.offset + new Vector2(0f, groundCheckHeight) +
                    new Vector2(boxCollider.size.x / 2f, -boxCollider.size.y / 2f)),
                -transform.up);
    }
}
