using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {

    public int health;

    public float moveSpeed;
    public float sensitivity;
    public float jumpPower;

    GameObject thisCamera;
    bool hasJumped;
	// Use this for initialization
	void Start ()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        thisCamera = Camera.main.gameObject;
        hasJumped = false;
	}

    void MovementHandling(float moveSpeed)
    {
        var x = Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed;
        var z = Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed;

        transform.Translate(x, 0, z);

        if(Input.GetKeyDown(KeyCode.Space))
        {
            Rigidbody thisRB = GetComponent<Rigidbody>();
            Vector3 newVel = thisRB.velocity;
            newVel.y = jumpPower;

            thisRB.velocity = newVel;
        }
    }
	
    void WholeCharacterLook(float sensitivity)
    {
        float mouseX = sensitivity * Input.GetAxis("Mouse X");
        transform.Rotate(0, mouseX, 0);
    }

    void CheckIfDead()
    {
        if(health <= 0)
        {

        }
    }

	// Update is called once per frame
	void Update ()
    {
        MovementHandling(moveSpeed);
        WholeCharacterLook(sensitivity);
    }
}
