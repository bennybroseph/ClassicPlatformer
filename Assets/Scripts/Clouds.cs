using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class Clouds : MonoBehaviour
{
    private Image m_Image;

    public Sprite[] sprites;

    private Vector3 previousPosition;

    // Use this for initialization
    private void Start()
    {
        m_Image = GetComponent<Image>();

        m_Image.sprite = sprites[Random.Range(0, sprites.Length)];
        m_Image.SetNativeSize();
    }
}
