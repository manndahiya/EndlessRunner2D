using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShieldMessage : MonoBehaviour
{
    [SerializeField] float timer = 5f;
    public TMP_Text text;
    public TMP_Text text2;

    void Start()
    {
        text.enabled = false;
        text2.enabled = false;
    }

   
    public void ShowMessage()
    {
        text.enabled = true;
        text2.enabled = true;
        timer = 5f;
        StartCoroutine(StartCounter(timer));
      
    }


    IEnumerator StartCounter(float timer)
    {
        while (timer > -1)
        {
            text2.SetText(Mathf.Ceil(timer).ToString());
            timer -= Time.deltaTime;
            yield return null;

        }
        text.enabled = false;
        text2.enabled = false;
    }



}
