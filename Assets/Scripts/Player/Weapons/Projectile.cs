using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public enum ExplosionType
{
	Immediate,
	Delayed
}

public class Projectile : MonoBehaviour
{
	public int radius;//Explosion radius
	public int power;
	public bool explosive;
	public ExplosionType projectileType;

	private IEnumerator _coroutine;

	[SerializeField] private ParticleSystem explosionParticle;
	[SerializeField] private AudioSource explosionAudio;
	private void OnCollisionEnter(Collision other)
	{
		//Debug.Log("collided");
		//Debug.Break();
		if (projectileType==ExplosionType.Immediate)
		{
			ImmediateExplode();
		}
	}

	private void Awake()
	{
		if (projectileType==ExplosionType.Delayed)
		{
			_coroutine = DelayedExplode();
			StartCoroutine(_coroutine);
		}
	}

	public void ImmediateExplode()
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
		if (GetComponent<BoxCollider>())
		{
			GetComponent<BoxCollider>().enabled = false;
		}
		else if (GetComponent<MeshCollider>())
		{
			GetComponent<MeshCollider>().enabled = false;
		}
		
		Destroy(gameObject,5f);
	}

	public IEnumerator DelayedExplode()
	{
		
		yield return new WaitForSeconds(5);
		
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
