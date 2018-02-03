using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ambush : IState {

	AI owner;

    public Ambush(AI owner) { this.owner = owner; }

    public void Enter()
    {
        Debug.Log("Beggining ambush");
    }
    public void Execute()
    {
        Debug.Log("Waiting to ambush");
    }
    public void Exit()
    {
        Debug.Log("Stopping the ambush");
    }
}
