using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerMotor : MonoBehaviour {
    public ElementManager eManager;

    private Animator anim;
    private Vector3 velocity = Vector3.zero;
    private Vector3 rotation = Vector3.zero;
    private Vector3 cameraTilt = Vector3.zero;
    [SerializeField] private float margin = 0.2f;
    [SerializeField] private float m_StickToGroundForce;
    [SerializeField] private float m_GravityMultiplier;
    [SerializeField] private float rotationSpeed;
    private bool floating = false;
    private Rigidbody rb;
    [SerializeField]
    private Camera camera;

    private float xMove, zMove;
    

    public float jumpHeight;

    public Transform target;
    //private CharacterController character;
    private bool castleBuilt = false;
    private PlayerController _controller;

	void Start () {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        _controller = GetComponent<PlayerController>();
        //character = GetComponent<CharacterController>();
    }

    public void Move(Vector3 _velocity)
    {
        if (!IsGrounded())
        {
            _velocity += Physics.gravity * m_GravityMultiplier * Time.fixedDeltaTime;
        }
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
        //Debug.Log("Is grounded? "+IsGrounded());
        xMove = _controller.xMove;
        zMove = _controller.zMove;
        
        PerformMovement();
        PerformRotation();
        //CamControl(xMove,zMove);
        
        if (IsGrounded())
        {
            anim.SetBool("Grounded", true);
        }
        else
        {
            anim.SetBool("Grounded",false);
        }
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
        
        if(velocity != Vector3.zero)
        {
            rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
        }
    }

    void PerformRotation()
    {
        rb.MoveRotation(rb.rotation * Quaternion.Euler(rotation));
        if (camera != null)
        {
            target.localRotation = Quaternion.Euler(cameraTilt);
        }
        
    }
    
    private void CamControl(float _mouseX, float _mouseY)
    {
        if (camera != null)
        {
            _mouseX += Input.GetAxis("Mouse X") * rotationSpeed;
            _mouseY -= Input.GetAxis("Mouse Y") * rotationSpeed;
            _mouseY = Mathf.Clamp(_mouseY, -35, 35);
		
            transform.LookAt(target);
            target.rotation = Quaternion.Euler(_mouseY, _mouseX, 0);
            //player.rotation=Quaternion.Euler(0,_mouseX,0);
            //camera.transform.localRotation = Quaternion.Euler(cameraTilt);
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
        if (IsGrounded())
        {
            rb.velocity = Vector3.up * jumpHeight + _velocity;
        }
            

    }

    public bool IsGrounded()
    {
        return Physics.Raycast(transform.position, -Vector3.up, margin);
    }
}
