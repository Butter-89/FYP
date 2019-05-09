using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Projectile : MonoBehaviour
{
	public int radius;//Explosion radius
	public int power;
	public bool explosive;

	[SerializeField] private ParticleSystem explosionParticle;
	[SerializeField] private AudioSource explosionAudio;
	private void OnCollisionEnter(Collision other)
	{
		
		//Debug.Log("collided");
		//Debug.Break();
		Explode();
		
	}

	public void Explode()
	{
		if (explosive)
		{
			Instantiate(explosionParticle, transform.position, transform.rotation);
			explosionAudio.Play();
		}
		

		Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
		foreach (var c in colliders)
		{
			if (c.GetComponent<Castle>())
			{
				c.GetComponent<Castle>().CmdDamage(power);
			}

			if (c.GetComponent<Actor>())
			{
				c.GetComponent<Actor>().ReceiveDamage(power);
			}

			if (c.GetComponent<Player>())
			{
				c.GetComponent<Player>().RpcTakeDamage(power);
			}
		}
		var mesh = GetComponentInChildren<MeshRenderer>();
		mesh.enabled = false;
		GetComponent<BoxCollider>().enabled = false;
		Destroy(gameObject,5f);
	}

	public void Deactivate()
	{
		if (explosive)
		{
			Instantiate(explosionParticle, transform.position, transform.rotation);
			explosionAudio.Play();
		}
		
		var mesh = GetComponentInChildren<MeshRenderer>();
		mesh.enabled = false;
		GetComponent<BoxCollider>().enabled = false;
		Destroy(gameObject,5f);
	}

	
}
