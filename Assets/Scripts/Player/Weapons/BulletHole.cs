using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHole : MonoBehaviour
{
	public GameObject bulletHole;//Prefab
	public List<GameObject> bulletHoleList;
	public int initCount;
	void Start () {
		for (int i = 0; i < initCount; i++)
		{
			GameObject hole = Instantiate(bulletHole,this.transform) as GameObject;
			bulletHoleList.Add(hole);
			hole.SetActive(false);
		}
	}
	
	void Update () {
		
	}
}
