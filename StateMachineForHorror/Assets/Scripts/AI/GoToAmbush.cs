using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToAmbush : IState {

    StateMachine SM;
    AI owner;
    public GoToAmbush(AI owner) { this.owner = owner; }

    //Variables
    Vector3 playerPredictedPos;
    Vector3 ambushSpotToMoveTo;

    Vector3[] ambushSpots;

    float minDistToHaveArrived = 1.5f;

    //Check if the AI has reached his destination.
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

	//Picks the ambush spot closest to the players predicted position
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

        return destination;
    }

	//Setup the ambush spots array to contain all the ambush spots in the AI class.
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
