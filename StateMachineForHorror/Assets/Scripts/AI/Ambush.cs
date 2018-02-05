using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ambush : IState {

	AI owner;

    float timeAtSpot = 0;

    const float signifigantTime = 20f;
    const float distToBeSignifigant = 15f;


    public Ambush(AI owner) { this.owner = owner; }

    public void Enter()
    {
        Debug.Log("Beggining ambush");
    }
    public void Execute()
    {
        if(timeAtSpot > signifigantTime)
        {
            
            float distToPlayer = Vector3.Distance(owner.transform.position, owner.currPlayerTransform.transform.position);
            if (distToPlayer > distToBeSignifigant)
            {
                owner.state.ChangeState(new GoToHidingSpot(owner));
            }
        }

        
        timeAtSpot += Time.deltaTime;
    }
    public void Exit()
    {
        Debug.Log("Stopping the ambush");
    }
}
