using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class UserGuide : MonoBehaviour {


    public static bool IsOn = false;

    [SerializeField]
    GameObject[] pageOfUserGuide;

    Button pButton;
    Button nButton;

    public int currentPage = 0;


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		for(int i = 0; i < pageOfUserGuide.Length;i++)
        {
            if (i == currentPage)
                pageOfUserGuide[i].SetActive(true);
            else
                pageOfUserGuide[i].SetActive(false);

        }
	}

    public void NextPage()
    {
        if(currentPage != pageOfUserGuide.Length -1)
            currentPage++;
    }

    public void PreviousPage()
    {
        if(currentPage != 0)
            currentPage--;
    }
}
