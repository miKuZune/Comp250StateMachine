using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamController : MonoBehaviour {


    float sensitivity;

    float yRotate = 0.0f;
	// Use this for initialization
	void Start () {
        sensitivity = GetComponentInParent<Movement>().sensitivity;
	}

    void LookWithCamera(float sensitivity)
    {
        Vector3 rot = transform.eulerAngles;

        yRotate += Input.GetAxis("Mouse Y") * sensitivity * 20.0f * Time.deltaTime;

        yRotate = Mathf.Clamp(yRotate, -45, 45);
        rot.x = -yRotate;

        transform.eulerAngles = rot;
    }

    // Update is called once per frame
    void Update () {
        LookWithCamera(sensitivity);
	}
}
