using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToHidingSpot : IState {

    AI owner;
    public GoToHidingSpot(AI owner) { this.owner = owner; }
    StateMachine SM;

    #region Variables
    // Variables
    Transform[] hidingSpots;
    Vector3 hidingPos;
    Transform playerPos;

    Vector3 moveToPos1, moveToPos2;
    Vector3 currentDestination;
    float moveSpeed;

    float arrivalDistance = 1.5f;
#endregion

    void GetHidingSpots()
    {
        //Go through each hiding spots stored in the AI class and pass to this class
        for (int i = 0; i < owner.hidingSpots.Length; i++)
        {
            hidingSpots[i] = owner.hidingSpots[i].transform;
        }
    }

    //Find a hiding spot which is closest to the AI in the direction of the player.
    Vector3 ChooseHidingSpot()
    {
        //Variables
        Vector3 destination = new Vector3(0,0,0);
        Vector3 currHidingPos = owner.currHidingSpot;
        //float hidingSpotPlayerDistTooClose = 10f;

        
        if(currHidingPos == null)
        {
            currHidingPos = new Vector3(0, 0, 0);
        }

        float distToChosenSpot = 2000000; //High number only for use in the loop to compare the lowest distance of hiding spots
        float distToPlayer = Vector3.Distance(owner.transform.position, playerPos.position);


        for (int i = 0; i < hidingSpots.Length; i++)
        {
            float distToHidingSpot = Vector3.Distance(owner.transform.position, hidingSpots[i].position);
            float distFromPlayerToHidingSpot = Vector3.Distance(hidingSpots[i].position, playerPos.position);
            
            if(hidingSpots[i].position != currHidingPos)
            {
                if(distFromPlayerToHidingSpot <= distToPlayer)
                {
                    if(distToHidingSpot < distToChosenSpot)
                    {
                        distToChosenSpot = distToHidingSpot;
                        destination = hidingSpots[i].position;
                    }
                }
            }
        }

        //If no destination is found from the first loop this indicates that there is no closest hiding spot in the direction of the player.
        //Hence the AI will find the closest spot without regarding the player.
        if(destination == new Vector3(0,0,0))
        {
            for(int i = 0; i < hidingSpots.Length; i++)
            {
                if(hidingSpots[i].position != currHidingPos)
                {
                    float distToHidingSpot = Vector3.Distance(owner.transform.position, hidingSpots[i].position);

                    if(distToHidingSpot < distToChosenSpot)
                    {
                        distToChosenSpot = distToHidingSpot;
                        destination = hidingSpots[i].position;
                    }
                }
            }
        }

        return destination;
    }

    //Check if the AI has reached where it is going.
    bool CheckIfArrived(Vector3 destination)
    {
        //Get the distance between the AI and the spot he is travelling to.
        float distToSpot = Vector3.Distance(owner.transform.position, destination);
        
        if (distToSpot < arrivalDistance)
        {
            //If the distance is less than 1 he has arrived
            return true;
        }
        else
        {
            //If not he has not arrived
            return false;
        }
    }

    void SetToHideState()
    {
        //Changes the AI's state to hiding
        SM.ChangeState(new Hide(owner));
    }

    //Get the current player position from the AI class.
    void UpdatePlayerPos()
    {
        playerPos = owner.currPlayerTransform;
    }

    public void Enter()
    {
        //Get AI's state machine
        SM = owner.state;

        //Set move speed of AI
        moveSpeed = owner.moveSpeed;

        //Get positions of player and hiding spots
        playerPos = owner.currPlayerTransform;
        hidingSpots = new Transform[owner.hidingSpots.Length];
        GetHidingSpots();

        //Choose which hiding spot to go towards
        hidingPos = ChooseHidingSpot();
        currentDestination = hidingPos;

        Debug.Log("Starting to go to hiding spot");
    }

    public void Execute()
    {
        //Update knowledge of where the player is.
        UpdatePlayerPos();
        //Set the destination
        owner.NMA.destination = currentDestination;

        if(CheckIfArrived(currentDestination))
        {
            SetToHideState();
        }
    }

    public void Exit()
    {
        owner.currHidingSpot = hidingPos;
        Debug.Log("Reached hiding spot");
    }
}
