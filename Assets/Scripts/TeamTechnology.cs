using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class TeamTechnology : NetworkBehaviour {

    [SyncVar]
    public int techLevel;

    [SerializeField] private int techDirection;
    [SerializeField] private int maxTechLevel;
    private int _currentWeapon;


    private PlayerController playerController;
    private ElementController elementController;
    private Player player;

    private Castle[] _castles;
    public GameObject[] weaponCategory;
    public GameObject buildingPool;

    public int teamNo;
    // Use this for initialization
    void Start()
    {
        techDirection = 0;
        techLevel = 0;
        _currentWeapon = 0;
        maxTechLevel = 3; //temporarily max tech level = 3
        buildingPool = gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        //if(isServer)
            //UpdateTechLevel();
    }
    
    public void UpdateTechLevel()
    {
        techLevel = -1;
        //Check for tech-level update
        _castles = buildingPool.GetComponentsInChildren<Castle>();
        foreach (Castle castle in _castles)
        {
            if (techLevel < maxTechLevel)
            {
                //techLevel += castle.TechLevel();
            }
        }
    }
    
    public int GetTechLevel()
    {
        return techLevel;
    }





    public void SetPlayer(Player _player)
    {
        player = _player;
        playerController = player.GetComponent<PlayerController>();
        elementController = player.GetComponent<ElementController>();
        //buildingPool.name = player.name + "'s Technology";
    }

    public Player GetPlayer()
    {
        return player;
    }
}
