using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerMotor : MonoBehaviour {
    public ElementManager eManager;

    private Vector3 velocity = Vector3.zero;
    private Vector3 rotation = Vector3.zero;
    private Vector3 cameraTilt = Vector3.zero;
    private float margin = 0.2f;
    private bool floating = false;
    private Rigidbody rb;
    [SerializeField]
    private Camera camera;

    public float jumpHeight;
    //private CharacterController character;
    private bool castleBuilt = false;

	void Start () {
        rb = GetComponent<Rigidbody>();
        //character = GetComponent<CharacterController>();
	}

    public void Move(Vector3 _velocity)
    {
        velocity = _velocity;
    }

    public void Rotate(Vector3 _rotation)
    {
        rotation = _rotation;
    }
    public void Tilt(Vector3 _cameraTilt)
    {
        cameraTilt = _cameraTilt;
    }
	
	void FixedUpdate () {
        if(IsGrounded())
        {
            PerformMovement();
        }
        
        PerformRotation();
        //Debug.Log(IsGrounded());
        //castleBuilt = eManager.CastleBuilt();
        //Debug.Log(castleBuilt);
        if (!castleBuilt)
        {
            if (IsGrounded())
            {
                floating = false;
            }
        }
        else
            floating = false;
        
	}

    void PerformMovement()
    {
        if(velocity != Vector3.zero && !floating)
        {
            rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
        }
    }

    void PerformRotation()
    {
        rb.MoveRotation(rb.rotation * Quaternion.Euler(rotation));

        if (camera != null)
        {
            camera.transform.localRotation = Quaternion.Euler(cameraTilt);
        }
    }

    public void PerformJump(Vector3 _velocity)
    {
        /*if (!castleBuilt)
        {
            Debug.Log("Castle not built");
            if (IsGrounded())
            {
                rb.velocity = Vector3.up * 10f + _velocity;
                //Debug.Log("Jump!");
            }
        }
        else*/
            rb.velocity = Vector3.up * jumpHeight + _velocity;

    }

    bool IsGrounded()
    {
        return Physics.Raycast(transform.position, -Vector3.up, margin);
    }
}
