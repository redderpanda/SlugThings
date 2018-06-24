using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyColor : MonoBehaviour {
    public Color head_color;
    public GameObject head;

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        head_color = head.GetComponent<SpriteRenderer>().color;
        GetComponent<SpriteRenderer>().color = head_color;
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "ColorableTile")
        {
            collision.gameObject.GetComponent<SpriteRenderer>().color = head_color;
        }
    }
}
