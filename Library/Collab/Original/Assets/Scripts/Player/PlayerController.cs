using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {
    [SerializeField]
    private float speed = 5f;
    [SerializeField]
    private float lookSensitivity = 3f;

    private PlayerMotor motor;
    private float yRot;
    private float xRot;
    [SerializeField]
    private Camera cam;
    private bool flag = false;
    public ElementManager elementManager;

    

    void Start () {
        motor = GetComponent<PlayerMotor>();
    }


    void Update()
    {
        float xMove = Input.GetAxisRaw("Horizontal");
        float zMove = Input.GetAxisRaw("Vertical");

        Vector3 moveHorizontal = transform.right * xMove;
        Vector3 moveVertical = transform.forward * zMove;

        yRot = Input.GetAxisRaw("Mouse X");
        xRot -= Input.GetAxisRaw("Mouse Y") * lookSensitivity;
        xRot = Mathf.Clamp(xRot, -60f, 60f);

        Vector3 _rotation = new Vector3(0f, yRot, 0f) * lookSensitivity;
        Vector3 _cameraTilt = new Vector3(xRot, 0f, 0f) ;
        Vector3 _velocity = (moveHorizontal + moveVertical).normalized * speed;

        motor.Move(_velocity);
        motor.Rotate(_rotation);
        motor.Tilt(_cameraTilt);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PickUp"))
        {
            Debug.Log("Collided");
            other.gameObject.SetActive(false);
            elementManager.elements.Add("Fire ");
            other.gameObject.GetComponent<SphereCollider>().isTrigger = false;
        }
    }

}
