using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public interface IState
{
    //Declare variables that must be in each class that inherits from IState
    void Enter();
    void Execute();
    void Exit();
}

public class StateMachine
{
    //Defines the current state of the AI
    IState currentState;

    //Allows the AI to a new state
    public void ChangeState(IState newState)
    {
        //Check if there is a current state and perform the Exit function if there is.
        if(currentState != null)
        {
            currentState.Exit();
        }
        //Define the new state and perfrom the enter function of that state.
        currentState = newState;
        currentState.Enter();
    }

    //Performs the Execute function of the current state every tick.
    public void Update()
    {
        if(currentState != null)
        {
            currentState.Execute();
        }
    }
}

public class AI : MonoBehaviour {

    #region variables

    public StateMachine state = new StateMachine();

    public NavMeshAgent NMA;

    //Player trackers
    public Transform currPlayerTransform;

    //Holds variables for tracking and predicting player movement
    const int playerTransformNum = 25;
    Vector3[] playerPos = new Vector3[playerTransformNum];
    public Vector3[] playerDir = new Vector3[playerTransformNum];
    Quaternion[] playerRot = new Quaternion[playerTransformNum];

    float updatePlayerPosTimer;
    public int timesPlayerPosUpdated = 0;

    public GameObject[] ambushSpots;
    public GameObject[] hidingSpots;

    public Vector3 currHidingSpot = new Vector3(0,0,0);
    public Vector3 ambushSpotToGoTo = new Vector3(0, 0, 0);

    public float moveSpeed;
    #endregion

    //Predict the player's direction based upon an array of player directions
    public Vector3 PredictPlayerDirection()
    {
        const float dirWeight = 10;
        const float distanceMultiplier = 3.5f;
        Vector3 predictedDir = new Vector3(0,0,0);


        for(int i = 1; i < playerDir.Length; i++)
        {
            predictedDir += playerDir[i];
        }

        Vector3 weightedPrediction = playerDir[0] * dirWeight;

        predictedDir = predictedDir / (playerDir.Length - 1 + dirWeight);

        predictedDir += weightedPrediction;
        predictedDir *= distanceMultiplier;

        return predictedDir;
    }

    //Take the current player direction and add it to the array of player directions
    void UpdatePlayerDirArray(Vector3 addedDir)
    {
        //Holds the direction for use across iterations of the loop.
        Vector3 holdingVec = playerDir[0];
        //Goes through each value in the array and moves it one position down
        for (int i = 1; i < playerTransformNum; i++)
        {
            Vector3 temp = playerDir[i];
            playerDir[i] = holdingVec;
            holdingVec = temp;
        }
        
        //Stores the new direction in the first spot of the array.
        playerDir[0] = addedDir;

        //Counts the number of times this operation has been performed.
        timesPlayerPosUpdated++;
    }


    //Check if the player is close enough and can be seen to enter the chase state
    bool CheckForChasePlayer()
    {

        bool canChase = false;

        //Check value for how close the player has to be to enter the chase state.
        const float distToBeCloseEnough = 12.5f;
        //Calculate and store the distance to the player.
        float distToPlayer = Vector3.Distance(transform.position, currPlayerTransform.position);

        if(distToPlayer <= distToBeCloseEnough)
        {
            //Calculate the direction & distance to the player
            Vector3 dirToPlayer = currPlayerTransform.position - transform.position;
            RaycastHit hit;
            //Raycast using the direction & distance to see if anything is in the way 
            if(Physics.Raycast(transform.position,dirToPlayer, out hit))
            {
                if(hit.transform.gameObject.tag == "Player")
                {
                     canChase = true;
                }
            }
        }

        return canChase;
    }

	// Use this for initialization
	void Start ()
    {
        //Find all the ambush spots in the map
        for(int i = 0; i < ambushSpots.Length; i++)
        {
            string finder = "AmbushSpot (" + i + ")";
            
            ambushSpots[i] = GameObject.Find(finder);
        }
        //Find all the hiding spots in the map
        for(int i = 0; i < hidingSpots.Length; i++)
        {
            string finder = "HidingSpot (" + i + ")";
            hidingSpots[i] = GameObject.Find(finder);
        }

        //Define the nav mesh agent to control
        NMA = GetComponent<NavMeshAgent>();
        //Set the initial state of the AI.
        state.ChangeState(new GoToHidingSpot(this));
    }
	
	// Update is called once per frame
	void Update ()
    {
        if(CheckForChasePlayer())
        {
            state.ChangeState(new Chase(this));            
        }

        //Track player position and add to array.
        if(updatePlayerPosTimer > 0.5f)
        {
            //UpdatePlayerPositionArray(currPlayerTransform.position);
            UpdatePlayerDirArray(currPlayerTransform.forward);
            updatePlayerPosTimer = 0;
        }
        //Timer to check if it is time to update the player dir
        updatePlayerPosTimer += Time.deltaTime;

        //Perform the Update function of the current state.
        state.Update();
	}
}
