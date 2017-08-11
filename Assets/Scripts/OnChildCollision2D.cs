using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class OnChildCollision2D : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision2D)
    {
        transform.parent.SendMessageUpwards(
            "OnCollisionEnter2D", collision2D, SendMessageOptions.DontRequireReceiver);
    }
    private void OnCollisionStay2D(Collision2D collision2D)
    {
        transform.parent.SendMessageUpwards(
            "OnCollisionStay", collision2D, SendMessageOptions.DontRequireReceiver);
    }
    private void OnCollisionExit2D(Collision2D collision2D)
    {
        transform.parent.SendMessageUpwards(
            "OnCollisionExit2D", collision2D, SendMessageOptions.DontRequireReceiver);
    }
}
