using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadScroll : MonoBehaviour
{
   
    
    

    public Renderer laneMeshRenderer;
    PickupManager pickup;


    private void Start()
    {
        laneMeshRenderer = GetComponent<Renderer>();
        pickup = FindObjectOfType<PickupManager>();
    }

    void Update()
    {

        MoveMesh(pickup.laneCurrentSpeed);
        
    }


    void MoveMesh(float speed)
    {
        laneMeshRenderer.material.mainTextureOffset += new Vector2(0, speed * Time.deltaTime);
    }
}
