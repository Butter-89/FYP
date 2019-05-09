using System.Collections;
using System.Collections.Generic;
using Boo.Lang;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Serialization;

public class TechLock
{
	List techLock = new List();
}

public class TechManager : NetworkBehaviour {
    [SerializeField] private int techLevel;
    [SerializeField] private int techDirection;
    [FormerlySerializedAs("maxATechLevel")] [SerializeField] private int maxTechLevel; //indicate the max A level
    private int _bestWeaponLevel;
    private int _currentWeapon;

    private Castle[] _castles;
    private Player _player;
    public GameObject[] weaponCategory;
    public GameObject throwingPoint;
    private GameObject buildingPool;


    public int teamNo;
    // Use this for initialization
    void Start () {
        techDirection = 0;
        techLevel = 0;  //Now that this script controls the weapon, the tech level here is A-Tech Level
        _bestWeaponLevel = 0;
        _currentWeapon = 0;
        CmdCheckLevel();
        maxTechLevel = 3; //temporarily max tech level = 3
        buildingPool = GameObject.Find("Team"+teamNo);
        //Debug.Log("Team"+teamNo);

        
    }

    // Update is called once per frame
	
	void Update () {  //change to update !!
		//Change weapon by pressing the alpha number keys
		if (Input.GetKeyDown(KeyCode.Alpha1)&&_bestWeaponLevel>=0)
		{
			CmdChangeWeapon(1);
		}
		if (Input.GetKeyDown(KeyCode.Alpha2)&&_bestWeaponLevel>=1)
		{
			CmdChangeWeapon(2);
		}
		if (Input.GetKeyDown(KeyCode.Alpha3)&&_bestWeaponLevel>=2)
		{
			CmdChangeWeapon(3);
		}
		if (Input.GetKeyDown(KeyCode.Alpha4)&&_bestWeaponLevel>=3)
		{
			CmdChangeWeapon(4);
		}

		if (Input.GetKeyDown(KeyCode.G))
		{
			Instantiate(weaponCategory[4], throwingPoint.transform.position, Quaternion.identity);
		}
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
		techLevel = 0;
		//Check for tech-level update
		_castles = buildingPool.GetComponentsInChildren<Castle>();
		foreach (Castle castle in _castles)
		{
			if (techLevel<=castle.TechLevel("A"))
			{
				techLevel=castle.TechLevel("A");
			}
		}
		
		//Debug.Log(buildingPool.transform.name+"'s techLevel= "+techLevel);
		//Debug.Log("Max weapon level: "+techLevel);
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
	public void CmdUpgradeWeapon(int techLevel)
	{
		RpcUpgradeWeapon(techLevel);
	}
	
	[ClientRpc]
	private void RpcUpgradeWeapon(int techLevel)
	{
		_player = GetComponent<Player>();
		weaponCategory[_currentWeapon].gameObject.SetActive(false);

		if (_bestWeaponLevel<=techLevel)
		{
			_bestWeaponLevel = techLevel;
		}
		
		weaponCategory[_bestWeaponLevel].gameObject.SetActive(true);
		
		_currentWeapon = _bestWeaponLevel;
		_player.activeWeapon = _currentWeapon;
	}

	[Command]
	public void CmdChangeWeapon(int weaponNo)
	{
		RpcChangeWeapon(weaponNo);
	}

	[ClientRpc]
	private void RpcChangeWeapon(int weaponNo)
	{
		_player = GetComponent<Player>();
		weaponCategory[_currentWeapon].gameObject.SetActive(false);

		_currentWeapon = weaponNo-1;
		weaponCategory[_currentWeapon].gameObject.SetActive(true);
		_player.activeWeapon = _currentWeapon;
	}

}
