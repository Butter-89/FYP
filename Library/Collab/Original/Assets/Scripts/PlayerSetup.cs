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

    [SerializeField]
    GameObject playerTechPoolPrefab;

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
        }
        else
        {
            sceneCamera = Camera.main;
            if(sceneCamera != null)
            {
                sceneCamera.gameObject.SetActive(false);
                 
            }
            SetupUI();
            CmdSetupTechPool();
        }
        
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

    [Command]
    private void CmdSetupTechPool()
    {
        playerTechPoolInstance = Instantiate(playerTechPoolPrefab);
        playerTechPoolInstance.name = playerTechPoolPrefab.name;
        TechTest TechPool = playerTechPoolInstance.GetComponent<TechTest>();

        if (TechPool == null)
            Debug.LogError("No PlayerUI component on PlayerUI prefab.");
        TechPool.SetPlayer(GetComponent<Player>());
        TechPool.name = TechPool.GetPlayer().name + "'s pool";
        GetComponent<ElementController>().buildingPool = GameObject.Find(TechPool.name);
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
