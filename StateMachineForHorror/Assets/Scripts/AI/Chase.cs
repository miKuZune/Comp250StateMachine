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

	//Show the game over screen when the player is caught
    public void CatchPlayer()
    {
        GameObject.Find("UIManager").GetComponent<UIManager>().ShowGameOver();
    }

	//Raycast to the player and see if the raycast is interupted by anything
    bool CheckCanSeePlayer()
    {

        Vector3 dirToPlayer = playerPos - owner.transform.position;

        RaycastHit hitinfo;
        if(Physics.Raycast(owner.transform.position, dirToPlayer, out hitinfo))
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

		//Sets the destination to the player position if the AI can see the player
		//This means if the AI cannot see the player it will go to the last position it saw the player at.
        if(CheckCanSeePlayer())
        {
            destination = playerPos;
        }
        owner.NMA.destination = destination;

		//If the player arrives at a destination it sees which destination it has arrived at and acts accordingly.
        if(IfNearDestination(destination))
        {
            if(destination == playerPos)
            {
                CatchPlayer();
            }
            else
            {
				//If the AI has arrived at it's destination and cannot see the player then the state is changed.
                if(!CheckCanSeePlayer())
                {
                    owner.state.ChangeState(new GoToHidingSpot(owner));
                }
            }
        }
    }

    public void Exit()
    {
        Debug.Log("Exiting chase state");
    }
}