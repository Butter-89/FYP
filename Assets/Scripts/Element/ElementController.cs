//using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

//Now this scrip mainly controls the building functions. The TechManager controls the player's weapons.

[System.Serializable]
public class TechTree
{
    public List<GameObject> defensiveTech = new List<GameObject>();
    public List<GameObject> aggressiveTech = new List<GameObject>();
    public List<GameObject> defensiveBuildings = new List<GameObject>();
    public GameObject finalTech;

    public int maxDTech;
    public int maxATech;
    

}

[System.Serializable]
public class TechBuilding
{
    //public string techType; "A" for aggressive; "D" for defensive
    public int techNumber; //used to mark the corresponding prefab of the building
    public int buildingAmount; //to show how many of this building are in the game
    public int woodNeeded;
    public int waterNeeded;
    public int fireNeeded;
}

public class ElementController : NetworkBehaviour
{
    public TechTree techTree;
    public TechBuilding[] dBuildingList;
    public TechBuilding[] aBuildingList;
    public TechBuilding[] dBuildings;
    private Castle[] _castles;
    
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
    public System.Collections.Generic.List<string> elements;

    public GameObject[] elementTypes; //1 - wood; 2 - water; 3 - fire

    private bool flag = true;

    private string _buildingType = "";
    [SerializeField] private int teamNo;
    private NetworkIdentity parentIdentity;
    private PlayerMotor playerMotor;
    

    // Use this for initialization
    void Start () {
        elements.Clear();
        _indicator = 0;
        maxElement = 3;
        buildRange = 999f;
        player = gameObject;
        techLevel = 0;
        playerMotor = GetComponent<PlayerMotor>();
        teamNo = GetComponent<TechManager>().teamNo;
        buildingPool = GameObject.Find("Team"+teamNo); //buildingPool is now Team#
        
        //Debug.Log(buildingPool.transform.name);
        
        //Debug.Log(transform.name+" "+buildingPool.transform.name);
        

    }

