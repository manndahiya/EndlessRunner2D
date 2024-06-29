using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    public TMP_Text highscoreText;
    public TMP_Text coinScoreText;

    // Start is called before the first frame update
    void Start()
    {
        float highScore = Highscore.GetHighScore();
        int coinScore = PlayerPrefs.GetInt("Coinscore", 0);
        highscoreText.text = "HighScore:\n" + highScore.ToString();
        coinScoreText.text = "CoinScore:\n" + coinScore.ToString(); 
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
