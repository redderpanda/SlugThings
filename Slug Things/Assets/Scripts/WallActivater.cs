using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallActivater : MonoBehaviour {
    public GameObject wall;
	// Use this for initialization
	void Start () {
        wall.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            wall.SetActive(true);
        }
    }
}
