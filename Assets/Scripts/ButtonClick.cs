using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonClick : MonoBehaviour
{
   public AudioSource m_AudioSource;
   
    void Start()
    {
        
        m_AudioSource = GetComponent<AudioSource>();
    }

  public void OnClick()
    {
        m_AudioSource.Play();
    }
}
