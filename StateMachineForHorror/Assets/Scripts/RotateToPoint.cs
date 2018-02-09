using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateToPoint : MonoBehaviour {

    public GameObject pointToRotateTo;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        Vector3 thisPosNoY = transform.position;
        Vector3 thatPosNoY = pointToRotateTo.transform.position;

        thisPosNoY.y = 0;
        thatPosNoY.y = 0;

        Vector3 newDir = thatPosNoY - thisPosNoY;

        transform.rotation = Quaternion.LookRotation(newDir);

	}
}
