using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    PauseManager pauseManager;
    PlayerMovement pMov;
 

    public Image[] images; // Reference to the Image component in the Inspector

   
  


    void Start()
    {
        pauseManager = FindObjectOfType<PauseManager>();
        pMov = FindObjectOfType<PlayerMovement>();
    }

    
   
  

    void Update()
    {
        // Check the condition
        if (pMov.crashCount == 1)
        {
            // Hide or disable the image
            images[0].enabled = false; // To hide the image
            // OR
            // image.gameObject.SetActive(false); // To disable the image
        }
       if (pMov.crashCount == 2)
  
        {
            // Show or enable the image
            images[1].enabled = false; // To show the image
            // OR
            // image.gameObject.SetActive(true); // To enable the image
        }

       if (pMov.crashCount >= 3)
        {
            images[2].enabled = false;
        }
    }
}
