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

    public void Setup()
    {
        
        wasEnabled = new bool[disableOnDeath.Length];
        for (int i = 0; i < wasEnabled.Length; i++)
        {
            wasEnabled[i] = disableOnDeath[i].enabled;
        }
        SetDefaults();
    }

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
    }

    [ClientRpc]
    public void RpcTakeDamage(int _amounts)
    {
        if (IsDead)
        {
            return;
        }
        
        
        currentHealth -= _amounts;
        Debug.Log(transform.name + " now has " + currentHealth +  " health");
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
    public void CmdOnHit(string playerID)
    {
        Debug.Log(playerID + "is hit.");
    }

    
    public void Die()
    {
        _isDead = true;
        for (int i = 0; i < disableOnDeath.Length; i++)
        {
            disableOnDeath[i].enabled = false;
        }
        CapsuleCollider _col = GetComponent<CapsuleCollider>();
        if (_col!=null)
        {
            _col.enabled = false;
        }
        
        Debug.Log(transform.name+" is dead");

        StartCoroutine(Respawn()); 
    }
}
