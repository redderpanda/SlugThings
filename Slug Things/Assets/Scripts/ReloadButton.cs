using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ReloadButton : NetworkBehaviour {
    public GameObject reload_button;

	// Use this for initialization
	void Start () {
		if(!isServer)
        {
            reload_button.gameObject.SetActive(false);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
