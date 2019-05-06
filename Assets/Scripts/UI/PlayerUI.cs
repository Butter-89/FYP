using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class PlayerUI : NetworkBehaviour {

    
    private PlayerController playerController;
    private ElementController elementController;
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
    GameObject endGame;

    GameObject team1;
    GameObject team2;

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

        if(Input.GetKeyDown("escape"))
        {
            TogglePauseMenu();
        }
        if(player != null)
            EndCheck();
        

    }

    void EndCheck()
    {
        if(player.isServer)
        {
            if (team1.GetComponent<TeamTechnology>().techLevel == -1 && endGameFlag == false)
            {
                ToggleEndGameLose();
                endGameFlag = true;
            }
            if (team2.GetComponent<TeamTechnology>().techLevel == -1 && endGameFlag == false)
            {
                ToggleEndGameWin();
                endGameFlag = true;
            }
        }
        else
        {
            if (team1.GetComponent<TeamTechnology>().techLevel == -1 && endGameFlag == false)
            {
                ToggleEndGameWin();
                endGameFlag = true;
            }
            if (team2.GetComponent<TeamTechnology>().techLevel == -1 && endGameFlag == false)
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
