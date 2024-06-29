using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class PickupCollision : MonoBehaviour
{
    private LaneManager lane;
    PlayerMovement pMov;
    ShieldMessage shieldMessage;
    PickupManager pMan;
    ObjectPooler op;


    private void Awake()
    {
        op = ObjectPooler.instance;
    }
    // Start is called before the first frame update
    void Start()
    {
        lane = FindObjectOfType<LaneManager>();
        shieldMessage = FindObjectOfType<ShieldMessage>();
        pMov = FindObjectOfType<PlayerMovement>();
        pMan = FindObjectOfType<PickupManager>();
       
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Player")
        {


            if (gameObject.tag == "LanePlus")
            {
                pMan.SelectLaneIncrease();
            }

            if (gameObject.tag == "LaneMinus")
            {
               // EnemyMovement.OnLaneMinusPickup();
                pMan.SelectLaneDecrease();

            }

            if (gameObject.tag == "Defense")
            {
                pMov.hasShield = true;
                shieldMessage.ShowMessage();
            }

            if (gameObject.tag == "LaneFast")
            {
              pMan.SelectLaneSpeedIncrease();
            }

            if (gameObject.tag == "LaneSlow")
            {
               pMan.SelectLaneSpeedDecrease();
            }

            if (gameObject.tag == "Traffic")
            {
                pMan.SelectTraffic();
            }

            if (gameObject.tag == "Magnet")
            {
                op.DeactivateGameObject("Magnet", gameObject);
            }


            // Destroy(gameObject);
            op.DeactivateGameObject(gameObject.tag, gameObject);
        }
    }
}
