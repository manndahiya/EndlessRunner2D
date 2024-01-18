using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] float Speed = 4f;
    public Transform E_transform;

    void Start()
    {
        E_transform =  GetComponent<Transform>();
    }

 
    void Update()
    {
        transform.position -= new Vector3(0, Speed * Time.deltaTime, 0);
        if(transform.position.y <= -12)
        {
            Destroy(gameObject);
        }
    }
}
