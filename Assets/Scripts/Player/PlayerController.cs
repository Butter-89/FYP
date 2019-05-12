using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {
    [SerializeField]
    private float speed = 5f;
    [SerializeField]
    private float lookSensitivity = 3f;
    private bool moving;

    private Animator animator;
    
    [SerializeField] private float m_StickToGroundForce;

    private CharacterController _charCtrl;

    public TechManager tm;
    
    private bool isLocalPlayer;

    private PlayerMotor motor;
    private float yRot;
    private float xRot;
    [SerializeField]
    private Camera cam;


    void Start () {
        motor = GetComponent<PlayerMotor>();
        animator = GetComponent<Animator>();
        tm = GetComponentInChildren<TechManager>();
        isLocalPlayer = true;
        _charCtrl = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        
        Weapon[] allWeapons = transform.GetComponentsInChildren<Weapon>();
        for (int i = 0; i < allWeapons.Length; i++)
        {
            allWeapons[i].isLocalPlayer = true;
            allWeapons[i].gameObject.SetActive(false);
        }
        tm.InitializeWeapon();
        //Debug.Log(allWeapons.Length);
    }


    void Update()
    {
        if(PauseMenu.IsOn||EndGame.IsOn||UserGuide.IsOn||TechTreeUI.IsOn)
        {
            Cursor.lockState = CursorLockMode.None;
            return;
        }

        if (!PauseMenu.IsOn&&!UserGuide.IsOn&&!TechTreeUI.IsOn)
            Cursor.lockState = CursorLockMode.Locked;


        //WASD


        float xMove = Input.GetAxisRaw("Horizontal");
        float zMove = Input.GetAxisRaw("Vertical");

        
        
        Vector3 moveHorizontal = transform.right * xMove;
        Vector3 moveVertical = transform.forward * zMove;

        //Mouse control
        yRot = Input.GetAxisRaw("Mouse X");
        xRot -= Input.GetAxisRaw("Mouse Y") * lookSensitivity;
        xRot = Mathf.Clamp(xRot, -60f, 60f);
        
        //Perform movement & rotation

        Vector3 _rotation = new Vector3(0f, yRot, 0f) * lookSensitivity;
        Vector3 _cameraTilt = new Vector3(xRot, 0f, 0f) ;
        Vector3 _velocity = (moveHorizontal + moveVertical).normalized * speed;
        
        if (xMove == 0f && zMove == 0f)
        {
            animator.SetBool("moving",false);
        }
        else
        {
            animator.SetBool("moving",true);
        }
		
        animator.SetFloat("inputH", xMove);
        animator.SetFloat("inputV",zMove);

        
        motor.Move(_velocity);
        motor.Rotate(_rotation);
        motor.Tilt(_cameraTilt);
        //Debug.Log("Character is grounded: "+_charCtrl.isGrounded);
        if (Input.GetButtonDown("Jump") )
        {
            //_velocity.y = jumpSpeed;
            
            motor.PerformJump(_velocity);
        }

        if (Input.GetKeyDown("escape"))
        {
            Cursor.lockState = CursorLockMode.None;
        }
        
    }

    

}
