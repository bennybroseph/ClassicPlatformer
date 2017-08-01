﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class SimplePlatform2D : MonoBehaviour
{
    public float speed = 1f;

    public float startDelay;
    public float delay;

    public Vector2 movement;

    private Rigidbody2D rigidbody;

    private Vector3 m_OriginalPosition;

    private bool m_Delayed;
    private bool m_Reverse;

    // Use this for initialization
    private void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();

        m_OriginalPosition = transform.localPosition;

        if (startDelay > 0f)
            Invoke("Delay", startDelay);
        else
            m_Delayed = true;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (!m_Delayed)
            return;

        var newPosition =
            Vector3.MoveTowards(
                transform.localPosition,
                m_OriginalPosition + (!m_Reverse ? (Vector3)movement : Vector3.zero),
                speed * Time.deltaTime);

        rigidbody.MovePosition(newPosition);

        if (newPosition == transform.localPosition)
        {
            if (delay > 0f)
            {
                if (!IsInvoking("ToggleReverse"))
                    Invoke("ToggleReverse", delay);
            }
            else
                m_Reverse = !m_Reverse;
        }
    }

    private void Delay()
    {
        m_Delayed = true;
    }

    private void ToggleReverse()
    {
        m_Reverse = !m_Reverse;
    }
}