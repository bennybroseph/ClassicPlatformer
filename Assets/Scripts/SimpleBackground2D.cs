using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class SimpleBackground2D : MonoBehaviour
{
    private Canvas m_Canvas;

    public float speed = 15f;
    public bool clampY = true;
    public Vector2 offset;
    public bool forceMove;
    public Vector3 forceVelocity;

    private Vector3 previousPosition;
    private Vector3 currentLocalPosition;
    // Use this for initialization
    void Start()
    {
        m_Canvas = transform.GetComponentInParent<Canvas>();

        currentLocalPosition = transform.localPosition;

        previousPosition = Camera.main.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        currentLocalPosition -= (Camera.main.transform.position - previousPosition) * speed;
        if (forceMove)
            currentLocalPosition += forceVelocity;

        var viewPortPosition =
            m_Canvas.renderMode == RenderMode.ScreenSpaceCamera ?
                Camera.main.WorldToViewportPoint(transform.parent.TransformPoint(currentLocalPosition)) :
                Camera.main.ScreenToViewportPoint(transform.parent.TransformPoint(currentLocalPosition));

        if (viewPortPosition.x > 1.5f + offset.x || viewPortPosition.x < -0.5f - offset.x)
            currentLocalPosition.x = 0f;

        var newPosition = currentLocalPosition;
        if (clampY && viewPortPosition.y > 0.5f + offset.y)
            newPosition.y =
                transform.parent.InverseTransformPoint(
                    m_Canvas.renderMode == RenderMode.ScreenSpaceCamera ?
                        Camera.main.ViewportToWorldPoint(new Vector3(0f, 0.5f + offset.y)) :
                        Camera.main.ViewportToScreenPoint(new Vector3(0f, 0.5f + offset.y))).y;

        transform.localPosition = newPosition;

        previousPosition = Camera.main.transform.position;
    }
}
