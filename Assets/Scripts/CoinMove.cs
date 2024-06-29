using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinMove : MonoBehaviour
{
    Coin cs;
    ObjectPooler op;
 

    private void Awake()
    {
        op = ObjectPooler.instance;
    }
    void Start()
    {
        cs = gameObject.GetComponent<Coin>();
        

    }

  
    void Update()
    {
        if (cs.canAttract)
        {
            
            transform.position = Vector2.MoveTowards(transform.position, cs.playerTransform.position, cs.moveSpeed * Time.deltaTime);
        }
      


    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
           // cs.canAttract = false;
            op.DeactivateGameObject(gameObject.tag, gameObject);
            
        }
    }

   

}
