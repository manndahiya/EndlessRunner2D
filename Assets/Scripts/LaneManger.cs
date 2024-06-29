using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;



public class LaneManager : MonoBehaviour
{
    RoadScroll roadScroll;
    CarScroll carScroll;
    CarSpawner cs;
    PickupSpawner ps;
    CoinSpawner coins;
    ObjectPooler op;

    List<EnemyMovement> em;


    //different
    [SerializeField] public GameObject leftLanePrefab;
    [SerializeField] public GameObject rightLanePrefab;
    //universal
    public GameObject leftBoundaryPrefab;
    public GameObject rightBoundaryPrefab;
  
   public GameObject instantiatedLane1;
   public  GameObject instantiatedLane2;
   public GameObject instantiatedBoundary1;
   public GameObject instantiatedBoundary2;

   public GameObject chosenLaneLeft, chosenLaneRight, chosenBoundLeft, chosenBoundRight;

    public List<GameObject> instantiatedLanesLeft;
    public List<GameObject> instantiatedLanesRight;
    public List<GameObject> instantiatedBoundariesLeft;
    public List<GameObject> instantiatedBoundariesRight;

    public bool hasLeftLane = false;
    public bool hasRightLane = false;
    public bool isLaneMoving = false;
    public bool hasToBeRemoved = false;

    public bool LaneCountIncrement = false;
    public bool LaneCountDecrement = false;

    //Lanes pos
    public Vector3 tPos1;
    public Vector3 tPos2;
    public Vector3 cPos1;
    public Vector3 cPos2;
   
    //Boundary pos
    public Vector3 cPos3;
    public Vector3 cPos4;
    public Vector3 tPos3;
    public Vector3 tPos4;

    public int MainLaneCount = 2;
    public int PlayerLaneCount = 2;
    public float[] LaneWidths = { 0.0225f, 0.0225f, 0.036f, 0.036f, 0.04f, 0.04f };
    public List<Vector2> startingPositions = new List<Vector2>();


    [SerializeField] float laneSpeed = 5f;


    string[] tags = { "Enemy", "Enemy2", "Enemy3", "Coins", "Defense", "LaneMinus", "LanePlus", "Magnet", "LaneFast", "LaneSlow", "Traffic", "Enemy4", "Enemy5", "Enemy6", "Enemy7", "Enemy8", "Enemy9" };
    public List<GameObject> items = new List<GameObject>();


    private void Awake()
    {
      

        //initialize with 2 lanes pos
        startingPositions.Add(new Vector2(-0.7f, -3.127f));
        startingPositions.Add(new Vector2(0.5f, -3.127f));
        instantiatedLanesLeft.Add(leftLanePrefab);
        instantiatedLanesRight.Add(rightLanePrefab);
        instantiatedBoundariesLeft.Add(leftBoundaryPrefab);
        instantiatedBoundariesRight.Add(rightBoundaryPrefab);
        
       LaneCountIncrement = false;
       LaneCountDecrement = false;
}
    private void Start()
    {
        roadScroll = FindObjectOfType<RoadScroll>();
        carScroll = FindObjectOfType<CarScroll>();
        coins = FindObjectOfType<CoinSpawner>(); 
        cs = FindObjectOfType<CarSpawner>();
        ps = FindObjectOfType<PickupSpawner>();

        op = ObjectPooler.instance;
      
}
    public void Update()
    {


        if (isLaneMoving)
        {

            float step = laneSpeed * Time.deltaTime;
            SelectRecentlyAdded(out chosenLaneLeft, out chosenLaneRight, out chosenBoundLeft, out chosenBoundRight);
            MoveLaneTowards(step);
            if (ConfirmMovementCompleted())
            {

                isLaneMoving = false;


            }
        }

        if (hasToBeRemoved)
        {

            float step = laneSpeed * Time.deltaTime;
           
            MoveLaneTowards(step);


        }

        ControlLaneMove();

       

    }

