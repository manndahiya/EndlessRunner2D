using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupManager : MonoBehaviour
{
    public float currentSpeed;
    public float laneCurrentSpeed;
    public float defaultSpeed = 5f;
    public float laneDefaultSpeed = 1f;
    private LaneManager lane;
    PlayerMovement pMov;
    ShieldMessage shieldMessage;
    CarSpawner carSpawner;

    public bool LaneSpeedIncrease = false;
    public bool LaneSpeedReset = false;
    public bool increaseTraffic = false;

    GameObject CoinDetector;
    public bool canAttract = false;


    private void Awake()
    {
        currentSpeed = defaultSpeed;
        laneCurrentSpeed = laneDefaultSpeed;
    }

    void Start()
    {
       carSpawner = FindObjectOfType<CarSpawner>();
       shieldMessage = FindObjectOfType<ShieldMessage>();
       pMov = FindObjectOfType<PlayerMovement>();
       lane = FindObjectOfType<LaneManager>();
        if (lane == null)
        {
            Debug.LogError("LaneManager component not found!");
        }

        increaseTraffic = false;

        CoinDetector = GameObject.FindGameObjectWithTag("CoinDetector");
        CoinDetector.SetActive(false);
    }

  
    void Update()
    {
        ControlLaneSpeed();
    }

    void ControlLaneSpeed()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            LaneSpeedIncrease = true;
            LaneSpeedReset = false;
            currentSpeed += 2f;
          
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            LaneSpeedIncrease = false;
            LaneSpeedReset = true;
            currentSpeed = Mathf.Max(defaultSpeed, currentSpeed - 2f);
           
        }
    }
   

    public void SelectLaneIncrease()
    {
        if (lane != null)
        {
            if (lane.MainLaneCount < 6)
            {
                lane.LaneIncrease();

            }
            else
            {
               
                return;
            }
        }
        
    }

    public void SelectLaneDecrease()
    {
        if (lane != null)
        {
            if (lane.MainLaneCount > 2)
            {
                lane.LaneDecrease();

            }
            else
            {
               
                return;
            }
        }
        
    }

    public void SelectDefense()
    {
        pMov.hasShield = true;
        shieldMessage.ShowMessage();
    }

    public void SelectLaneSpeedIncrease()
    {
        LaneSpeedIncrease = true;
        LaneSpeedReset = false;
        currentSpeed += 2f;
        laneCurrentSpeed += 2f;
       

    }

    public void SelectLaneSpeedDecrease()
    {
        LaneSpeedReset = true;
        LaneSpeedIncrease = false;
        currentSpeed = Mathf.Max(defaultSpeed, currentSpeed - 2f);
        laneCurrentSpeed = Mathf.Max(laneDefaultSpeed, laneCurrentSpeed - 2f);
       

    }

    public void SelectTraffic()
    {
        carSpawner.radius = 2f;
        carSpawner.minSpawnDistance = 10f;
        //carSpawner.minSpawnDistance = 1f;
        carSpawner.timeTillNextSpawn = 0f;
      
    }

    public void SelectMagnet()
    {
        
        StartCoroutine(Magnetism());
    }

    public IEnumerator Magnetism()
    {
         
        CoinDetector.SetActive(true);
        yield return new WaitForSeconds(10f);
        CoinDetector.SetActive(false);
      
    }

}
