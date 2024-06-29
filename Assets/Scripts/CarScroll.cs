using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarScroll : MonoBehaviour
{
    [SerializeField] public float carScrollSpeed = 10f;
    public Renderer carMeshRenderer;
    public Transform carTransform;

    EnemyMovement em;
    PickupManager pickupManager;

    private void Start()
    {
        em = FindObjectOfType<EnemyMovement>();
        carTransform = GetComponent<Transform>();
        pickupManager = FindObjectOfType<PickupManager>();
    }
    void Update()
    {
        if (em != null)
        {
            if (pickupManager != null)
            {
                if (pickupManager.LaneSpeedIncrease == true)
                {
                    carScrollSpeed = 20f;
                    em.MoveCars(20f);

                    Debug.Log("SPEED 20", this);
                }

                if ((pickupManager.LaneSpeedReset == true) || (!pickupManager.LaneSpeedReset && !pickupManager.LaneSpeedIncrease))
                {
                    carScrollSpeed = 10f;
                    em.MoveCars(10f);

                    Debug.Log("SPEED 20", this);
                }

                else
                {
                    carScrollSpeed = 10f;
                    em.MoveCars(10f);
                }
            }
            else
            {
                carScrollSpeed = 10f;
                em.MoveCars(10f);
            }
           
        }
       
    }
}
