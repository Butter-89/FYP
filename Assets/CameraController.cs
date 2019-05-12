using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	public float rotationSpeed;

	public Transform target, player;

	private float _mouseX, _mouseY;

	private bool _toggleLock;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void LateUpdate () {
		CamControl();
		

	}

	private void CamControl()
	{
		_mouseX += Input.GetAxis("Mouse X") * rotationSpeed;
		_mouseY -= Input.GetAxis("Mouse Y") * rotationSpeed;
		_mouseY = Mathf.Clamp(_mouseY, -35, 35);
		
		transform.LookAt(target);
		target.rotation = Quaternion.Euler(_mouseY, _mouseX, 0);
		player.rotation=Quaternion.Euler(0,_mouseX,0);
	}
}
