using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Chase : IState
{
    AI owner;
    public Chase(AI owner) { this.owner = owner; }

    //Variables
    Vector3 playerPos;
    Vector3 playerLastSeenPos;
    Vector3 destination;
    const float distToCatch = 0.75f;

    void UpdatePlayerPos()
    {
        playerPos = owner.currPlayerTransform.position;
    }

    bool IfNearDestination(Vector3 destination)
    {
        float distance = Vector3.Distance(owner.transform.position, destination);

        if(distance < distToCatch)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void CatchPlayer()
    {

    }

    public void GoToLastKnownPlayerLocation()
    {

    }

    public void CheckForPlayerEscape()
    {
        //Raycast to the player
        //If the raycast hits something that isn't the player start count down.
        //If the count down hits a certain point the AI has lost the player
    }

    bool CheckCanSeePlayer()
    {

        Vector3 dirToPlayer = owner.transform.position - playerPos;

        RaycastHit hitinfo;
        if(Physics.Raycast(owner.transform.position, owner.transform.forward, out hitinfo))
        {
            if(hitinfo.transform.gameObject.tag == "Player")
            {
                return true;
            }
            else
            {
                playerLastSeenPos = playerPos;
                return false;
            }
        }
        else
        {
            playerLastSeenPos = playerPos;
            return false;
        }
    }

    public void Enter()
    {
        Debug.Log("Starting Chase state");
    }

    public void Execute()
    {
        UpdatePlayerPos();

        if(CheckCanSeePlayer())
        {
            destination = playerPos;
        }
        else
        {
            destination = playerLastSeenPos;
        }
        owner.NMA.destination = destination;

        if(IfNearDestination(destination))
        {
            if(destination == playerPos)
            {
                CatchPlayer();
            }
        }
    }

    public void Exit()
    {
        Debug.Log("Exiting chase state");
    }
}