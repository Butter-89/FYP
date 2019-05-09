using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class EnergyShield : MonoBehaviour
{
	
	//public GameObject sphereShield;
	//public List<Collider> targets;
	public float shieldScale;
	private Vector3 _localScale;
	private SphereCollider _shield;
	
	// Use this for initialization
	void Start ()
	{
		//_localScale = gameObject.transform.localScale;
		transform.localScale = new Vector3(shieldScale,shieldScale,shieldScale);
		
		_shield = GetComponent<SphereCollider>();
	}
	
	// Update is called once per frame


	private void OnTriggerEnter(Collider other)
	{
		if (other.GetComponent<Projectile>())
		{
			//Debug.Log("Collider center: "+gameObject.transform.position+" Object position: "+other.transform.position);
			float distance = Vector3.Distance(other.transform.position, gameObject.transform.position);
			float radius = _shield.radius * gameObject.transform.localScale.x / 5; //just trying to figure out how to calculate
			//Debug.Log("Shield radius: " + radius+" Distance: "+distance);
			if (distance>=radius)
			{
				//Debug.Log("Inside the shield!");
				other.GetComponent<Projectile>().Deactivate();
			}

			//other.GetComponent<Projectile>().Deactivate();
			
		}
	}
	
}
