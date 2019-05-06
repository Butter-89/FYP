using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TechManager : NetworkBehaviour {
    [SerializeField] private int techLevel;
    [SerializeField] private int techDirection;
    [SerializeField] private int maxTechLevel;
    private int _currentWeapon;


    private Castle[] _castles;
    private Player _player;
    public GameObject[] weaponCategory;
    private GameObject buildingPool;

    public int teamNo;
    // Use this for initialization
    void Start () {
        techDirection = 0;
        techLevel = 0;
        _currentWeapon = 0;
        CmdCheckLevel();
        maxTechLevel = 3; //temporarily max tech level = 3
        buildingPool = GameObject.Find("Team"+teamNo);
        //Debug.Log("Team"+teamNo);

        
    }

    private void Awake()
    {
	    
    }
    // Update is called once per frame
	
	void Update () {
		if (Input.GetKeyDown("t"))
		{
			CmdCheckLevel();
			//CmdDebugTest();
		}
	}

	[Command]
	private void CmdCheckLevel()
	{
		_player = GetComponent<Player>();
		buildingPool = GameObject.Find("Team"+teamNo);
		if(buildingPool==null)
			Debug.Log("Building pool not fuound!");
		techLevel = -1;
		//Check for tech-level update
		_castles = buildingPool.GetComponentsInChildren<Castle>();
		foreach (Castle castle in _castles)
		{
			if (techLevel<maxTechLevel)
			{
				techLevel+=castle.TechLevel();
			}
		}
		//Debug.Log(buildingPool.transform.name+"'s techLevel= "+techLevel);
		CmdUpgradeWeapon(techLevel);
		_player.CmdUpdateWeapon(techLevel);
	}
	

	[Command]
	private void CmdDebugTest()
	{
		Debug.Log("Client commands");
	}
	
	public void InitializeWeapon()
	{
		//Debug.Log("Tech-level is:" + techLevel);
		weaponCategory[0].gameObject.SetActive(true);
	}

	[Command]
	private void CmdUpgradeWeapon(int techLevel)
	{
		RpcUpgradeWeapon(techLevel);
	}
	
	[ClientRpc]
	private void RpcUpgradeWeapon(int techLevel)
	{
		_player = GetComponent<Player>();
		//Debug.Log("Active weapon: "+_player.activeWeapon);
		_player.activeWeapon = techLevel;
		weaponCategory[_currentWeapon].gameObject.SetActive(false);
		weaponCategory[techLevel].gameObject.SetActive(true);
		//Debug.Log("Techlevel: "+techLevel);
		_currentWeapon = techLevel;
	}

}