    // Update is called once per frame
    void Update ()
	{
       if(isLocalPlayer)
        {
            if (Input.GetKeyDown("b") && playerMotor.IsGrounded())
            {
                //Debug.Log("Build a castle");
                /* //if(player.transform.position.y<15.1&&player.transform.position.y>15) need improvement
                {
                    UIInformation = "Hint1";
                    BuildCastle();
                }
                else
                {
                    UIInformation = "Please build on your base!";
                }*/
                BuildCastle();

                //perform building function here
            }

            else if (Input.GetKeyDown("v") && elements.Count>0 )
            {
                //abandon the first element
                //_woodNumber = 3;
                if (elements[elements.Count-1]=="Wood")
                {
                    CmdDropElement(0);
                }
                else if (elements[elements.Count-1]=="Water")
                {
                    CmdDropElement(1);
                }
                else if (elements[elements.Count-1]=="Fire")
                {
                    CmdDropElement(2);
                }
                //ShiftElement();
                elements.RemoveAt(elements.Count - 1);
                Debug.Log("Current elements: " + elements.ToString());
            }
            
            else if (Input.GetKeyDown(KeyCode.C))
            {
                CmdDropAllElements();
                //Debug.Log("Current element number: "+elements.Count);
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
                    //ShiftElement();
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

    private void FixedUpdate()
    {
        UpdateTechBuildingAmount();
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
        string techType = "";
        int buildingNumber = 0; //mark which building in the list is to be built

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

        int maxALevel = GetMaxTechPoints("A");
        int maxDLevel = GetMaxTechPoints("D");

        foreach (var option in aBuildingList)
        {
            if (option.woodNeeded == _woodNumber&&option.waterNeeded==_waterNumber&&option.fireNeeded==_fireNumber)
            {
                //Debug.Log("Combination found");
                if (option.techNumber <= maxALevel)
                {
                    techType = "A";
                    buildingNumber = option.techNumber;
                }
            } 
        }
        
        foreach (var option in dBuildingList)
        {
            if (option.woodNeeded == _woodNumber&&option.waterNeeded==_waterNumber&&option.fireNeeded==_fireNumber)
            {
                //Debug.Log("Combination found");
                if (option.techNumber <= maxDLevel)
                {
                    techType = "D";
                    buildingNumber = option.techNumber;
                }
            } 
        }

        foreach (var option in dBuildings)
        {
            if (option.woodNeeded == _woodNumber&&option.waterNeeded==_waterNumber&&option.fireNeeded==_fireNumber)
            {
                if (dBuildingList[option.techNumber].buildingAmount>=1)
                {
                    techType = "B";
                    buildingNumber = option.techNumber;
                }
            } 
        }
        //Debug.Log("Tech Type: "+techType+" Building number: "+buildingNumber+" Max ALevel: "+maxALevel+" Max DLevel"+maxDLevel);

        if (techType != "")
        {
            //Debug.Log("Building...");
            Vector3 position = player.transform.position + player.transform.forward * 10;
            CmdBuildCastle(techType, buildingNumber, position);
            elements.Clear();
            CounterToZero();
            if (techType == "A")
            {
                aBuildingList[buildingNumber].buildingAmount++;
            }
            else if (techType == "D")
            {
                dBuildingList[buildingNumber].buildingAmount++;
            }
            else if (techType == "B")
            {
                dBuildings[buildingNumber].buildingAmount++;
            }
        }
        
        
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
	private void CmdBuildCastle(string typeDecision, int buildingNumber, Vector3 position)
	{
        //RpcBuildCastle(cPlayer,decision);
       parentIdentity = buildingPool.GetComponent<NetworkIdentity>();
        //Vector3 position = cPlayer.transform.position + cPlayer.transform.forward * 10;
        GameObject building = null;
        if (typeDecision == "A")
        {
            building = Instantiate(techTree.aggressiveTech[buildingNumber], position, Quaternion.identity, buildingPool.transform);
            NetworkServer.Spawn(building);
            building.GetComponent<Castle>().teamNo = teamNo;
            elements.Clear();
            CounterToZero();
            CmdSetParent(parentIdentity,building);
            
        }
        if (typeDecision == "D")
        {
            
            building = Instantiate(techTree.defensiveTech[buildingNumber], position, Quaternion.identity, buildingPool.transform);
            NetworkServer.Spawn(building);
            building.GetComponent<Castle>().teamNo = teamNo;
            elements.Clear();
            CounterToZero();
            CmdSetParent(parentIdentity,building);
        }
        if (typeDecision == "B")
        {
            
            building = Instantiate(techTree.defensiveBuildings[buildingNumber], position, Quaternion.identity, buildingPool.transform);

            NetworkServer.Spawn(building);
            if (building.GetComponent<Castle>())
            {
                building.GetComponent<Castle>().teamNo = teamNo;
            }

            if (building.GetComponentInChildren<Turret>())
            {
                //int team = 0;
                if (gameObject.CompareTag("Team1"))
                {
                    teamNo = 1;
                }
                else
                {
                    teamNo = 2;
                }
                Debug.Log("Turret script found, player teamNo: "+teamNo);
                CmdSetTeam(building,teamNo);
                    
                //building.GetComponentInChildren<Turret>().teamNo = teamNo;
                
                //building.GetComponentInChildren<Turret>().UpdateTargetTag();
            }
            else
            {
                Debug.Log("Turret script NOT found");
            }
            
            elements.Clear();
            CounterToZero();
            CmdSetParent(parentIdentity,building);
        }
        /*
        switch(typeDecision)
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
        */
        //Above: need refinement when there are more element types
        
	
	}

    [Command]
    public void CmdDropElement(int type)
    {
        
        Vector3 position = player.transform.position + 
                           player.transform.forward * 5 + player.transform.up * 2 + 
                           new Vector3(Random.Range(-5 / 2, 5 / 2), 0, Random.Range(-5 / 2, 5 / 2));
        var droppedElement = Instantiate(elementTypes[type], position, Quaternion.identity);
        NetworkServer.Spawn(droppedElement);
    }

    [Command]
    public void CmdDropAllElements()
    {
        foreach (var element in elements)
        {
            if (element=="Wood")
            {
                CmdDropElement(0);
            }
            else if (element=="Water")
            {
                CmdDropElement(1);
            }
            else if (element=="Fire")
            {
                CmdDropElement(2);
            }
        }
        elements.Clear();
        CounterToZero();
    }
    
    [Command]
    private void CmdSetTeam(GameObject building, int team)
    {
        if (building.GetComponent<NetworkIdentity>())  //for turret only
        {
            Debug.Log("Turret NetId found");
            RpcSetTeam(building.GetComponent<NetworkIdentity>().netId, team);
        }
    }

    [ClientRpc]
    private void RpcSetTeam(NetworkInstanceId buildingNetId, int team)
    {
        if (buildingNetId != NetworkInstanceId.Invalid)
        {
            Debug.Log("NetID not null");
            var targetBuilding = ClientScene.FindLocalObject(buildingNetId);
            if (targetBuilding != null)
            {
                Debug.Log("Target building found: "+targetBuilding.transform.name);
                if (targetBuilding.GetComponent<Castle>())
                {
                    Debug.Log("Target building: castle");
                    targetBuilding.GetComponent<Castle>().teamNo = team;
                }
                else if (targetBuilding.GetComponentInChildren<Turret>())
                {
                    Debug.Log("Target building: turret head, team"+team);
                    targetBuilding.GetComponentInChildren<Turret>().teamNo = team;
                    targetBuilding.GetComponentInChildren<Turret>().UpdateTargetTag();
                }
            }
        }
    }

    [Command]
    private void CmdSetParent(NetworkIdentity parentNetworkIdentity, GameObject building)
    {
        //this.parentNetworkIdentity = buildingPool.GetComponent<NetworkIdentity>();
        //Debug.Log(this.parentIdentity);
        if (parentNetworkIdentity != null) {
            //Debug.Log("BuildingPool netID found");
            // Set locally on server
            building.transform.SetParent (parentNetworkIdentity.transform);
            // Set remotely on clients
            RpcSetParent (parentNetworkIdentity.netId,building);
        }
        else {
            Debug.Log("BuildingPool netID NOT found");
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
            //Debug.Log("Parent netID is not null");
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
        System.Collections.Generic.List<string> shifted = new System.Collections.Generic.List<string>();
		for (int i = 1; i < elements.Count; i++)
		{
			shifted.Add(elements[i]);
		}
		shifted.Add(elements[0]);
		elements = shifted;
	}

    private int GetMaxTechPoints(string branch)
    {
        int level = 0;
        foreach (var castle in buildingPool.GetComponentsInChildren<Castle>())
        {
            if (castle.TechLevel(branch) >= level)
            {
                level = castle.TechLevel(branch);
            }
        }

        return level;
    }

    public void UpdateTechBuildingAmount()
    {
        foreach (var option in dBuildingList)
        {
            option.buildingAmount = 0;
        }
        foreach (var option in aBuildingList)
        {
            option.buildingAmount = 0;
        }
        
        
        _castles = buildingPool.GetComponentsInChildren<Castle>();
        foreach (Castle castle in _castles)
        {
            if (castle.TechLevel("D")>0)
            {
                dBuildingList[castle.TechLevel("D")-1].buildingAmount++;
            }
            else if (castle.TechLevel("A")>0)
            {
                aBuildingList[castle.TechLevel("A")-1].buildingAmount++;
            }
        }
    }
}


