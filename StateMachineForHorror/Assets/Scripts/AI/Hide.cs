using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hide : IState {

    AI owner;
    public Hide(AI owner) { this.owner = owner; }
    StateMachine SM;

    Vector3[] ambushSpots;

    //Variables
    Vector3 playerPos;

    bool PlayerDistSignifigant()
    {
        float distToBeSignifigant = 60f;
        float dist = Vector3.Distance(playerPos, owner.transform.position);
        if (dist > distToBeSignifigant)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    bool CouldAmbush(Vector3 playerPredictedPos, Vector3[] ambushSpots)
    {

        float distForPotentialAmbush = 25f;

        foreach (Vector3 curr in ambushSpots)
        {
            float dist = Vector3.Distance(playerPredictedPos, curr);

            if (dist < distForPotentialAmbush)
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
        for (int i = 0; i < ambushSpots.Length; i++)
        {
            ambushSpots[i] = owner.ambushSpots[i].transform.position;
        }

        Debug.Log("Begining to hide");
    }
    public void Execute()
    {
        playerPos = owner.currPlayerTransform.position;

        Vector3 predictedPos = owner.PredictPlayerPosition(playerPos);
        if (CouldAmbush(predictedPos, ambushSpots))
        {
            SM.ChangeState(new GoToAmbush(owner));
        }
        else if (PlayerDistSignifigant())
        {
            SM.ChangeState(new GoToHidingSpot(owner));
        }

    }
    public void Exit()
    {
        Debug.Log("Stopping hiding");
    }
}
