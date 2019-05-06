using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementOperation : MonoBehaviour {
    public ElementManager elementManager;
    public AudioSource reloading;
    // Use this for initialization
    void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown("5"))
        {
            elementManager.AbandonAllElement();
        }

        if(Input.GetKeyDown("b"))
        {
            elementManager.BuildCastle();
        }

        if(Input.GetKeyDown("r"))
        {
            reloading.Play();
            elementManager.Reloading();
        }

        if (Input.GetKeyDown("4"))
        {
            elementManager.SwitchElements();
        }
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PickUp"))
        {
            other.gameObject.SetActive(false);

            elementManager.AddNewElement(other.gameObject.GetComponent<ElementType>().type);
        }
    }

    void ElementToAmmo()
    {
        
    }
}
