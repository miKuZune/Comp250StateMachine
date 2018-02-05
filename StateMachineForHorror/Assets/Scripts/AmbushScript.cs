using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbushScript : MonoBehaviour {

    public GameObject objToMissplace;

    public Transform objNewTransformPos;

    public GameObject hitBox;
    bool active;


    GameObject AI;
    const float signifigantDist = 1.5f;
    void Start()
    {
        active = false;
        AI = GameObject.FindGameObjectWithTag("AI");
    }

    void Update()
    {
        if(!active)
        {
            float distFromAI = Vector3.Distance(AI.transform.position, transform.position);
            if (distFromAI < signifigantDist)
            {
                Debug.Log("Hello it me");
            }
        }
        else
        {

        }
        
    }
}
