using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class PlayerUI : NetworkBehaviour {

    
    private PlayerController playerController;
    private ElementController elementController;
    private Weapon weapon;
    private Player player;

    public Text firstElementUI;
    public Text secondElementUI;
    public Text thirdElementUI;

    public Text informationUI;
    private string information;


    [SerializeField] public List<string> elements;

    [SerializeField]
    GameObject pauseMenu;

    [SerializeField]
    GameObject userGuide;

    [SerializeField]
    GameObject endGame;

    GameObject team1;
    GameObject team2;

    [SerializeField]
    RectTransform HPFill;

    [SerializeField]
    RectTransform AmmoFill;

    private bool endGameFlag = false;


    public void SetPlayer(Player _player)
    {

        player = _player;
        player.gameObject.tag = "Team1";
        playerController = player.GetComponent<PlayerController>();
        elementController = player.GetComponent<ElementController>();
    }


    public void RefreshInformation()
    {
        information = elementController.UIInformation;
        informationUI.text = information;
    }


	// Use this for initialization
	void Start () {
        PauseMenu.IsOn = false;
        EndGame.IsOn = false;

        team1 = GameObject.Find("Team1");
        team2 = GameObject.Find("Team2");
	}
	
	// Update is called once per frame 
	void Update () {
        RefreshUI();
        RefreshInformation();
        SetHPAmount(player.GetCurrentHP());
        if (player != null)
            weapon = player.GetComponentInChildren<Weapon>();
        if(weapon != null)
            SetAmmoAmount(weapon.GetCurrentAmmo());
        if(Input.GetKeyDown("escape"))
        {
            TogglePauseMenu();
        }
        if(player != null)
            EndCheck();
        

    }

    void SetHPAmount (int _amount)
    {
        float amount = _amount;
        HPFill.localScale = new Vector3(1f, amount/100, 1f);
    }

    void SetAmmoAmount (int _amount)
    {
        float amount = _amount;
        AmmoFill.localScale = new Vector3(1f, amount / weapon.ammoCapacity, 1f);
    }

    void EndCheck()
    {
        bool base1Existing = false;
        bool base2Existing = false;
        foreach (var castle in team1.GetComponentsInChildren<Castle>())
        {
            if (castle.isBase)
            {
                base1Existing = true;
            }
        }
        foreach (var castle in team2.GetComponentsInChildren<Castle>())
        {
            if (castle.isBase)
            {
                base2Existing = true;
            }
        }
        
        if(player.isServer)
        {
            
            
            if (!base1Existing && endGameFlag == false) //originally condition1 is: team1.GetComponent<TeamTechnology>().techLevel == -1
            {
                ToggleEndGameLose();
                endGameFlag = true;
            }
            if (!base2Existing && endGameFlag == false) //originally condition1 is: team2.GetComponent<TeamTechnology>().techLevel == -1
            {
                ToggleEndGameWin();
                endGameFlag = true;
            }
        }
        else
        {
            if (!base1Existing && endGameFlag == false)
            {
                ToggleEndGameWin();
                endGameFlag = true;
            }
            if (!base2Existing && endGameFlag == false)
            {
                ToggleEndGameLose();
                endGameFlag = true;
            }
        }
        
    }

    void TogglePauseMenu()
    {
        pauseMenu.SetActive(!pauseMenu.activeSelf);
        PauseMenu.IsOn = pauseMenu.activeSelf;

        if (UserGuide.IsOn)
        {
            userGuide.SetActive(!userGuide.activeSelf);
            UserGuide.IsOn = userGuide.activeSelf;
        }
            

    }

    void ToggleEndGameLose()
    {
        endGame.SetActive(!endGame.activeSelf);
        EndGame.IsOn = endGame.activeSelf;
        EndGame.status = "lose";
    }

    void ToggleEndGameWin()
    {
        endGame.SetActive(!endGame.activeSelf);
        EndGame.IsOn = endGame.activeSelf;
        EndGame.status = "win";
    }

    private void RefreshUI()
    {

        elements = elementController.elements;
        if (elementController == null)
            return;
        if (elements.Count == 0)
        {
            firstElementUI.text = "None";
            secondElementUI.text = "None";
            thirdElementUI.text = "None";
        }
        
        if (elements.Count ==1 &&elements[0] != null)
        {
            firstElementUI.text = elements[0];
        }
        if (elements.Count == 2 && elements[1] != null)
            secondElementUI.text = elements[1];
        if (elements.Count == 3 && elements[2] != null)
            thirdElementUI.text = elements[2];

        if (information != null)
        {
            informationUI.text = information;
        }
    }
}
