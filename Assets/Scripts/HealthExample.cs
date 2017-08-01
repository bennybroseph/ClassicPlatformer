using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthExample : MonoBehaviour
{
    [Range(0f, 75f)]
    public float health;
    [Range(0f, 75f)]
    public float maxHealth;
}
