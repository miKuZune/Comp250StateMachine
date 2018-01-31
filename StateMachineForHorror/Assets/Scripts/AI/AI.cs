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

public class Hide : IState
{
    AI owner;
    public Hide(AI owner) { this.owner = owner; }
    StateMachine SM;

    Vector3[] ambushSpots;

    //Variables
    Vector3 playerPos;
    
    bool PlayerDistSignifigant()
    {
        float dist = Vector3.Distance(playerPos, owner.transform.position);
        if (dist > 30f)
        {
            return true;
        }else
        {
            return false;
        }
    }

    bool CouldAmbush(Vector3 playerPredictedPos, Vector3[] ambushSpots)
    {

        foreach (Vector3 curr in ambushSpots)
        {
            float dist = Vector3.Distance(playerPredictedPos, curr);
            
            if(dist < 25)
            {
                return true;
            }
        }

        return false;
    }

    public void Enter()
    {
        SM = owner.state;
        ambushSpots = new Vector3[owner.ambushSpots.Length];
        for(int i = 0; i < ambushSpots.Length; i++)
        {
            ambushSpots[i] = owner.ambushSpots[i].transform.position;
        }

        Debug.Log("Begining to hide");
    }
    public void Execute()
    {
        playerPos = owner.currPlayerTransform.position;
        if(CouldAmbush(owner.PredictPlayerPosition(playerPos),ambushSpots) && Vector3.Distance(owner.transform.position, owner.currPlayerTransform.position) > 30.1f && Vector3.Distance(owner.transform.position, owner.PredictPlayerPosition(owner.currPlayerTransform.position)) > 30.1f)
        {
            SM.ChangeState(new GoToAmbush(owner));
        }
        else if(PlayerDistSignifigant())
        {
            SM.ChangeState(new GoToHidingSpot(owner));
        }
        Debug.Log("Hiding");
    }
    public void Exit()
    {
        Debug.Log("Stopping hiding");
    }
}

public class GoToHidingSpot : IState
{
    AI owner;
    public GoToHidingSpot(AI owner) { this.owner = owner; }
    StateMachine SM;
    // Variables
    Vector3[] hidingSpots;
    Vector3 hidingPos;
    Vector3 playerPos;

    Vector3 moveToPos1, moveToPos2;
    Vector3 currentDestination;
    float moveSpeed;

    void GetHidingSpots()
    {
        for(int i = 0; i < owner.hidingSpots.Length; i++)
        {
            hidingSpots[i] = owner.hidingSpots[i].transform.position;
        }
    }

    Vector3 ChooseHidingSpot()
    {
        Vector3 destination = new Vector3(0, 0, 0);
        float distToCurrDest = 200000;
        foreach (Vector3 curr in hidingSpots)
        {
            float dist = Vector3.Distance(playerPos, curr);
            if(dist < distToCurrDest)
            {
                destination = curr;
                distToCurrDest = dist;
            }
        }
        return destination;
    }

    bool CheckIfArrived(Vector3 destination)
    {
        float distToSpot = Vector3.Distance(owner.transform.position, destination);
        if(distToSpot < 1f)
        {
            return true;
        }else
        {
            return false;
        }
    }

    void NextState()
    {
        SM.ChangeState(new Hide(owner));
    }

    Vector3 GetMoveToPos(Vector3 comparedPos)
    {
        Vector3 posToMoveTo = comparedPos;
        posToMoveTo = Vector3.MoveTowards(posToMoveTo, owner.transform.position, 31);

        
        return posToMoveTo;
    }

    void GetNextPos()
    {
        if(currentDestination == moveToPos1)
        {
            currentDestination = moveToPos2;
            Debug.Log("To pos 2");
        }else if(currentDestination == moveToPos2)
        {
            currentDestination = hidingPos;
            Debug.Log("To hiding pos");
        }else if(currentDestination == hidingPos)
        {
            NextState();
        }
    }

    public void Enter()
    {
        SM = owner.state;
        playerPos = owner.currPlayerTransform.position;
        moveSpeed = owner.moveSpeed;
        hidingSpots = new Vector3[owner.hidingSpots.Length];
        GetHidingSpots();
        hidingPos = ChooseHidingSpot();
        moveToPos1 = GetMoveToPos(playerPos);
        moveToPos2 = GetMoveToPos(owner.PredictPlayerPosition(playerPos));
        currentDestination = moveToPos1;
        Debug.Log("Starting to go to hiding spot");
    }

    public void Execute()
    {
        owner.NMA.destination = currentDestination;
        //owner.transform.position = Vector3.MoveTowards(owner.transform.position, hidingPos, moveSpeed * Time.deltaTime);
        if (CheckIfArrived(currentDestination))
        {
            GetNextPos();
        }
        Debug.Log("Going to hiding spot");
    }
    public void Exit()
    {
        Debug.Log("Reached hiding spot");
    }
}

