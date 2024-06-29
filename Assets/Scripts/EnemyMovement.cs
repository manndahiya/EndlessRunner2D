using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public Transform E_transform;
    ObjectPooler op;
    PickupManager pManager;
    LaneManager Lm;
    List<GameObject> items = new List<GameObject>();


    void Start()
    {
        E_transform = GetComponent<Transform>();
        pManager = FindObjectOfType<PickupManager>();

        Lm = FindObjectOfType<LaneManager>();


        op = ObjectPooler.instance;
    }


    void Update()
    {

        MoveCars(pManager.currentSpeed);
    }



    public void MoveCars(float speed)
    {
        transform.position -= new Vector3(0, pManager.currentSpeed * Time.deltaTime, 0);
        if (transform.position.y <= -12)
        {

            items.Remove(gameObject);
            op.DeactivateGameObject(gameObject.tag, gameObject);

        }
    }




}

 
