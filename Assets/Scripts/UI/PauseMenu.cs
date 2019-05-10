using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;

public class PauseMenu : MonoBehaviour
{
    public static bool IsOn = false;

    private NetworkManager networkManager;

    [SerializeField]
    GameObject userGuide;

    [SerializeField]
    GameObject techTreeUI;

    [SerializeField]
    GameObject pauseMenu;

    private void Start()
    {
        networkManager = NetworkManager.singleton;
    }

    public void LeaveRoom()
    {
        MatchInfo matchInfo = networkManager.matchInfo;
        networkManager.matchMaker.DropConnection(matchInfo.networkId, matchInfo.nodeId, 0, networkManager.OnDropConnection);
        networkManager.StopHost();
    }

    public void OpenUserGuide()
    {
        userGuide.SetActive(!userGuide.activeSelf);
        UserGuide.IsOn = userGuide.activeSelf;

        pauseMenu.SetActive(false);
        PauseMenu.IsOn = false;
    }

    public void OpenTechTree()
    {
        techTreeUI.SetActive(!techTreeUI.activeSelf);
        TechTreeUI.IsOn = techTreeUI.activeSelf;

        pauseMenu.SetActive(false);
        PauseMenu.IsOn = false;
    }


}