public class GoToAmbush : IState
{
    AI owner;
    public GoToAmbush(AI owner) { this.owner = owner; }

    //Variables
    Vector3[] ambushSpots;
    Vector3 playerPredictedPos;
    Vector3 spotToMoveTo;
    float moveSpeed;

    Vector3 moveToPos1, moveToPos2;
    Vector3 currentDestination;

    Vector3 ChooseAmbushSpot(Vector3 predictedPos)
    {
        Vector3 closestGameAmbushSpot = ambushSpots[0];
        float closestDist = Vector3.Distance(predictedPos, ambushSpots[0]);

        foreach (Vector3 curr in ambushSpots)
        {
            float dist = Vector3.Distance(predictedPos, curr);
            if (dist < closestDist)
            {
                closestDist = dist;
                closestGameAmbushSpot = curr;
            }
        }

        return closestGameAmbushSpot;
    }

    void GetNextPos()
    {
        if (currentDestination == moveToPos1)
        {
            currentDestination = moveToPos2;
            Debug.Log("To pos 2");
        }
        else if (currentDestination == moveToPos2)
        {
            currentDestination = spotToMoveTo;
            Debug.Log("To hiding pos");
        }
        else if (currentDestination == spotToMoveTo)
        {
            //Change state here
        }
    }

    bool CheckIfArrived(Vector3 destination)
    {
        float distToSpot = Vector3.Distance(owner.transform.position, destination);
        
        if (distToSpot < 1f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    Vector3 GetMoveToPos(Vector3 comparedPos)
    {
        Vector3 posToMoveTo = comparedPos;
        posToMoveTo = Vector3.MoveTowards(posToMoveTo, owner.transform.position, 31);


        return posToMoveTo;
    }

    public void Enter()
    {
        moveSpeed = owner.moveSpeed;
        playerPredictedPos = owner.PredictPlayerPosition(owner.currPlayerTransform.position);
        ambushSpots = new Vector3[owner.ambushSpots.Length];
        for(int i = 0; i < ambushSpots.Length; i ++)
        {
            ambushSpots[i] = owner.ambushSpots[i].transform.position;
        }

        spotToMoveTo = ChooseAmbushSpot(playerPredictedPos);
        moveToPos1 = GetMoveToPos(owner.currPlayerTransform.position);
        moveToPos2 = GetMoveToPos(playerPredictedPos);
        currentDestination = moveToPos1;
        Debug.Log("Starting to go to ambush spot");
    }
    public void Execute()
    {
        owner.NMA.destination = currentDestination;
        if(CheckIfArrived(currentDestination))
        {
            GetNextPos();
        }
        //owner.transform.position = Vector3.MoveTowards(owner.transform.position, spotToMoveTo, moveSpeed * Time.deltaTime);
        Debug.Log("Going to ambush spot");
    }
    public void Exit()
    {
        Debug.Log("Reached Ambush spot");
    }
}

public class Ambush : IState
{
    AI owner;

    public Ambush(AI owner) { this.owner = owner; }

    public void Enter()
    {
        Debug.Log("Beggining ambush");
    }
    public void Execute()
    {
        Debug.Log("Waiting to ambush");
    }
    public void Exit()
    {
        Debug.Log("Stopping the ambush");
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
    Quaternion[] playerRot = new Quaternion[playerTransformNum];
    float updatePlayerPosTimer;

    int timesPlayerPosUpdated = 0;


    public GameObject[] ambushSpots;
    public GameObject[] hidingSpots;

    public float moveSpeed;

    //{Delete me plz}
    public GameObject temp;

    

    #endregion



    void GoToAmbushSpot(Vector3 ambushPos)
    {
        
    }

    void SetUpAmbush()
    {

    }

    void CatchPlayer()
    {

    }

    public Vector3 PredictPlayerPosition(Vector3 currPos)
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
    }

    void UpdatePlayerPositionArray(Vector3 position)
    {
        //Move values in the array to the position beneath themselves
            //Holds a value taken from one iteration, to be taken into the next iteration.
        Vector3 holdingVec = playerPos[0];
        for(int i = 1; i < playerTransformNum; i++)
        {
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
        state.ChangeState(new GoToHidingSpot(this));
        NMA = GetComponent<NavMeshAgent>();
        Debug.Log(NMA.destination);
	}
	
	// Update is called once per frame
	void Update ()
    {
        //Track player position and add to array.
        if(updatePlayerPosTimer > 0.5f)
        {
            UpdatePlayerPositionArray(currPlayerTransform.position);
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
