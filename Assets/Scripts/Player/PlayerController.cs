using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {
    [SerializeField]
    private float speed = 5f;
    [SerializeField]
    private float lookSensitivity = 3f;

    public TechManager tm;
    
    private bool isLocalPlayer;

    private PlayerMotor motor;
    private float yRot;
    private float xRot;
    [SerializeField]
    private Camera cam;


    void Start () {
        motor = GetComponent<PlayerMotor>();
        tm = GetComponentInChildren<TechManager>();
        isLocalPlayer = true;

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
        if(PauseMenu.IsOn||EndGame.IsOn||UserGuide.IsOn)
        {
            Cursor.lockState = CursorLockMode.None;
            return;
        }

        if (!PauseMenu.IsOn&&!UserGuide.IsOn)
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

        
        motor.Move(_velocity);
        motor.Rotate(_rotation);
        motor.Tilt(_cameraTilt);

        if (Input.GetButtonDown("Jump"))
        {
            motor.PerformJump(_velocity);
        }

        if (Input.GetKeyDown("escape"))
        {
            Cursor.lockState = CursorLockMode.None;
        }
        
    }

    

}
