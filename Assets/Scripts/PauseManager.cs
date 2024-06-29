using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    
    void Start()
    {
        
    }

    
    void Update()
    {
        
    }

   public void PauseGame()
    {

        Time.timeScale = 0f;
    }

  public void ResumeGame()
    {
        Time.timeScale = 1f;
    }
}
