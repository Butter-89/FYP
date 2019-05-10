using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TestingOnMouseOver : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    Text techTreeInformation;

    [SerializeField]
    string item;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnPointerEnter(PointerEventData eventData)
    {
        switch(item)
        {
            case "WeaponCrate":
                techTreeInformation.text = "Weapon Crate\nFire+Wood+Wood\nUnlock pistol";
                return;
            case "Cargo":
                techTreeInformation.text = "Cargo\nFire+Fire+Fire\nUnlock SMG";
                return;
            case "RocketPack":
                techTreeInformation.text = "Rocket Pack\nFire+Fire+Water\nUnlock rocket launcher";
                return;
            case "WareHouse":
                techTreeInformation.text = "WareHouse\nWater+Wood+Wood\nPrimary defensive technology\nUnlock sandbags";
                return;
            case "Arsenal":
                techTreeInformation.text = "Arsenal\nFire+Water+Wood\nIntermeditate defensive technology\nUnlock turret";
                return;
            case "PowerStation":
                techTreeInformation.text = "Power Station\nFire+Water+Water\nAdvanced defensive technology\n Unlock shiled";
                return;
            case "SandBags":
                techTreeInformation.text = "Sandbags\nWood+Wood+Wood\nPrimary defensive facility";
                return;
            case "Turret":
                techTreeInformation.text = "Turret\nFire+Fire+Wood\nIntermediate defensive facility";
                return;
            case "Shield":
                techTreeInformation.text = "Shield\nWater+Water+Wood\nAdvanced defensive facility";
                return;
            case "Radar":
                techTreeInformation.text = "Radar\nNuclear+Nuclear+Wood\nUnlock ultimate power!";
                return;
            default: return;
        }
            

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        techTreeInformation.text = "";
    }
}
