using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public static class Highscore 
{

   public static float GetHighScore()
    {
        return PlayerPrefs.GetFloat("Highscore", 0);
       
    }

   
    public static void SaveHighScore(float score)
    {
        float currentHighScore = GetHighScore();
        
        if (score > currentHighScore)
        {
            PlayerPrefs.SetFloat("Highscore", score);
            PlayerPrefs.Save();
        }
        
        
    }


    public static void SaveCoins(int coins)
    {
        int totalCoins = PlayerPrefs.GetInt("Coinscore" , 0) + 1;
        PlayerPrefs.SetInt("Coinscore", totalCoins);
        PlayerPrefs.Save();
        
    }
 
}
