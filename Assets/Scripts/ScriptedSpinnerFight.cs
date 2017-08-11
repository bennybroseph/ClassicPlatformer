using UnityEngine;

public class ScriptedSpinnerFight : MonoBehaviour
{
    [SerializeField]
    private SimpleHangingPlatform2D m_HangingPlatform2D;

    private void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D.CompareTag("Player"))
        {
            m_HangingPlatform2D.maxSpeed = 0f;
            Destroy(gameObject);
        }
    }
}
