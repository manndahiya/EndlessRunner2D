using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float driftAmount = 40f;
    [SerializeField] float rotationSpeed = 5f;
    [SerializeField] float driftRevert = 10f;


    public Transform p_transform;
    public Scorekeeper scoreValue;

    void Start()
    {
       p_transform = GetComponent<Transform>();
    }

    
    void Update()
    {
        PlayerInput();
        ClampPlayer();
    }

    private void PlayerInput()
    {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            p_transform.position += new Vector3(moveSpeed * Time.deltaTime, 0, 0);
            p_transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, -driftAmount), rotationSpeed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            p_transform.position -= new Vector3(moveSpeed * Time.deltaTime, 0, 0);
            p_transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, driftAmount), rotationSpeed * Time.deltaTime);
        }

        if(p_transform.rotation.z != 90) //If not drifting, update rotation to 0,0,0
        {
            p_transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0,0,0), driftRevert * Time.deltaTime);
        }
    }


    private void ClampPlayer()
    {
        Vector3 pos = p_transform.position;
        pos.x = Mathf.Clamp(pos.x, -2.16f, 2f);
        transform.position = pos;
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            Time.timeScale = 0f;
        }

        if(collision.gameObject.tag == "Coins")
        {
            scoreValue.score += 20;
            Debug.Log("SCORE:" + scoreValue.score);
            Destroy(collision.gameObject);
        }
    }

    
}
