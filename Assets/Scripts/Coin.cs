using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public Transform playerTransform;
    public float moveSpeed = 17f;
    public bool canAttract = false;
    
    CoinMove cm;

    void Start()
    {
        
        cm = FindObjectOfType<CoinMove>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
     
      if(collision.gameObject.tag == "CoinDetector")
        {
            canAttract = true;
            cm.enabled = true;
        }  
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "CoinDetector")
        {
            canAttract = false;
            cm.enabled = false;
        }
    }
}
