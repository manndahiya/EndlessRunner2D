using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ActivateMagnet : MonoBehaviour
{


    public bool canAttract = false;
    PickupManager pm;
    ObjectPooler op;


    private void Awake()
    {
        op = ObjectPooler.instance;
    }
    void Start()
    {
       
        pm = FindObjectOfType<PickupManager>();

    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
           
            pm.SelectMagnet();
           
            op.DeactivateGameObject(transform.GetChild(0).gameObject.tag , transform.GetChild(0).gameObject);
            op.DeactivateGameObject(gameObject.tag, gameObject);
            
        }
    }

  

}
