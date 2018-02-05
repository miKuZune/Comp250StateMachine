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
        float distToBeSignifigant = 50f;
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
        for (int i = 0; i < ambushSpots.Length; i++)
        {
            ambushSpots[i] = owner.ambushSpots[i].transform.position;
        }

        Debug.Log("Begining to hide");
    }
    public void Execute()
    {
        playerPos = owner.currPlayerTransform.position;

        //Vector3 predictedPos = owner.PredictPlayerPosition(playerPos);
        Vector3 predictedPos = owner.PredictPlayerDirection();
        //Debug.Log("predicted: " + predictedPos);
        if (CouldAmbush(predictedPos, ambushSpots))
        {
            SM.ChangeState(new GoToAmbush(owner));
        }
        else if (PlayerDistSignifigant())
        {
            SM.ChangeState(new GoToHidingSpot(owner));
        }

        if(Input.GetKeyDown(KeyCode.Q))
        {
            Vector3 spawnPos = playerPos + (owner.PredictPlayerDirection());
            GameObject.Instantiate(owner.temp, spawnPos, Quaternion.identity);
            Debug.Log(owner.timesPlayerPosUpdated);
        }
    }
    public void Exit()
    {
        Debug.Log("Stopping hiding");
    }
}
