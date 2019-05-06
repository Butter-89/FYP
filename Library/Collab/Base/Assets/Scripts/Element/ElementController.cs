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


    
    [SerializeField] private int techLevel;
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
    [SerializeField] public List<string> elements;
    // Use this for initialization
    void Start () {
        elements.Clear();
        _indicator = 0;
        maxElement = 3;
        buildRange = 999f;
        player = gameObject;
        techLevel = 0;
        //buildingPool = GameObject.Find(player.name+"'s pool");
    }
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetKeyDown("b"))
		{
			//Debug.Log("Build a castle");
			CmdBuildCastle();
			//perform building function here
		}

		else if (Input.GetKeyDown("v"))
		{
			//abandon the first element
			//_woodNumber = 3;
			ShiftElement();
			elements.RemoveAt(elements.Count-1);
            Debug.Log("Current element cound: " + elements.Count);
		}

		else if (Input.GetKeyDown("4"))
		{
			//shift the elements by 1
			ShiftElement();
		}
		else if (Input.GetKeyDown("r"))
		{
			//Consume the first element and reload
			if (elements.Count >= 1)
			{
				//Debug.Log("Reloading element:"+elements.Count);
				ShiftElement();
				elements.RemoveAt(elements.Count-1);
				transform.GetComponentInChildren<Weapon>().Reload();
			}
			
		}
        else if (Input.GetKeyDown("t"))
        {
            Debug.Log(buildingPool.name);
            if (buildingPool == null)
            {
                Debug.Log("No building pool");
                return;
            }
                
            techLevel = buildingPool.GetComponent<TechTest>().GetTechLevel();
            Debug.Log(buildingPool.name + techLevel);
            UpgradeWeapon();
            //CmdDebugTest();
        }


    }
    

    //technology tree
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
                var elementType = other.GetComponent<ElementType>().type;
                elements.Add(elementType);
                //Debug.Log(elements[elements.Count-1]);
                _indicator++;
            }
        }
		
	}

    [Command]
	private void CmdBuildCastle() //tech tree here?
	{
		
		for (int i = 0; i < elements.Count; i++)
		{
			//decide which type of castle the elements can build
			if (elements[i] == "Wood")
				_woodNumber++;
			else if (elements[i] == "Water")
				_waterNumber++;
			else if (elements[i] == "Fire")
			{
				_fireNumber++;
			}
			
		}
		Vector3 origin = cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0f));
		Vector3 direction = cam.transform.forward;
		// The ray that will be used for this shot
		Ray ray = new Ray(origin, direction);
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit, buildRange))
		{
            
            Vector3 position = player.transform.position + player.transform.forward * 10;
            //Vector3 position = hit.point;
            //Debug.Log(_woodNumber);

            //Here is the tech-combination

            Debug.Log(buildingPool.name + " builds");
            if (_woodNumber==3)
			{
                NetworkServer.Spawn(Instantiate(basicBuilding, position, Quaternion.identity, buildingPool.transform));
                //NetworkServer.Spawn(Instantiate(basicBuilding, position, Quaternion.identity, buildingPool.transform));
                //Instantiate(basicBuilding, position, Quaternion.identity, buildingPool.transform);
                elements.Clear();
				_woodNumber = 0;
				_indicator = 0;
			}

			if (_waterNumber == 3)
			{
				NetworkServer.Spawn(Instantiate(distillery, position, Quaternion.identity,buildingPool.transform));
				elements.Clear();
				_waterNumber = 0;
				_indicator = 0;
			}

			if (_fireNumber == 3)
			{
				NetworkServer.Spawn(Instantiate(smelter, position, Quaternion.identity,buildingPool.transform));
				elements.Clear();
				_fireNumber = 0;
				_indicator = 0;
			}
			//Above: need refinement when there are more element types
		}

		_fireNumber = 0;
		_woodNumber = 0;
		_waterNumber = 0;
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


