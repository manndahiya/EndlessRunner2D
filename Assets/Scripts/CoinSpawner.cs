using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
   


    public GameObject coinPrefab;
    void Start()
    {
        StartCoroutine(RepeatCoins());
    }

    
    void Update()
    {
        
    }

    void CoinSpawn()
    {
        float rand = Random.Range(-1.8f, 1.8f);
        Instantiate(coinPrefab, new Vector3(rand, transform.position.y, transform.position.z), Quaternion.identity);
    }

    IEnumerator RepeatCoins()
    {
        while (true)
        {
            int time = Random.Range(1,2);
            yield return new WaitForSeconds(time);
            CoinSpawn();
        }
    }
}
