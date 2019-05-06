using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class ElementController : NetworkBehaviour {
    public float[] testing;
    public Text firstElementUI;
    public Text secondElementUI;
    public Text thirdElementUI;

    public GameObject elementPickedParticleSystem;
    
    [SerializeField] public int techLevel;
    [SerializeField] private int techDirection;
    [SerializeField] private int maxTechLevel;
    private int _currentWeapon;
    public GameObject[] weaponCategory;

    GameObject TechPool;


    //The building options
    public GameObject basicBuilding;
    public GameObject distillery;
    public GameObject smelter;

    public GameObject buildingPool;

    private GameObject player;

    public Camera cam;
    private int _indicator;
    
    //Types of elements here
    private int _woodNumber = 0;
    private int _waterNumber = 0;
    private int _fireNumber = 0;
    
    [SerializeField] private int maxElement;
    [SerializeField] private float buildRange;
    [SerializeField]
    public List<string> elements;

    public string UIInformation;

    private bool flag = true;

    private string _buildingType = "";
    private int teamNo;
    private NetworkIdentity parentIdentity;

    // Use this for initialization
    void Start () {
        elements.Clear();
        _indicator = 0;
        maxElement = 3;
        buildRange = 999f;
        player = gameObject;
        techLevel = 0;
        teamNo = GetComponent<TechManager>().teamNo;
        buildingPool = GameObject.Find("Team"+teamNo); //buildingPool is now Team#
        parentIdentity = buildingPool.GetComponent<NetworkIdentity>();
        
        //Debug.Log(transform.name+" "+buildingPool.transform.name);
        if (isServer)
        {
            UIInformation = "Is server";
        }
        if (isClient)
        {
            UIInformation += " Is client";
        }

    }

    // Update is called once per frame
    void Update ()
	{
       if(isLocalPlayer)
        {
            if (Input.GetKeyDown("b"))
            {
                //Debug.Log("Build a castle");
                if(player.transform.position.y<15.1&&player.transform.position.y>15)
                {
                    UIInformation = "Hint1";
                    BuildCastle();
                }
                else
                {
                    UIInformation = "Please build on your base!";
                }
                

                //perform building function here
            }

            else if (Input.GetKeyDown("v"))
            {
                //abandon the first element
                //_woodNumber = 3;
                ShiftElement();
                elements.RemoveAt(elements.Count - 1);
                Debug.Log("Current element cound: " + elements.Count);
            }

            else if (Input.GetKeyDown("4"))
            {
                //shift the elements by 1
                //ShiftElement();
                CmdDebugTest();
            }
            else if (Input.GetKeyDown("r"))
            {
                //Consume the first element and reload
                if (elements.Count >= 1)
                {
                    //Debug.Log("Reloading element:"+elements.Count);
                    ShiftElement();
                    elements.RemoveAt(elements.Count - 1);
                    transform.GetComponentInChildren<Weapon>().Reload();
                }

            }
            else if (Input.GetKeyDown("t"))
            {
                //UpgradeWeapon();
                //CmdDebugTest();
            }

            //UpdateTechLevelFromPoolToPlayer();
        }
        

        
        /*
        if(flag)
        {
            UIInformation = buildingPool.name;
            flag = false;
        }
        */
    }

    

    public void SetUIInformation(string _information)
    {
        UIInformation = _information;
    }


    private void UpdateTechLevelFromPoolToPlayer()
    {
        if(buildingPool != null)
            techLevel = buildingPool.GetComponent<TeamTechnology>().techLevel;
        Debug.Log(buildingPool.name+" Tech-level: "+ techLevel);
    }

    //technology tree
    public void InitializeWeapon()
    {
        //Debug.Log("Tech-level is:" + techLevel);
        weaponCategory[0].gameObject.SetActive(true);
    }


    void UpgradeWeapon()
    {
        //CmdDebugTest();
        weaponCategory[_currentWeapon].gameObject.SetActive(false);
        weaponCategory[techLevel].gameObject.SetActive(true);
        _currentWeapon = techLevel;
    }
    /// ////////////////////////////////////////////////
    /// 


    void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("PickUp"))
		{
            
            if (elements.Count <= maxElement - 1)
            {
                //Debug.Log(elements.Count);
                //other.gameObject.SetActive(false);
                NetworkServer.Destroy(other.gameObject);
                //NetworkServer.RegisterHandler();
                var elementType = other.GetComponent<ElementType>().type;
                elements.Add(elementType);
                //Debug.Log(elements[elements.Count-1]);
                _indicator++;
            }
        }
		
	}

    [Command]
    void CmdDebugTest()
    {
	    Debug.Log("going through UpgradeWeapon()");
    }
    
    private void BuildCastle()
    {
        

        for(int i = 0; i < elements.Count; i++)
		{
            //decide which type of castle the elements can build
            if (elements[i] == "Wood")
                _woodNumber++;
            else if (elements[i] == "Water")
                _waterNumber++;
            else if (elements[i] == "Fire")
                _fireNumber++;
        }

        if (_woodNumber == 3)
            _buildingType = "basicBuilding";

        if (_fireNumber == 3)
            _buildingType = "smelter";

        if (_waterNumber == 3)
            _buildingType = "distillery";
        
        CmdBuildCastle(player,_buildingType);
        elements.Clear();
        CounterToZero();
        
    }

    private void CounterToZero()
    {
        _fireNumber = 0;
        _woodNumber = 0;
        _waterNumber = 0;
        _indicator = 0;
        _buildingType = "";
    }

    [Command]
	private void CmdBuildCastle(GameObject cPlayer, string decision)
	{
        //RpcBuildCastle(cPlayer,decision);
       
        Vector3 position = cPlayer.transform.position + cPlayer.transform.forward * 10;
        GameObject building = null;
        switch(decision)
        {
            
            //may need modification to fix the problem of tech tree
            case "basicBuilding" :
                building = Instantiate(basicBuilding, position, Quaternion.identity, buildingPool.transform);
                building.GetComponent<Castle>().teamNo = teamNo;
                NetworkServer.Spawn(building);
                elements.Clear();
                CounterToZero();
                CmdSetParent(parentIdentity,building);
                return;
            case "smelter":
                building = Instantiate(smelter, position, Quaternion.identity, buildingPool.transform);
                building.GetComponent<Castle>().teamNo = teamNo;
                NetworkServer.Spawn(building);
                elements.Clear();
                CounterToZero();
                CmdSetParent(parentIdentity,building);
                return;
            case "distillery":
                building = Instantiate(distillery, position, Quaternion.identity, buildingPool.transform);
                building.GetComponent<Castle>().teamNo = teamNo;
                NetworkServer.Spawn(building);
                elements.Clear();
                CounterToZero();
                CmdSetParent(parentIdentity,building);
                return;
            default:
                return;

        }
        
        //Above: need refinement when there are more element types
        
	
	}

    [Command]
    private void CmdSetParent(NetworkIdentity parentNetworkIdentity, GameObject building)
    {
        if (parentNetworkIdentity != null) {
            // Set locally on server
            transform.SetParent (parentNetworkIdentity.transform);
            // Set remotely on clients
            RpcSetParent (parentNetworkIdentity.netId,building);
        }
        else {
            // Set locally on server
            transform.SetParent (null);
            // Set remotely on clients
            RpcSetParent (NetworkInstanceId.Invalid,building);
        }
    }

    [ClientRpc]
    private void RpcSetParent(NetworkInstanceId newParentNetId,GameObject building)
    {
        Transform parentTransform = null;
        if (newParentNetId != NetworkInstanceId.Invalid) {
            // Find the parent by netid and set self as child
            var parentGobj = ClientScene.FindLocalObject (newParentNetId);
            if (parentGobj != null) {
                parentTransform = parentGobj.transform;
            }
            else {
                Debug.LogWarningFormat ("{0} Could not find NetworkIdentity '{1}'.", gameObject.name, newParentNetId.Value);
            }
        }
        building.transform.SetParent (parentTransform);
    }

    [ClientRpc]
    private void RpcUpdateTeamTech()
    {
        var teamTech = GameObject.Find("Team" + GetComponent<TechManager>().teamNo).GetComponent<TeamTechnology>();
        teamTech.UpdateTechLevel();
    }
    
	private void ShiftElement()
	{
		List<string> shifted = new List<string>();
		for (int i = 1; i < elements.Count; i++)
		{
			shifted.Add(elements[i]);
		}
		shifted.Add(elements[0]);
		elements = shifted;
	}

}


