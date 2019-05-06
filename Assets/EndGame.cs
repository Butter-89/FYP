using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;


public class EndGame : MonoBehaviour {

    public static bool IsOn = false;

    public static string status = "";

    [SerializeField]
    Text gameResultText;

    //public float _Timer = 3f;

    private NetworkManager networkManager;

    private void Start()
    {
        networkManager = NetworkManager.singleton;
    }

    private void Update()
    {
        if (status == "win")
            gameResultText.text = "YOU WIN!";

        if (status == "lose")
            gameResultText.text = "YOU LOSE!";
    }


    /*
    private void Update()
    {
        _Timer -= Time.deltaTime;

        if (_Timer < 0)
            _Timer = 0;

        if (_Timer > 0)
            return;

        if (_Timer == 0)
            LeaveRoom();
    }
    */


    public void LeaveRoom()
    {
        MatchInfo matchInfo = networkManager.matchInfo;
        networkManager.matchMaker.DropConnection(matchInfo.networkId, matchInfo.nodeId, 0, networkManager.OnDropConnection);
        networkManager.StopHost();
    }
}
