using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ElementManager : MonoBehaviour {

// UI part
    public Text text1;
    public Text text2;
    public Text text3;
    public Text buildHintText;
    public Text reloadHintText;

    public int elementValue;
//Game Object
    public GameObject fireElement;
    public GameObject waterElement;
    public GameObject woodElement;

    public GameObject basicBuilding;
    public GameObject distillery;

//Technology parameters
    public bool basicBuildingBuilt = false;
    public bool distilleryBuilt = false;


    public float spawnTime = 1f;

    public Vector3 size;
    public Transform[] spawnPoints;
    public GameObject[] Elements;

    public string memory;

    private GameObject player;
    private bool _castleBuilt = false;

    private void Start()
    {

        //element1 = "None";
    }
    void Awake()
    {
        InvokeRepeating("Spawn", spawnTime, spawnTime);

        player = GameObject.FindGameObjectWithTag(Tags.Player);

    }

    void Update()
    { }
    

    public void AddNewElement(string newElement)
    {

        if (text1.text == "None")
        {
            text1.text = newElement;
        }
        else if (text2.text == "None")
        {
            text2.text = newElement;
        }
        else if (text3.text == "None")
        {
            text3.text = newElement;
        }


    }

    public void AbandonAllElement()
    {
        text1.text = "Non";
        text2.text = "None";
        text3.text = "None";
    }

    public void elementPair(string elementType1, string elementType2, string elementType3)
    {
        elementValue = 0;
        switch (elementType1)
        {
            case "None" :elementValue += 0;
                break;
            case "Wood": elementValue += 1;
                break;
            case "Water": elementValue += 4;
                break;
            case "Fire": elementValue += 13;
                break;
        }
        switch (elementType2)
        {
            case "None":
                elementValue += 0;
                break;
            case "Wood":
                elementValue += 1;
                break;
            case "Water":
                elementValue += 4;
                break;
            case "Fire":
                elementValue += 13;
                break;
        }
        switch (elementType3)
        {
            case "None":
                elementValue += 0;
                break;
            case "Wood":
                elementValue += 1;
                break;
            case "Water":
                elementValue += 4;
                break;
            case "Fire":
                elementValue += 13;
                break;
        }
    }

    

    public void Spawn()
    {
        /*
        int spawnPointIndex1 = Random.Range(0, spawnPoints.Length);
        int spawnPointIndex2 = Random.Range(0, spawnPoints.Length);
        int spawnPointIndex3 = Random.Range(0, spawnPoints.Length);

        
        Instantiate(fireElement, spawnPoints[spawnPointIndex1].position, spawnPoints[spawnPointIndex1].rotation);
        Instantiate(waterElement, spawnPoints[spawnPointIndex2].position, spawnPoints[spawnPointIndex2].rotation);
        Instantiate(woodElement, spawnPoints[spawnPointIndex3].position, spawnPoints[spawnPointIndex3].rotation);

    */


        /*
         * Vector3 firePos = spawnPoints[0].position + new Vector3(Random.Range(-size.x / 2, size.x / 2), 0, Random.Range(-size.z / 2, size.z / 2));
        Instantiate(fireElement, firePos, Quaternion.identity);

        Vector3 waterPos = spawnPoints[1].position + new Vector3(Random.Range(-size.x / 2, size.x / 2), 0, Random.Range(-size.z / 2, size.z / 2));
        Instantiate(waterElement, waterPos, Quaternion.identity);

        Vector3 woodPos = spawnPoints[2].position + new Vector3(Random.Range(-size.x / 2, size.x / 2), 0, Random.Range(-size.z / 2, size.z / 2));
        Instantiate(woodElement, woodPos, Quaternion.identity);
        */
        for(int i=0;i< spawnPoints.Length;i++)
        {
            Instantiate(Elements[i], spawnPoints[i].position + new Vector3(Random.Range(-size.x / 2, size.x / 2), 0, Random.Range(-size.z / 2, size.z / 2)), Quaternion.identity);
        }
        
    }



    public void BuildCastle()
    {
        if ((text1.text == "None") || (text2.text == "None") || (text3.text == "None"))
        {
            buildHintText.text = "You don't have enough elements!";
        }
        else
        {
            elementPair(text1.text, text2.text, text3.text);
            buildHintText.text = elementValue.ToString();
            Vector3 relativePosition = transform.InverseTransformPoint(player.transform.position);
            Vector3 buildCastlePosition = transform.TransformPoint(relativePosition + player.transform.forward * 10);
            switch(elementValue)
            {
                case 3: Instantiate(basicBuilding, buildCastlePosition, Quaternion.identity);
                    basicBuildingBuilt = true;
                    break;
                case 12:
                    if (basicBuildingBuilt == false)
                    {
                        buildHintText.text = "pre-requiste required! Please check technology tree!";
                    }
                    else
                    {
                        Instantiate(distillery, buildCastlePosition, Quaternion.identity);
                        distilleryBuilt = true;
                    }
                    break;
                            
                default:
                    buildHintText.text = "Incorrect combination! Please check the technology tree!";
                    break;
            }
            
            _castleBuilt = true;
            AbandonAllElement();
        }

    }

    public void Reloading()
    {
        
        if(text1.text == "None")
        {
            //reloadHintText.text = "You don't have elements for reloading";
        }
        else
        {
            //reloadHintText.text = "Reloading completed.";
            player.GetComponentInChildren<Pistol>().increaseAmmo(7);
            Debug.Log("Reload");

        }
    }

    public void SwitchElements()
    {
        memory = text1.text;
        text1.text = text2.text;
        text2.text = text3.text;
        text3.text = memory;
    }

    public bool CastleBuilt()
    {
        return _castleBuilt;
    }
}
