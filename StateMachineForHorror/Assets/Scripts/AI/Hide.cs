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

    //Check how far away the player is and return true if it is signifigant.
    bool PlayerDistSignifigant()
    {
        float distToBeSignifigant = 50f;

        float distToPlayer = Vector3.Distance(playerPos, owner.transform.position);

        if (distToPlayer > distToBeSignifigant)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //Checks if the player is moving toward a position where they could be ambushed.
    bool CouldAmbush(Vector3 playerPredictedPos, Vector3[] ambushSpots)
    {
        if(owner.timesPlayerPosUpdated >= owner.playerDir.Length)
        {
            float distForPotentialAmbush = 31f;

            for(int i = 0; i < ambushSpots.Length; i++)
            {
                float dist = Vector3.Distance(playerPredictedPos, ambushSpots[i]);
                
                if (dist < distForPotentialAmbush)
                {
                    return true;
                }
            }
            return false;
        }
        else
        {
            return false;
        }
    }

    public void Enter()
    {

        SM = owner.state;

        ambushSpots = new Vector3[owner.ambushSpots.Length];
        //Get all the ambush spots from the AI class.
        for (int i = 0; i < ambushSpots.Length; i++)
        {
            ambushSpots[i] = owner.ambushSpots[i].transform.position;
        }

        Debug.Log("Begining to hide");
    }
    public void Execute()
    {
        //Update the current player position
        playerPos = owner.currPlayerTransform.position;
        //Get the predicted position of the player.
        Vector3 predictedPos = owner.PredictPlayerDirection();

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
