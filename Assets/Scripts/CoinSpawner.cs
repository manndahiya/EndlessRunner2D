using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.PlayerLoop;



public class CoinSpawner : MonoBehaviour
{


    private LaneManager lm;
    ObjectPooler op;
    public GameObject coinPrefab;
    CarSpawner carSpawner;
  


    Vector2 spawnPos = Vector2.zero;
    bool canSpawnHere = false;
  

    private GameObject selectedPrefab;

    [SerializeField] private float minSpawnDistance = 2f; // Minimum allowed distance between colliders
    [SerializeField] LayerMask layersEnemyCannotSpawnOn;
    [SerializeField] float radius = 1f;

    private void Awake()
    {
        lm = FindObjectOfType<LaneManager>();
        carSpawner = GetComponent<CarSpawner>();

    }
    private void Start()
    {
        op = ObjectPooler.instance;
        StartCoroutine(SpawnCoins());

    }

    private IEnumerator SpawnCoins()
    {


        while (true)
        {

            GetRandomSpawnPosition();

            canSpawnHere = CheckForValidPosition(spawnPos);
            if (canSpawnHere)
            {
                //Instantiate(coinPrefab, spawnPos, Quaternion.identity);
                op.SpawnFromPool("Coins", spawnPos);
                carSpawner.items.AddRange(FindObjectsOfType<GameObject>().Where(go => go.tag == "Coins").ToList());
                
            }


            float time = Random.Range(0.1f, 0.2f);

            yield return new WaitForSeconds(time);
        }

    }


    private Vector2 GetRandomSpawnPosition()
    {

        int randomLaneIndex = Random.Range(0, lm.GetCurrentLaneCount());
       
        float laneWidth = lm.GetCurrentLaneWidth(randomLaneIndex);
        float middleLanePosition = lm.GetLanePosition(randomLaneIndex) + laneWidth / 2f;
        

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
