using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AdvancedCharacter2D))]
public class SimpleUserControl2D : MonoBehaviour
{
    private AdvancedCharacter2D m_Character2D;

    // Use this for initialization
    private void Awake()
    {
        m_Character2D = GetComponent<AdvancedCharacter2D>();
    }

    private void FixedUpdate()
    {
        var h = Input.GetAxisRaw("Horizontal");

        var shoot = Input.GetKeyDown(KeyCode.Return);

        var crouch = Input.GetKey(KeyCode.C);
        var jump = Input.GetButton("Jump");

        if (Input.GetKey(KeyCode.LeftShift))
            h *= 2f;

        m_Character2D.Move(h, shoot, crouch, jump);
    }
}
