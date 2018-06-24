using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Shell_Sc :  NetworkBehaviour{
	[SyncVar]
	public bool occupied;
	[SyncVar]
	public GameObject rider;

	// Use this for initialization
	void Start () {
		occupied = false;
		rider = null;
	}
	
	// Update is called once per frame
	void Update () {
	}
}
