using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowFly : MonoBehaviour {

    public float moveSpeed;
    Vector3 moveDir;

    float timeAlive;
    const float timeTillDespawn = 20f;

	// Use this for initialization
	void Start () {
        moveDir = transform.up;
        moveDir.x = Random.Range(-0.35f, 0.35f);
        moveDir.z = Random.Range(-0.35f, 0.35f);
	}
	
	// Update is called once per frame
	void Update () {
        if(timeAlive > timeTillDespawn)
        {
            Destroy(this.gameObject);
        }

        timeAlive += Time.deltaTime;
        transform.position = transform.position + (moveSpeed * moveDir * Time.deltaTime);
	}
}
