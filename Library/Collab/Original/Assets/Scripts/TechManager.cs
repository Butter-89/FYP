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
    public GameObject[] weaponCategory;
    public GameObject buildingPool;

    public int teamNo;
    // Use this for initialization
    void Start () {
        techDirection = 0;
        techLevel = 1;
        _currentWeapon = 0;
        UpgradeWeapon();
        maxTechLevel = 3; //temporarily max tech level = 3
        buildingPool = GameObject.FindGameObjectWithTag("Tech");
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
		
		techLevel = 0;
		//Check for tech-level update
		_castles = buildingPool.GetComponentsInChildren<Castle>();
		foreach (Castle castle in _castles)
		{
			if (techLevel<maxTechLevel)
			{
				techLevel+=castle.TechLevel(teamNo);
			}
		}
		UpgradeWeapon();
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

	void UpgradeWeapon()
	{
		weaponCategory[_currentWeapon].gameObject.SetActive(false);
		weaponCategory[techLevel].gameObject.SetActive(true);
		_currentWeapon = techLevel;
	}
	
}
