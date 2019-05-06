using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
	public int maxHealth;

	private int _currentHealth;

	private bool _death;
	// Use this for initialization
	void Awake ()
	{
		_currentHealth = maxHealth;
		_death = false;
	}
	
	// Update is called once per frame
	public void Damage(int value)
	{
		_currentHealth -= value;
		if (_currentHealth<0)
		{
			_currentHealth = 0;
		}

		if (_currentHealth == 0)
		{
			_death = true;
			Dying();
			Dead();
		}
	}

	void Dying()
	{
		
	}

	void Dead()
	{
		Destroy(gameObject);
	}
	
	
}
