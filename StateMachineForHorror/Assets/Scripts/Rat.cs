using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Rat : MonoBehaviour {

    NavMeshAgent NMagent;

    float timeAlive;
    const float timeToDespawn = 20f;

	// Use this for initialization
	void Start () {
        NMagent = GetComponent<NavMeshAgent>();

        Vector3 destination = new Vector3(0, 0, 0);
        destination.x = Random.Range(-100, 100);
        destination.z = Random.Range(-100, 100);

        NMagent.destination = destination;
	}
	
	// Update is called once per frame
	void Update () {
		if(timeAlive > timeToDespawn)
        {
            Destroy(this.gameObject);
        }
        timeAlive += Time.deltaTime;
	}
}
