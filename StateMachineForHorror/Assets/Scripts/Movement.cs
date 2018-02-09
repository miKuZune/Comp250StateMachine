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

    const float distToSeeFloor = 0.9999f;
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

        

        if (Input.GetKey(KeyCode.LeftShift))
        {
            x = x * 2;
            z = z * 2;
        }

        Vector3 translateBy = new Vector3(x, 0, z);

        transform.Translate(x, 0, z);

        if(Input.GetKeyDown(KeyCode.Space) && !hasJumped)
        {
            Rigidbody thisRB = GetComponent<Rigidbody>();
            Vector3 newVel = thisRB.velocity;
            newVel.y = jumpPower;

            thisRB.velocity = newVel;
            hasJumped = true;
        }

        
    }
	
    void CheckIfCanJump()
    {
        if(Physics.Raycast(transform.position, Vector3.down, distToSeeFloor))
        {
            hasJumped = false;
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
        CheckIfCanJump();
    }
}
