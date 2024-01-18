using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadScroll : MonoBehaviour
{
    [SerializeField] float scrollSpeed = 0.1f;

    public Renderer meshRenderer;
  

   
    void Update()
    {
        meshRenderer.material.mainTextureOffset += 
            new Vector2(0, scrollSpeed * Time.deltaTime);
    }
}
