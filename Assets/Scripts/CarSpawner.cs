using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Unity.VisualScripting;

using UnityEngine;

public class CarSpawner : MonoBehaviour
{
    
    private LaneManager Lm;
    PickupManager pm;
    ObjectPooler op;


    public GameObject[] carPrefab;
  
    private GameObject[] selectedPrefab;
    public List<GameObject> items;

    [SerializeField] public float minSpawnDistance = 2f; // Minimum allowed distance between colliders
    [SerializeField] LayerMask layersEnemyCannotSpawnOn;
    [SerializeField] public float radius = 1f;
    [SerializeField] public float timeTillNextSpawn = 0f;

    Vector2 spawnPos = Vector2.zero;
    bool canSpawnHere = false;
  
    int randomPrefabIndex = 0;
    int randomLaneIndex = 0;
    float laneWidth;
    float middleLanePosition;

    public float maxSpawnY = 50f;

    string[] tags = { "Enemy", "Enemy2", "Enemy3", "Enemy4", "Enemy5", "Enemy6", "Enemy7", "Enemy8", "Enemy9" };

    void Awake()
    {
        Lm = FindObjectOfType<LaneManager>();
        pm = FindObjectOfType<PickupManager>();
        
        
        
        
        if (Lm == null)
        {
            Debug.LogError("LaneManager component not found!");
        }
    }

    void Start()
    {

        op = ObjectPooler.instance;
        if (Lm != null)
        {
            StartCoroutine(SpawnCars());
        }
        else
            Debug.Log("LM NOT FOUND IN START");


    }

    public void Update()
    {
 
    }

    private IEnumerator SpawnCars()
    {
        while (true)
        {
            randomPrefabIndex = Random.Range(0, carPrefab.Length);
            GetRandomSpawnPosition();

            canSpawnHere = CheckForValidPosition(spawnPos);
            if (canSpawnHere)
            {
              
                string tag = carPrefab[randomPrefabIndex].tag;
                
                op.SpawnFromPool(tag, spawnPos);
                items.AddRange(FindObjectsOfType<GameObject>().Where(go => go.tag == tag).ToList());
      
            }

            //radius = 5f;
            //minSpawnDistance = 10f;
            timeTillNextSpawn = 0f;
            yield return new WaitForSeconds(timeTillNextSpawn);
        }

    }

    private Vector2 GetRandomSpawnPosition()
    {

        randomLaneIndex = Random.Range(0, Lm.GetCurrentLaneCount());
        
        laneWidth = Lm.GetCurrentLaneWidth(randomLaneIndex);
        middleLanePosition = Lm.GetLanePosition(randomLaneIndex) + laneWidth / 2f;
        spawnPos = new Vector2(middleLanePosition, Random.Range(10.25f, maxSpawnY) + minSpawnDistance);

       
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
                    if (tags.Contains(col.gameObject.tag))
                    {
                        // Check distance to existing car
                        float distanceToCar = Vector2.Distance(spawnPosition, col.transform.position);
                        if (distanceToCar < minSpawnDistance)
                        {
                            isInvalidCollision = true;
                            while(distanceToCar > minSpawnDistance)
                            {
                                GetRandomSpawnPosition();
                            }
                           
                        }
                    }
                    else
                    {
                      
                        isInvalidCollision = true;
                        break;

                       
                    }
         

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
