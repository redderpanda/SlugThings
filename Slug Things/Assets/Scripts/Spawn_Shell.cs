using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Spawn_Shell : NetworkBehaviour {
	public GameObject shell_prefab;

	public override void OnStartServer()
	{
		Spawn ();
	}

	// Use this for initialization
	void Start () {
		//Spawn ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Spawn(){
		GameObject shell = (GameObject)Instantiate(shell_prefab, transform.position, transform.rotation);
		NetworkServer.Spawn (shell);
	}
}
