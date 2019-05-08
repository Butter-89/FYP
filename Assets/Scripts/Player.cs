using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Player : NetworkBehaviour
{
    [SyncVar]
    private bool _isDead = false;

    public bool IsDead
    {
        get { return _isDead; }
        protected set { _isDead = value; }
    }

    [SerializeField]
    private int maxHealth = 100;

    [SyncVar]
    private int currentHealth;

    [SerializeField] private Behaviour[] disableOnDeath;
    private bool[] wasEnabled;

    public Weapon[] weapons;
    public int activeWeapon;

    private GameObject projectile;

    public void Setup()
    {
        activeWeapon = 0;   //for testing, the default weapon is pistol (1)
        wasEnabled = new bool[disableOnDeath.Length];
        for (int i = 0; i < wasEnabled.Length; i++)
        {
            wasEnabled[i] = disableOnDeath[i].enabled;
        }
        SetDefaults();
    }
    
    
/*
    private void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            RpcTakeDamage(99999);
        }
    }*/

    [ClientRpc]
    public void RpcTakeDamage(int _amounts)
    {
        if (IsDead)
        {
            return;
        }
        
        
        currentHealth -= _amounts;
        //Debug.Log(transform.name + " now has " + currentHealth +  " health");
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    
    [Command]
    public void CmdDamage(int _amounts, string playerID)
    {
        //Debug.Log(playerID + "has been shot");

        Player player = GameManager.GetPlayer(playerID);
        player.RpcTakeDamage(_amounts);
        //Debug.Log(player.name + " health: " + player.currentHealth);
    }

    [Command]
    public void CmdHitCastle(int dmg, NetworkInstanceId id)
    {
        //Debug.Log(id);
        RpcCastle(dmg,id);
    }

    [ClientRpc]
    private void RpcCastle(int dmg, NetworkInstanceId id)
    {
        if(isClient)
            ClientScene.FindLocalObject(id).GetComponent<Castle>().CmdDamage(dmg);
        
        //Debug.Log(NetworkServer.FindLocalObject(id).name);
        //NetworkServer.FindLocalObject(id).GetComponent<Castle>().CmdDamage(dmg);
    }
    
    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(3f);
	    
        SetDefaults();
        Transform _spawnPosition = NetworkManager.singleton.GetStartPosition();
        transform.position = _spawnPosition.position;
        transform.rotation = _spawnPosition.rotation;
    }
    public void SetDefaults()
    {
        _isDead = false;
        currentHealth = maxHealth;
        
        for (int i = 0; i < disableOnDeath.Length; i++)
        {
            disableOnDeath[i].enabled = wasEnabled[i];
        }

        CapsuleCollider _col = GetComponent<CapsuleCollider>();
        if (_col!=null)
        {
            _col.enabled = true;
        }
        
    }

    [Command]
    public void CmdSyncShotEffect()
    {
        RpcShotEffect();
    }

    [ClientRpc]
    private void RpcShotEffect()
    {
        weapons[activeWeapon].DoShotEffect();
    }

    [Command]
    public void CmdGenerateProjectile(float Px, float Py, float Pz, float Qx, float Qy, float Qz, float W)
    {
        
        RpcSyncProjectile(Px,Py,Pz,Qx,Qy,Qz,W);
        //weapons[activeWeapon].GenerateProjectile();
    }

    [ClientRpc]
    private void RpcSyncProjectile(float Px, float Py, float Pz, float Qx, float Qy, float Qz, float W)
    {
        var position = new Vector3(Px,Py,Pz);
        var rotation = new Quaternion();
        rotation.Set(Qx,Qy,Qz,W);
        
        Debug.Log("Active weapon: "+activeWeapon);
        projectile = weapons[activeWeapon].projectile;
        //var raycastSpot = weapons[activeWeapon].raycastStartSpot;
        var newProjectile = Instantiate(projectile, position, rotation);
        //if(!isServer)
            //NetworkServer.Spawn(newProjectile);
        //Destroy(newProjectile);
        if (newProjectile.GetComponent<Rigidbody>())
        {
            Rigidbody rb = newProjectile.GetComponent<Rigidbody>();
            rb.AddForce(newProjectile.transform.forward*weapons[activeWeapon].forceMultiplier);
            
        }
        else
        {
            Debug.Log("Rigidbody is not attached to this projectile!");
        }
        CmdSyncShotEffect();
        //weapons[activeWeapon].GenerateProjectile();
    }

    [Command]
    public void CmdProjectileForce()
    {
        projectile = weapons[activeWeapon].projectile;
        
    }

    [ClientRpc]
    private void RpcAddForce()
    {
        
    }

    [Command]
    public void CmdOnHit(string playerID)
    {
        Debug.Log(playerID + "is hit.");
    }

    [Command]
    public void CmdUpdateWeapon(int weapon)
    {
        //activeWeapon = weapon;
        RpcUpdateWeapon(weapon);
    }

    [ClientRpc]
    private void RpcUpdateWeapon(int weapon)
    {
        activeWeapon = weapon;
    }
    /*
   [Command]
    public void CmdHitCastle(int dmg, Transform tr)
    {
        RpcCastleHealth(dmg, tr);
    }

    [ClientRpc]
    private void RpcCastleHealth(int dmg, Transform tr)
    {
        tr.gameObject.GetComponent<Castle>().CmdDamage(dmg);
    }

    */
    public void Die()
    {
        _isDead = true;
        GetComponent<Collider> ().enabled = false;
        //GetComponent<Renderer> ().enabled = false;
        CapsuleCollider _col = GetComponent<CapsuleCollider>();
        if (_col!=null)
        {
            _col.enabled = false;
        }
        for (int i = 0; i < disableOnDeath.Length; i++)
        {
            disableOnDeath[i].enabled = false;
        }
        
        
        //Debug.Log(transform.name+" is dead");

        StartCoroutine(Respawn());
        
        //return _isDead;
    }
}