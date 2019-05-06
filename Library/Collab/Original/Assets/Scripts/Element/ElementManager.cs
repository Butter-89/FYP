using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ElementManager : MonoBehaviour {


    public Text text1;
    public Text text2;
    public Text text3;
    public Text buildHintText;
    public Text reloadHintText;


    private string element1;

    public GameObject fireElement;
    public GameObject waterElement;
    public GameObject woodElement;

    public GameObject castle;

    public float spawnTime = 1f;

    public Vector3 size;
    public Transform[] spawnPoints;
    public GameObject[] Elements;

    public string memory;

    private GameObject player;
    private bool _castleBuilt = false;

    private void Start()
    {

        element1 = "None";
    }
    void Awake()
    {
        InvokeRepeating("Spawn", spawnTime, spawnTime);

        player = GameObject.FindGameObjectWithTag(Tags.Player);

    }

    void Update()
    {
        text1.text = "aaa";
    }

    public void AddNewElement(string newElement)
    {

        if (element1 == "None")
        {
            element1 = newElement;
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
        element1 = "None";
        text2.text = "None";
        text3.text = "None";
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
        if ((element1 == "None") || (text2.text == "None") || (text3.text == "None"))
        {
            buildHintText.text = "You don't have enough elements!";
        }
        else
        {
            buildHintText.text = "Construction completed.";
            Vector3 relativePosition = transform.InverseTransformPoint(player.transform.position);
            Vector3 buildCastlePosition = transform.TransformPoint(relativePosition + player.transform.forward * 10);
            Instantiate(castle, buildCastlePosition, Quaternion.identity);
            _castleBuilt = true;
            AbandonAllElement();
        }

    }

    public void Reloading()
    {
        
        if(element1 == "None")
        {
            reloadHintText.text = "You don't have elements for reloading";
        }
        else
        {
            reloadHintText.text = "Reloading completed.";
            player.GetComponentInChildren<Pistol>().increaseAmmo(7);
            Debug.Log("Reload");
            element1 = "None";
        }
    }

    public void SwitchElements()
    {
        memory = element1;
        element1 = text2.text;
        text2.text = text3.text;
        text3.text = memory;
    }

    public bool CastleBuilt()
    {
        return _castleBuilt;
    }
}
