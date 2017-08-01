using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class OnChildCollision : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision2D)
    {
        GetComponent<Collider2D>().attachedRigidbody.SendMessage("OnCollisionEnter2D", collision2D);
    }
    private void OnCollisionStay2D(Collision2D collision2D)
    {
        GetComponent<Collider2D>().attachedRigidbody.SendMessage("OnCollisionStay", collision2D);
    }
    private void OnCollisionExit2D(Collision2D collision2D)
    {
        GetComponent<Collider2D>().attachedRigidbody.SendMessage("OnCollisionExit2D", collision2D);
    }
}
