using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;


public class PickupSpawner : MonoBehaviour
{
    private LaneManager laneManager;
    PickupManager pm;
   
    ObjectPooler op;
    CarSpawner carSpawner;

    public List<GameObject> pickups;
    // List of pickup tags to exclude when MainLaneCount is 2
    private List<string> excludedPickupsForTwoLanes = new List<string>();
    private Dictionary<string, float> lastSpawnTimes;


    [SerializeField] private float minSpawnDistance = 2f; // Minimum allowed distance between colliders
    [SerializeField] LayerMask layersEnemyCannotSpawnOn;
    [SerializeField] float radius = 1f;
    [SerializeField] private float cooldownTime = 10f;


    Vector2 spawnPos = Vector2.zero;
    bool canSpawnHere = false;
    int randomPrefabIndex = 0;

    private void Awake()
    {
        carSpawner = GetComponent<CarSpawner>();    
        laneManager = FindObjectOfType<LaneManager>();
        pm = FindObjectOfType<PickupManager>();
        excludedPickupsForTwoLanes.Clear();
        

    }
    void Start()
    {
        op = ObjectPooler.instance;
        if(laneManager.MainLaneCount == 2)
        {
            RemoveSpecificPickups();
        }
        lastSpawnTimes = new Dictionary<string, float>();

        foreach (var pickup in pickups)
        {
            lastSpawnTimes[pickup.tag] = -cooldownTime; // Initialize with negative cooldown to allow initial spawn
        }
        StartCoroutine(SpawnPickups());


    }


    void Update()
    {
      
    }

    void RemoveSpecificPickups()
    {
        
    }

   
   IEnumerator SpawnPickups()
    {

        while (true)
        {
            excludedPickupsForTwoLanes.Clear();
            List<GameObject> filteredPickups = GetFilteredPickups();
            


            randomPrefabIndex = Random.Range(0, filteredPickups.Count);
            string selectedTag = pickups[randomPrefabIndex].tag;

            if(Time.time - lastSpawnTimes[selectedTag] >= cooldownTime)
            {
                GetRandomSpawnPosition();


                canSpawnHere = CheckForValidPosition(spawnPos);
                if (canSpawnHere)
                {

                    op.SpawnFromPool(filteredPickups[randomPrefabIndex].tag, spawnPos);
                    carSpawner.items.AddRange(FindObjectsOfType<GameObject>().Where(go => go.tag == tag).ToList());
                    lastSpawnTimes[selectedTag] = Time.time;
                    



                }
            }
            

            float time = Random.Range(1f, 3f);
            float time2 = Random.Range(3f, 5f);
            if(laneManager.MainLaneCount == 2)
            {
                yield return new WaitForSeconds(time);
            }
            else
            {
                yield return new WaitForSeconds(time2);
            }
            
        }
                
        

    }

    private List<GameObject> GetFilteredPickups()
    {
        if (laneManager.MainLaneCount == 2)
        {
            excludedPickupsForTwoLanes.Add("LaneMinus");
            excludedPickupsForTwoLanes.Add("Traffic");
            if (pm.currentSpeed == pm.defaultSpeed || pm.laneCurrentSpeed == pm.laneDefaultSpeed)
            {
                excludedPickupsForTwoLanes.Add("LaneSlow");
              
            }

                return pickups.Where(pickup => !excludedPickupsForTwoLanes.Contains(pickup.tag)).ToList();
        }

        if (laneManager.MainLaneCount == 4)
        {
            if (pm.currentSpeed == pm.defaultSpeed || pm.laneCurrentSpeed == pm.laneDefaultSpeed)
            {
                excludedPickupsForTwoLanes.Add("LaneSlow");
               
            }
            return pickups.Where(pickup => !excludedPickupsForTwoLanes.Contains(pickup.tag)).ToList();
        }


            if (laneManager.MainLaneCount == 6)
        {
            excludedPickupsForTwoLanes.Add("LanePlus");
            if (pm.currentSpeed == pm.defaultSpeed || pm.laneCurrentSpeed == pm.laneDefaultSpeed)
            {
                excludedPickupsForTwoLanes.Add("LaneSlow");
                
            }

                return pickups.Where(pickup => !excludedPickupsForTwoLanes.Contains(pickup.tag)).ToList();
        }
    
        return pickups;
    }

    private Vector2 GetRandomSpawnPosition()
    {

        int randomLaneIndex = Random.Range(0, laneManager.GetCurrentLaneCount());
        float laneWidth = laneManager.GetCurrentLaneWidth(randomLaneIndex);
        float middleLanePosition = laneManager.GetLanePosition(randomLaneIndex) + laneWidth / 2f;
        spawnPos = new Vector2(middleLanePosition, Random.Range(10.25f, 50f) + minSpawnDistance);

        return spawnPos;
    }

    bool CheckForValidPosition(Vector2 spawnPosition)
    {
        bool isSpawnPosValid = false;
        int attemptCount = 0;
        int maxAttempts = 200;

        Collider2D[] colliders;

        while (!isSpawnPosValid && attemptCount < maxAttempts)
        {

            bool isInvalidCollision = false;

            colliders = Physics2D.OverlapCircleAll(spawnPos, radius);
            foreach (Collider2D col in colliders)
            {
                if (((1 << col.gameObject.layer) & layersEnemyCannotSpawnOn) != 0)
                {
                    // Invalid collision found
                    isInvalidCollision = true;

                    break;
                }
            }

            if (!isInvalidCollision)
            {
                isSpawnPosValid = true;
            }
            attemptCount++;
        }



        // If no invalid collisions found, spawn position is valid
        if (!isSpawnPosValid)
        {
            
            return false;
        }


        return true;

    }


}
