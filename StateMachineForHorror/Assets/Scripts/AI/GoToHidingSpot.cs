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

    Vector3 ChooseHidingSpot()
    {
        //Variables
        Vector3 destination = new Vector3(0,0,0);
        Vector3 currHidingPos = owner.currHidingSpot;
        float hidingSpotPlayerDistTooClose = 10f;

        
        if(currHidingPos == null)
        {
            currHidingPos = new Vector3(0, 0, 0);
        }

        float distToChosenSpot = 2000000; //High number only for use in the loop to compare the lowest distance of hiding spots
        float distToPlayer = Vector3.Distance(owner.transform.position, playerPos.position);

        //float AIplayerAngle = Vector3.Angle(owner.transform.forward, playerPos.position);


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

        return destination;
    }

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


    //LEGACY
    /*Vector3 GetMoveToPos(Vector3 comparedPos)
    {
        Vector3 posToMoveTo = comparedPos;
        posToMoveTo = Vector3.MoveTowards(posToMoveTo, owner.transform.position, hidingDistFromPlayer);


        return posToMoveTo;
    }*/


    //LEGACY
    /*void GetNextPos()
    {
        if (currentDestination == moveToPos1)
        {
            currentDestination = moveToPos2;
            Debug.Log("To pos 2");
        }
        else if (currentDestination == moveToPos2)
        {
            currentDestination = hidingPos;
            Debug.Log("To hiding pos");
        }
        else if (currentDestination == hidingPos)
        {
            SetToHideState();
        }
    }*/

    bool CheckIfNearPlayer()
    {
        //Const variables
        const float minDistFromPlayer = 35f;

        float distFromPlayer = Vector3.Distance(owner.transform.position, playerPos.position);
        if(distFromPlayer <= minDistFromPlayer)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /*Vector3 MoveAwayFromPlayer()
    {
        //Direction = (posA - posB) / distance[between posA and posB]
        Debug.Log("GET AWAY!");

        const float moveAwayAmount = 15f;

        Vector3 AiPlayerDir = playerPos - owner.transform.position;
        float AiPlayerDist = Vector3.Distance(playerPos, owner.transform.position);
        AiPlayerDir = AiPlayerDir / AiPlayerDist;

        Vector3 AiHidingSpotDir = hidingPos;
        float AiHideSpotDist = Vector3.Distance(hidingPos, owner.transform.position);
        AiHidingSpotDir = AiHidingSpotDir / AiHideSpotDist;
        float AiHidingSpotAngle = Vector3.Angle(hidingPos, owner.transform.position);

        Vector3 newDir;

        if(AiHidingSpotAngle >= 0)
        {
            newDir = AiPlayerDir + owner.transform.right;
        }
        else
        {
            newDir = AiPlayerDir - owner.transform.right;
        }

        Vector3 newMoveToPos = newDir * moveAwayAmount;

        return newMoveToPos;
        /*
        Psudocode for checking if too close to player
     
        START
        if(distanceToPlayer < certainDistance)
            THEN
        AItoPlayerDir = playerPos - AIpos
        AItoPlayerDir =AItoPlayerDir / distBetweenAInPlayer

        DirectionToHidingSpot = hidingSpotPos - AIpos
        DirToHideAngle = getAngle(DirectionToHidingSpot)

        if(DirToHideAngle > 0)
           THEN
        NewDir = AItoPlayerDir + RightDir
        else
            THEN
        NewDir = AItoPlayerDir - RightDir

        NewMoveToPos = NewDir * distanceToGo
    
    }*/

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
        UpdatePlayerPos();
        
        owner.NMA.destination = currentDestination;
        //owner.transform.position = Vector3.MoveTowards(owner.transform.position, hidingPos, moveSpeed * Time.deltaTime);

        if(CheckIfArrived(currentDestination))
        {

            SetToHideState();
        }
        

        /*if (CheckIfNearPlayer())
        {
            currentDestination = MoveAwayFromPlayer();
        }
        if (CheckIfArrived(currentDestination))
        {
            if(currentDestination != hidingPos)
            {
                currentDestination = hidingPos;
                //Debug.Log("Arrived at pos away from player");
            }
            else
            {
                SetToHideState();
            }
        }*/
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("CurrDest:" + currentDestination);
            Debug.Log("HidingPos:" + hidingPos);
        }
    }

    public void Exit()
    {
        owner.currHidingSpot = hidingPos;
        Debug.Log("Reached hiding spot");
    }
}
