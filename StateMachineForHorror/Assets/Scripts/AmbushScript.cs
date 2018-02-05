using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbushScript : MonoBehaviour {

    public GameObject objToMissplace;

    public Transform objNewTransformPos;
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
        float distFromAI = Vector3.Distance(AI.transform.position, transform.position);
        if (distFromAI < signifigantDist)
        {
            active = true;
            objToMissplace.transform.position = objNewTransformPos.position;
            objToMissplace.transform.rotation = objNewTransformPos.rotation;
        }
        else
        {
            active = false;
        }
        
    }
    void OnTriggerEnter(Collider coll)
    {
        if (active)
        {
            if(coll.gameObject.tag == "Player")
            {
                GameObject.Find("UIManager").GetComponent<UIManager>().ShowGameOver();
            }
        }
    }
}
