using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Scorekeeper : MonoBehaviour
{
    [SerializeField] float ScoreRate = 0.8f;

    public int score = 0;
    public TMP_Text scoreText;
    void Start()
    {
        StartCoroutine(Score());
        
    }

   
    void Update()
    {
        scoreText.text = score.ToString();
    }

    IEnumerator Score()
    {
        while (true)
        {
            yield return new WaitForSeconds(ScoreRate);
            score += 1;
            
        }
    }
}
