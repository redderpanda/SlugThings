using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorSwitch : MonoBehaviour {
	//TODO::Move Most Of This Into Player_Controller In Order To Sync State
    public GameObject Switch;
    public GameObject Wall;
    public bool to_set_active;
	public bool _isActive;
	//public bool _isInActive;

    // Use this for initialization
    void Start() {
        if (to_set_active) {
            Wall.SetActive(false);
			_isActive = false;
        } else {
            Wall.SetActive(true);
			_isActive = true;
        }

    }

    // Update is called once per frame
    void Update() {
		Wall.SetActive (_isActive);

    }

}
