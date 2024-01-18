using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSpawner : MonoBehaviour
{
    [SerializeField] float timeTillNextSpawn = 3f;


    public GameObject[] cars;


    void Start()
    {
        StartCoroutine(SpawnCars());
    }

   
    void Update()
    {
        
    }

    void InstantiateCars()
    {
        int rand = Random.Range(0, cars.Length);
        float randXpos = Random.Range(-2.16f, 2f);
        Instantiate(cars[rand], new Vector3(randXpos, transform.position.y, transform.position.z), Quaternion.identity);
    }
    IEnumerator SpawnCars()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeTillNextSpawn);
            InstantiateCars();
        }
    }
}