    private void ControlLaneMove()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            LaneIncrease();
        }
        else if (Input.GetKeyDown(KeyCode.M))
        {
            LaneDecrease();
        }
    }

    public float GetLanePosition(int laneIndex)
    {
        if (laneIndex < 0 || laneIndex >= startingPositions.Count)
        {
          
            return float.NaN;
        }

        return startingPositions[laneIndex].x;
    }

    private void MoveLaneTowards(float step)
    {
        if (instantiatedLanesLeft != null && instantiatedLanesRight != null && instantiatedBoundariesLeft != null && instantiatedBoundariesRight != null)
        {
           

            if(chosenLaneLeft && chosenLaneRight && chosenBoundLeft && chosenBoundRight != null)
            {
                chosenLaneLeft.transform.position = Vector3.MoveTowards(chosenLaneLeft.transform.position, tPos1, step);
                chosenLaneRight.transform.position = Vector3.MoveTowards(chosenLaneRight.transform.position, tPos2, step);
                chosenBoundLeft.transform.position = Vector3.MoveTowards(chosenBoundLeft.transform.position, tPos3, step);
                chosenBoundRight.transform.position = Vector3.MoveTowards(chosenBoundRight.transform.position, tPos4, step);
            }
            else
            {
                SelectRecentlyAdded(out chosenLaneLeft, out chosenLaneRight, out chosenBoundLeft, out chosenBoundRight);
               
            }

            
        }
       
    }


    public bool ConfirmMovementCompleted()
    {
        if (instantiatedLanesLeft != null && instantiatedLanesRight != null && instantiatedBoundariesLeft != null && instantiatedBoundariesRight != null )
        {
            bool lanesReachedTarget = false;
            bool boundsReachedTarget = false;

            SelectRecentlyAdded(out chosenLaneLeft, out chosenLaneRight, out chosenBoundLeft, out chosenBoundRight);

            if(chosenLaneLeft && chosenLaneRight && chosenBoundLeft && chosenBoundRight != null ) 
            {
                lanesReachedTarget = (chosenLaneLeft.transform.position == tPos1 && chosenLaneRight.transform.position == tPos2);
                boundsReachedTarget = (chosenBoundLeft.transform.position == tPos3 && chosenBoundRight.transform.position == tPos4);
                

            }
            return (lanesReachedTarget && boundsReachedTarget);
        }

        else
        { return false; }
            
    }

    public void SelectRecentlyAdded(out GameObject chosenLaneLeft, out GameObject chosenLaneRight, out GameObject chosenBoundLeft, out GameObject chosenBoundRight)
    {
        chosenLaneLeft = instantiatedLanesLeft[instantiatedLanesLeft.Count - 1];
        chosenLaneRight = instantiatedLanesRight[instantiatedLanesRight.Count - 1];
        chosenBoundLeft = instantiatedBoundariesLeft[instantiatedBoundariesLeft.Count - 1];
        chosenBoundRight = instantiatedBoundariesRight[instantiatedBoundariesRight.Count - 1];
    }


    public int GetCurrentLaneCount()
    {
       
        return MainLaneCount;

    }

    public float GetCurrentLaneWidth(int LaneIndex)
    {
        
        if (LaneIndex >= 0 && LaneIndex < LaneWidths.Length)
        {
            return LaneWidths[LaneIndex];
        }
        else
        {
            
            return 0f;
        }
    }

    IEnumerator AllowClampingWhenMovementCompleted()
    {
        while (!ConfirmMovementCompleted())
        {  
            yield return null;
        }


        LaneCountManage();

        
        yield return null;

    }
  


    private void LaneCountManage()
    {
        if (LaneCountIncrement)
        {
            MainLaneCount += 2;
            PlayerLaneCount += 2;
            LaneCountIncrement = false;
        }

        if (LaneCountDecrement || hasToBeRemoved)
        {
            
            
            SelectRecentlyAdded(out chosenLaneLeft, out chosenLaneRight, out chosenBoundLeft, out chosenBoundRight);
            instantiatedLanesLeft.Remove(chosenLaneLeft);
            instantiatedLanesRight.Remove(chosenLaneRight);
            instantiatedBoundariesLeft.Remove(chosenBoundLeft);
            instantiatedBoundariesRight.Remove(chosenBoundRight);

            Destroy(chosenLaneLeft);
            Destroy(chosenLaneRight);
            Destroy(chosenBoundLeft);
            Destroy(chosenBoundRight);
            
           
            PlayerLaneCount -= 2;
            LaneCountDecrement = false;
            hasToBeRemoved = false;
        }
    }

    public void LaneIncrease()
    {
        
        LaneCountIncrement = true;
        LaneCountDecrement = false;
        if (MainLaneCount == 2)
        {
            cPos1 = new Vector3(-1.82f, 20.98f, 0f);
            tPos1 = new Vector3(-1.82f, 4.54f, 0f);
            

            cPos2 = new Vector3(1.75f, 20.98f, 0f);
            tPos2 = new Vector3(1.75f, 4.54f, 0f);

            cPos3 = new Vector3(-2.51f, 20.98f, 0f);
            tPos3 = new Vector3(-2.51f, 4.58f, 0f);
           

            cPos4 = new Vector3(2.55f, 20.98f, 0f);
            tPos4 = new Vector3(2.55f, 4.58f, 0f);

          
            StartCoroutine(AllowClampingWhenMovementCompleted());  
            startingPositions.Add(new Vector2(-1.82f, 4.54f));
            startingPositions.Add(new Vector2(1.75f, 4.54f));

           
        }

      

        else if (MainLaneCount == 4)
        {
            cPos1 = new Vector3(-2.82f, 21.03f, 0f);
            tPos1 = new Vector3(-2.82f, 4.54f, 0f);
            

            cPos2 = new Vector3(2.82f, 21.03f, 0f);
            tPos2 = new Vector3(2.82f, 4.54f, 0f);

            cPos3 = new Vector3(-3.504f, 21.03f, 0f);
            tPos3 = new Vector3(-3.504f, 4.54f, 0f);

            cPos4 = new Vector3(3.59f, 21.03f, 0f);
            tPos4 = new Vector3(3.59f, 4.54f, 0f);

            StartCoroutine(AllowClampingWhenMovementCompleted());
            startingPositions.Add(new Vector2(-2.82f, 4.54f));
            startingPositions.Add(new Vector2(2.82f, 4.54f));


        }


        else
        {
            Debug.Log("MAX LANES");
            return;

        }

        RandomLaneSelect();
        isLaneMoving = true;
   
    }

    public void LaneDecrease()
    {

       
        LaneCountDecrement = true;
        LaneCountIncrement = false;
        if (MainLaneCount == 4)
        {
            //get current Instantiated Lane/Boundary positions
            cPos1 = new Vector2(-1.82f, 4.54f);
            cPos2 = new Vector2(1.75f, 4.54f);
            cPos3 = new Vector2(-2.51f, 4.58f);
            cPos4 = new Vector2(2.55f, 4.58f);

            //assign target position out of screen bottom
            tPos1 = new Vector2(-1.82f, -20.76f);
            tPos2 = new Vector2(1.75f, -20.76f);
            tPos3 = new Vector2(-2.51f,- 20.76f);
            tPos4 = new Vector2(2.55f, -20.76f);




            startingPositions.Remove(new Vector2(-1.82f, 4.54f));
            startingPositions.Remove(new Vector2(1.75f, 4.54f));

        }

        else if (MainLaneCount == 6)
        {
            //get current Instantiated Lane/Boundary positions
            cPos1 = new Vector2(-2.82f, 4.54f);
            cPos2 = new Vector2(2.82f, 4.54f);
            cPos3 = new Vector2(-3.504f, 4.54f);
            cPos4 = new Vector2(3.59f, 4.54f);

            //assign target position out of screen bottom
            tPos1 = new Vector2(-2.82f, -20.76f);
            tPos2 = new Vector2(2.82f, -20.76f);
            tPos3 = new Vector2(-3.504f, -20.76f);
            tPos4 = new Vector2(3.59f, -20.76f);

            startingPositions.Remove(new Vector2(-2.82f, 4.54f));
            startingPositions.Remove(new Vector2(2.82f, 4.54f));


        }

        else if (MainLaneCount == 2)
        {
            return;
        }

        MainLaneCount -= 2;
     
        ManageLanelessItems();
        hasToBeRemoved = true;
        StartCoroutine(AllowClampingWhenMovementCompleted());

    }

    void ManageLanelessItems()
    {
       

        foreach (string tag in tags)
        {
            GameObject[] taggedItems = GameObject.FindGameObjectsWithTag(tag);
            items.AddRange(taggedItems);
        }

        if(MainLaneCount == 2)
        {
          FullyOutsideItems(1.768f, -1.802f);
        }

        else if(MainLaneCount == 4)
        {
          FullyOutsideItems(-2.8f, 2.84f);
        }
       

        //if partially
        //clamp within the lane mesh
    }

    private void FullyOutsideItems(float posX1, float posX2)
    {
       
        foreach(GameObject obj in items)
        {
            if (obj.transform.position.x == posX1 || obj.transform.position.x == posX2)
            {

                if (obj.transform.position.y > Camera.main.orthographicSize)
                {
                    op.DeactivateGameObject(obj.tag, obj);
                   
                }
            }
        }
       
    }


    private void RandomLaneSelect()
    {
        
           

            InstantiateLane(cPos1,cPos2, leftLanePrefab, rightLanePrefab );
            InstantiateBoundary(cPos3, cPos4, leftBoundaryPrefab, rightBoundaryPrefab);
            hasLeftLane = true;
            hasRightLane = true;
            

    }

    private void InstantiateLane(Vector3 startPosition1, Vector3 startPosition2, GameObject Prefab1, GameObject Prefab2)
    {
        instantiatedLane1 = Instantiate(Prefab1, startPosition1, Quaternion.identity);
        instantiatedLane1.SetActive(true);
        instantiatedLanesLeft.Add(instantiatedLane1);

       

        instantiatedLane2 = Instantiate(Prefab2, startPosition2, Quaternion.identity);
        instantiatedLane2.SetActive(true);
        instantiatedLanesRight.Add(instantiatedLane2);

        
    }

    private void InstantiateBoundary(Vector3 startPos1, Vector3 startPos2, GameObject Prefab1, GameObject Prefab2)
    {
        instantiatedBoundary1 = Instantiate(Prefab1, startPos1, Quaternion.identity);
        instantiatedBoundary1.SetActive(true);
        instantiatedBoundariesLeft.Add(instantiatedBoundary1);


        instantiatedBoundary2 = Instantiate(Prefab2 , startPos2, Quaternion.identity);
        instantiatedBoundary2.SetActive(true);  
        instantiatedBoundariesRight.Add(instantiatedBoundary2);
    }
    

    

   

}


