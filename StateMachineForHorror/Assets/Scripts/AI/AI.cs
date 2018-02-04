using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public interface IState
{
    void Enter();
    void Execute();
    void Exit();
}

public class StateMachine
{
    IState currentState;

    public void ChangeState(IState newState)
    {
        if(currentState != null)
        {
            currentState.Exit();
        }
        currentState = newState;
        currentState.Enter();
    }

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

    //{Delete me plz}
    public GameObject temp;

    #endregion


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
       // predictedDir = predictedDir / playerDir.Length;

        

        return predictedDir;
    }

    void UpdatePlayerDirArray(Vector3 addedDir)
    {
        

        Vector3 holdingVec = playerDir[0];
        for (int i = 1; i < playerTransformNum; i++)
        {
            //Debug.Log(playerDir[i]);
            Vector3 temp = playerDir[i];
            playerDir[i] = holdingVec;
            holdingVec = temp;
        }
        timesPlayerPosUpdated++;
        playerDir[0] = addedDir;
    }

    /*public Vector3 PredictPlayerPosition(Vector3 currPos)
    {
        Vector3 predictedPos = new Vector3(0, 0, 0);
        Vector3 weightedDiff = new Vector3(0, 0, 0);
        Vector3 difference = new Vector3(0,0,0);

        float weightedDiffMultiplyer = 2.5f;
        float predictedPosMultiplyer = 2f;

        float predictedPosY = 1f;

        weightedDiff = playerPos[0] - playerPos[1];
        weightedDiff = weightedDiff * weightedDiffMultiplyer;

        for(int i = 1; i < playerPos.Length - 1; i++)
        {
            difference += (playerPos[i + 1] - playerPos[i]);
        }
        difference = difference / playerPos.Length;

        predictedPos = currPos + weightedDiff + difference;
        predictedPos = predictedPos * predictedPosMultiplyer;

        predictedPos.y = predictedPosY;

        //Debug.Log("Predicted: " + predictedPos);
        //Instantiate(temp, predictedPos, Quaternion.identity);
        return predictedPos;
    }*/

    void UpdatePlayerPositionArray(Vector3 position)
    {
        //Move values in the array to the position beneath themselves
            //Holds a value taken from one iteration, to be taken into the next iteration.
        Vector3 holdingVec = playerPos[0];
        for(int i = 1; i < playerTransformNum; i++)
        {
            //Debug.Log(playerPos[i]);
            Vector3 temp = playerPos[i];
            playerPos[i] = holdingVec;
            holdingVec = temp;
        }
        //Sets the first player pos to the player's current position.
        playerPos[0] = position;

        timesPlayerPosUpdated++;
    }

	// Use this for initialization
	void Start ()
    {
        for(int i = 0; i < ambushSpots.Length; i++)
        {
            string finder = "AmbushSpot (" + i + ")";
            
            ambushSpots[i] = GameObject.Find(finder);
        }

        for(int i = 0; i < hidingSpots.Length; i++)
        {
            string finder = "HidingSpot (" + i + ")";
            hidingSpots[i] = GameObject.Find(finder);
            //Debug.Log(hidingSpots[i]);
        }

        
        NMA = GetComponent<NavMeshAgent>();
        state.ChangeState(new GoToHidingSpot(this));
        Debug.Log(NMA);
    }
	
	// Update is called once per frame
	void Update ()
    {
        //Track player position and add to array.
        if(updatePlayerPosTimer > 0.5f)
        {
            //UpdatePlayerPositionArray(currPlayerTransform.position);
            UpdatePlayerDirArray(currPlayerTransform.forward);
            updatePlayerPosTimer = 0;
        }
        //Predict where the player whill be based upon their movements
        /*if(timesPlayerPosUpdated >= playerPos.Length)
        {
            //PredictPlayerPosition(currPlayerTransform.position);
            ChooseAmbushSpot(PredictPlayerPosition(currPlayerTransform.position));
            timesPlayerPosUpdated = 0;
        }*/

        updatePlayerPosTimer += Time.deltaTime;

        state.Update();
	}
}
