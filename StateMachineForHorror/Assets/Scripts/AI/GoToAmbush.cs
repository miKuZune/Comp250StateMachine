using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToAmbush : IState {


    #region LEGACY
    /*
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
            owner.state.ChangeState(new Ambush(owner));
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
        SM = owner.state;
        moveSpeed = owner.moveSpeed;
        //playerPredictedPos = owner.PredictPlayerPosition(owner.currPlayerTransform.position);
        ambushSpots = new Vector3[owner.ambushSpots.Length];
        for (int i = 0; i < ambushSpots.Length; i++)
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
        if (CheckIfArrived(currentDestination))
        {
            GetNextPos();
        }
        //owner.transform.position = Vector3.MoveTowards(owner.transform.position, spotToMoveTo, moveSpeed * Time.deltaTime);
        Debug.Log("Going to ambush spot");
    }
    public void Exit()
    {
        Debug.Log("Reached Ambush spot");
    }*/
    #endregion


    StateMachine SM;
    AI owner;
    public GoToAmbush(AI owner) { this.owner = owner; }

    //Variables
    Vector3 playerPredictedPos;
    Vector3 ambushSpotToMoveTo;

    Vector3[] ambushSpots;

    float minDistToHaveArrived = 1.5f;

    bool CheckForArrival(Vector3 currPos, Vector3 checkPos)
    {
        float currDist = Vector3.Distance(currPos, checkPos);
        if(currDist < minDistToHaveArrived)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    Vector3 ChooseAmbushSpot()
    {
        Vector3 destination = new Vector3(0, 0, 0);

        float distToCurrChosen = 200000000f;

        for(int i = 0; i < ambushSpots.Length; i++)
        {
            float distToCurr = Vector3.Distance(playerPredictedPos, ambushSpots[i]);
            if(distToCurr < distToCurrChosen)
            {
                distToCurrChosen = distToCurr;
                destination = ambushSpots[i];
            }
        }


        Debug.Log("Player predicted pos: "+ playerPredictedPos);
        return destination;
    }

    void GetAmbushSpots()
    {
        ambushSpots = new Vector3[owner.ambushSpots.Length];
        for(int i = 0; i < ambushSpots.Length; i++)
        {
            ambushSpots[i] = owner.ambushSpots[i].transform.position;
        }
    }

    public void Enter()
    {
        playerPredictedPos = owner.PredictPlayerDirection() + owner.currPlayerTransform.position;
        GetAmbushSpots();
        ambushSpotToMoveTo = ChooseAmbushSpot();
        Debug.Log("Starting go to ambush");
        
    }

    public void Execute()
    {
        owner.NMA.destination = ambushSpotToMoveTo;
        if(CheckForArrival(owner.transform.position, ambushSpotToMoveTo))
        {
            owner.state.ChangeState(new Ambush(owner));
        }
    }

    public void Exit()
    {

    }
}
