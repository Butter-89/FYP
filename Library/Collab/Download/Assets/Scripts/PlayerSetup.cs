using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(Player))]
public class PlayerSetup : NetworkBehaviour {
    

    [SerializeField]
    Behaviour[] componentsToDisable;

    Camera sceneCamera;

    [SerializeField]
    GameObject playerUIPrefab;

    [HideInInspector]
    private GameObject playerUIInstance;

    [HideInInspector]
    private GameObject playerTechPoolInstance;

    void Start ()
    {
		if(!isLocalPlayer)
        {
            for (int i=0; i<componentsToDisable.Length; i++)
            {
                componentsToDisable[i].enabled = false; 
            }

            if (isServer)
                SetupTeam2Tech();
            else
                SetupTeam1Tech();
        }
        else
        {
            sceneCamera = Camera.main;
            if(sceneCamera != null)
            {
                sceneCamera.gameObject.SetActive(false);
                 
            }
            SetupUI();
            //SetupTechPool();
            
            if(isServer)
                SetupTeam1Tech();
            else
                SetupTeam2Tech();
                
        }

        GetComponent<Player>().Setup();

    }
    
    

    public override void OnStartClient()
    {
        base.OnStartClient();
        string _netID = GetComponent<NetworkIdentity>().netId.ToString();
        Player _player = GetComponent<Player>();
        GameManager.RegisterPlayer(_netID, _player);


    }

    private void SetupUI()
    {
        playerUIInstance = Instantiate(playerUIPrefab);
        playerUIInstance.name = playerUIPrefab.name;
        PlayerUI ui = playerUIInstance.GetComponent<PlayerUI>();

        if (ui == null)
            Debug.LogError("No PlayerUI component on PlayerUI prefab.");
        ui.SetPlayer(GetComponent<Player>());

    }

    
    /*private void SetupTechPool()
    {
        playerTechPoolInstance = Instantiate(playerTechPoolPrefab);
        NetworkServer.Spawn(playerTechPoolInstance);
        playerTechPoolInstance.name = playerTechPoolPrefab.name;
        TechTest TechPool = playerTechPoolInstance.GetComponent<TechTest>();

        if (TechPool == null)
            Debug.LogError("No PlayerUI component on PlayerUI prefab.");
        TechPool.SetPlayer(GetComponent<Player>());
        TechPool.name = TechPool.GetPlayer().name + "'s pool";
        GetComponent<ElementController>().buildingPool = GameObject.Find(TechPool.name);
    }*/


    void SetupTeam1Tech()
    {
        GetComponent<ElementController>().buildingPool = GameObject.Find("Team1");
    }

    void SetupTeam2Tech()
    {
        GetComponent<ElementController>().buildingPool = GameObject.Find("Team2");
    }


void OnDisable()
    {
        if(sceneCamera != null)
        {
            sceneCamera.gameObject.SetActive(true);
        }

        GameManager.UnRegisterPlayer(transform.name);
    }
}
