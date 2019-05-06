using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class ElementSpawner : NetworkBehaviour {

    public Transform[] spawnPoints;
    public GameObject[] Elements;

    public float spawnTime = 1f;
    public Vector3 size;


    private void Awake()
    {
       
        InvokeRepeating("Spawn", spawnTime, spawnTime);
    }

    public void Spawn()
    {
        /*for (int i = 0; i < spawnPoints.Length; i++)
        {
            GameObject elementSpawned = Instantiate(Elements[i], spawnPoints[i].position + new Vector3(Random.Range(-size.x / 2, size.x / 2), 0, Random.Range(-size.z / 2, size.z / 2)), Quaternion.identity);
            NetworkServer.Spawn(elementSpawned);
            Debug.Log("spawned");
        }*/


        /*if (isServer)
        {
            Debug.Log("Is server");
        }

        if(!isClient)
        {
            Debug.Log("Is client");
            return;
        }*/
        if (isServer)
        {
            for (int i = 0; i < spawnPoints.Length; i++)
            {
                GameObject elementSpawned = Instantiate(Elements[i], spawnPoints[i].position + new Vector3(Random.Range(-size.x / 2, size.x / 2), 0, Random.Range(-size.z / 2, size.z / 2)), Quaternion.identity);
                NetworkServer.Spawn(elementSpawned);
                Destroy(elementSpawned, spawnTime * 3);
            }
        }
            



        }

    // Update is called once per frame
    void Update () {
		 
         /*if(Input.GetKeyDown(KeyCode.G))
        {
            Spawn();
        }*/
	}
}
