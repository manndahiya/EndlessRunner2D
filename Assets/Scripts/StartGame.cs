using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{

   
  public void LoadGame()
    {
        StartCoroutine(LoadSceneAfterDelay());
        Time.timeScale = 1.0f;
    }

    IEnumerator LoadSceneAfterDelay()
    {
        // Wait for the specified delay
        yield return new WaitForSeconds(1f);

        // Load the specified scene
        SceneManager.LoadScene(1);
        
    }
   
    
}
