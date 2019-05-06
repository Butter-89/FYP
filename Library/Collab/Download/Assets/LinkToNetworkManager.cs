using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkToNetworkManager : MonoBehaviour {

	public void CreateRoom()
    {
        GameObject networkManager = GameObject.Find("_NetworkManager");
        networkManager.GetComponent<HostGame>().CreateRoom();
    }
}
