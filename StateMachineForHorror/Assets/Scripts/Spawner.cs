using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    public GameObject objToSpawn;

    float timeSinceLastSpawn;
    const float timeForSpawn = 3.5f;

    public int numToSpawn;

    bool hasSpawned = false;

    void Spawn()
    {
        Debug.Log("Spawining");
        for(int i = 0; i < numToSpawn; i++)
        {
            Instantiate(objToSpawn, transform.position, Quaternion.identity);
        }
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        
	}

    void OnTriggerEnter(Collider coll)
    {
        if(coll.gameObject.tag == "Player" || coll.gameObject.tag == "AI")
        {
            if(!hasSpawned)
            {
                Spawn();
                hasSpawned = true;
            }
            
        }
    }
}
