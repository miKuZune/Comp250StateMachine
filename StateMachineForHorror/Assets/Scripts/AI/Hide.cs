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
        float dist = Vector3.Distance(playerPos, owner.transform.position);
        if (dist > 60f)
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

        foreach (Vector3 curr in ambushSpots)
        {
            float dist = Vector3.Distance(playerPredictedPos, curr);

            if (dist < 25)
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
        if (CouldAmbush(owner.PredictPlayerPosition(playerPos), ambushSpots) && Vector3.Distance(owner.transform.position, owner.currPlayerTransform.position) > 30.1f && Vector3.Distance(owner.transform.position, owner.PredictPlayerPosition(owner.currPlayerTransform.position)) > 30.1f)
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
