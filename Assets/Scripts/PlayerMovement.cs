using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


[RequireComponent(typeof(Rigidbody), typeof(BoxCollider2D))]
public class PlayerMovement : MonoBehaviour
{

    private LaneManager lm;
    PickupManager pm;
    ObjectPooler op;
    ShieldMessage shieldMessage;

    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float driftAmount = 40f;
    [SerializeField] float rotationSpeed = 5f;
    [SerializeField] float driftRevert = 10f;
    [SerializeField] float shieldTime = 5f;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private DynamicJoystick dj;

    public float CoinScore = 0f;



    public float crashCount = 0;

    public Transform p_transform;
    public Scorekeeper scoreValue;


    public bool hasShield = false;
    public bool hasCrashed = false;
    public bool hasMagnet = false;

    Vector2 pos;

    public GameObject coinDetector;


    public AudioSource audioSource;
    public AudioClip coinCollect;
    public AudioClip pickupCollect;
    public AudioClip carCollide;
    public AudioClip carCrash;


    void Start()
    {
        lm = FindObjectOfType<LaneManager>();
        p_transform = GetComponent<Transform>();
        op = ObjectPooler.instance;
        pm = FindObjectOfType<PickupManager>();
        shieldMessage = FindObjectOfType<ShieldMessage>();
        
    }


    void Update()
    {
        //PlayerInput();
        ClampPlayer();

       



    }

    private void FixedUpdate()
    {
        PlayerInput();
        JoystickInput();
    }

    void JoystickInput()
    {
        float horizontalInput = dj.Horizontal;

        // Move the player
        p_transform.position += new Vector3(horizontalInput * moveSpeed * Time.deltaTime, 0, 0);

        // Rotate the player
        if (horizontalInput > 0)
        {
            p_transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, -driftAmount), rotationSpeed * Time.deltaTime);
        }
        else if (horizontalInput < 0)
        {
            p_transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, driftAmount), rotationSpeed * Time.deltaTime);
        }
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

        if (p_transform.rotation.z != 90) //If not drifting, update rotation to 0,0,0
        {
            p_transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, 0), driftRevert * Time.deltaTime);
        }
    }


    private void ClampPlayer()
    {



        if (lm.PlayerLaneCount == 2)
        {
            pos = p_transform.position;
            pos.x = Mathf.Clamp(pos.x, -0.9f, 0.8f);
            pos.y = Mathf.Clamp(pos.y, -7.09f, 28.32f);
            transform.position = pos;
        }

        if (lm.PlayerLaneCount == 4)
        {


            pos = p_transform.position;
            pos.x = Mathf.Clamp(pos.x, -1.8f, 1.8f);
            pos.y = Mathf.Clamp(pos.y, -7.09f, 28.32f);
            transform.position = pos;


        }

        if (lm.PlayerLaneCount == 6)
        {

            pos = p_transform.position;
            pos.x = Mathf.Clamp(pos.x, -2.9f, 2.9f);
            pos.y = Mathf.Clamp(pos.y, -7.09f, 28.32f);
            transform.position = pos;
        }


    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Coins")
        {
            scoreValue.coinScore += 1;
            Highscore.SaveCoins(scoreValue.coinScore);
            audioSource.PlayOneShot(coinCollect);


            if (collision.gameObject != null)
            {
                op.DeactivateGameObject(collision.gameObject.tag, collision.gameObject);
            }
        }

        if (collision.gameObject.tag == "Magnet")
        {
            audioSource.PlayOneShot(pickupCollect);
        }

        if (collision.gameObject.tag.StartsWith("Enemy"))
        {
            if (!hasShield)
            {
                
                    crashCount++;
                    
                    if(crashCount == 3)
                    {
                        Highscore.SaveHighScore(scoreValue.score);
                        audioSource.PlayOneShot(carCrash);
                    StartCoroutine(LoadSceneAfterDelay());
                    Time.timeScale = 0f;

                      
                        SceneManager.LoadScene(0);
                }
                    else
                    {
                        audioSource.PlayOneShot(carCollide);
                    }
                   
                
               
            }

            else
            {
                StartCoroutine(ActivateShield());
            }
            
             op.DeactivateGameObject(collision.gameObject.tag, collision.gameObject);
            
            
        }

        if (collision.gameObject.tag == "LanePlus")
        {
            pm.SelectLaneIncrease();
            op.DeactivateGameObject("LanePlus", collision.gameObject);
            audioSource.PlayOneShot(pickupCollect);
        }

        if (collision.gameObject.tag == "LaneMinus")
        {
            
            pm.SelectLaneDecrease();
            op.DeactivateGameObject("LaneMinus", collision.gameObject);
            audioSource.PlayOneShot(pickupCollect);

        }

        if (collision.gameObject.tag == "Defense")
        {
            hasShield = true;
            shieldMessage.ShowMessage();
            op.DeactivateGameObject("Defense", collision.gameObject);
            audioSource.PlayOneShot(pickupCollect);
        }

        if (collision.gameObject.tag == "LaneFast")
        {
            pm.SelectLaneSpeedIncrease();
            op.DeactivateGameObject("LaneFast", collision.gameObject);
            audioSource.PlayOneShot(pickupCollect);
        }

        if (collision.gameObject.tag == "LaneSlow")
        {
            pm.SelectLaneSpeedDecrease();
            op.DeactivateGameObject("LaneSlow", collision.gameObject);
            audioSource.PlayOneShot(pickupCollect);
        }

        if (collision.gameObject.tag == "Traffic")
        {
            pm.SelectTraffic();
            op.DeactivateGameObject("Traffic", collision.gameObject);
            audioSource.PlayOneShot(pickupCollect);
        }


        if(crashCount == 3)
        {
            Debug.Log("Unknown Collision" + collision.gameObject.name);
        }
       

    }


    IEnumerator LoadSceneAfterDelay()
    {
        // Wait for the specified delay
        yield return new WaitForSeconds(1f);

       

    }
    IEnumerator ActivateShield()
    {
            hasShield = true;
            
            yield return new WaitForSeconds(shieldTime);
            hasShield = false;
           


    }

    
}
