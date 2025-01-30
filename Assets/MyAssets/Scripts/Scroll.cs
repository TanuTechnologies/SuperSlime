using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scroll : MonoBehaviour
{
    public float scrollSpeed;

    private Renderer rend;
    private Material material;
    private Vector4 offset;

    private void Start()
    {
        rend = GetComponentInChildren<Renderer>();
        material = rend.material;    
    }

    private void Update()
    {
        //// Calculate the new offset based on time and scroll speed.
        float newOffsetX = Mathf.Repeat(Time.time * scrollSpeed, 1);
        offset.y = newOffsetX;
        material.mainTextureOffset = offset;
    }
}