using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class EventManager : NetworkBehaviour {

	public class CustomMsgTypes
	{
		public const short InGameMsg = 1003;
	}
	
	public void RegisterMsgHandles()
	{
		// 注册自定义消息类型
		NetworkServer.RegisterHandler(CustomMsgTypes.InGameMsg, OnRecvCustomMsg);
	}

// 回调方法
	protected void OnRecvCustomMsg(NetworkMessage netMsg)
	{
		Debug.LogWarning("OnRecvCustomMsg");
	}
	// Use this for initialization
	void Start () {
		NetworkServer.Listen(7777);
		Debug.Log("Registering server callbacks");
		NetworkServer.RegisterHandler(MyMessageTypes.MSG_SCORE, OnHit  );
	}
	
	void OnHit(NetworkMessage netMsg)
	{
		Debug.Log("Client connected");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
