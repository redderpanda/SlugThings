using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTest : MonoBehaviour {
	private Rigidbody2D rigidB;
	public float move_speed = 2f;
	// Use this for initialization
	void Start () {
		rigidB = transform.GetComponent<Rigidbody2D>();
	}

	// Update is called once per frame
	void Update () {

	}

	void FixedUpdate(){
		float H_move = Input.GetAxis ("Horizontal 2");
		float V_move = Input.GetAxis ("Vertical 2");

		float movement_H = H_move * move_speed;
		float movement_V = V_move * move_speed;

		rigidB.velocity = new Vector2(movement_H,movement_V);
	}
}
