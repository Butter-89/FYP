using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Castle : NetworkBehaviour
{
    [SerializeField] private int dTechLevel;
    [SerializeField] private int aTechLevel;
    public int teamNo;//Used to indicate the which team
    //Team 1- No 1, Team 2 - No 2
    [SerializeField] private int health;
    private bool sinking;
    private bool building;
    [SerializeField] private float sinkSpeed = 3f;
    private bool destroyed;

    public GameObject explosionParticleSystem;
    public AudioSource collapse;
    public bool isBase;
    public float _Timer = 3f;

    void Start () {
        health = 100;
        sinking = false;
        destroyed = false;
        building = false;
        // Just for testing techLevel = 1;
    }
	
	// Update is called once per frame
	void Update () {
        if (health <= 0 && !destroyed)
            Collapse();
        if (sinking)
        {
            _Timer -= Time.deltaTime;
            transform.Translate(-Vector3.up * sinkSpeed * Time.deltaTime);
            if (_Timer < 0)
                _Timer = 0;

            if (_Timer > 0)
                return;

            if (_Timer == 0)
                NetworkServer.Destroy(gameObject); //here!
        }
        
    }

	
    public void CmdDamage(int dmg)
    {
        Debug.Log("Castle: "+ transform.name + "hit with damage: "+ dmg);
        CmdUpdateHealth(dmg);
    }

    //[ClientRpc]
    
    private void CmdUpdateHealth(int dmg)
    {
        if (health - dmg > 0)
            health -= dmg;
        else
            health = 0;
    }

    private void Collapse() //castle destroyed by the other side
    {
        
        
        if (explosionParticleSystem)
        {
            Instantiate(explosionParticleSystem, this.transform.position, Quaternion.identity);
        }

        if (collapse)
        {
            collapse.Play();
        }
        sinking = true;
        destroyed = true;
        
    }

    private void Build()
    {
        building = true;
    }

    public int TechLevel(string branch)
    {
        if(branch=="D")
            return dTechLevel;
        if (branch == "A")
            return aTechLevel;
        else
        {
            return 0;
        }
    }
}
