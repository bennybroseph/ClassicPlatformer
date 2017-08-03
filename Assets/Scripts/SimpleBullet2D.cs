using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class SimpleBullet2D : MonoBehaviour
{
    private Rigidbody2D m_Rigidbody2D;

    [NonSerialized]
    public GameObject parent;
    public float speed;
    public float maxAliveTime;

    public Vector3 direction;

    private float m_AliveTime;

    // Use this for initialization
    void Start()
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        m_AliveTime += Time.deltaTime;
        if (m_AliveTime >= maxAliveTime)
            Destroy(gameObject);
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        m_Rigidbody2D.velocity = direction * speed;
    }

    private void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D.gameObject != parent && !collider2D.GetComponent<SimpleBullet2D>())
            Destroy(gameObject);
    }
}
