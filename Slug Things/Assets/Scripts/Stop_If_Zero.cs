using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stop_If_Zero : MonoBehaviour {
	public GameObject Main_Segment;
	private bool stop_seg;
	private Rigidbody2D Main_Segment_RB;
	private Rigidbody2D This_Seg_RB;
	private Player_Controller main_controller;
	// Use this for initialization
	void Start () {
		Main_Segment_RB = Main_Segment.GetComponent<Rigidbody2D>();
		This_Seg_RB = transform.GetComponent<Rigidbody2D> ();
		main_controller = Main_Segment.GetComponent<Player_Controller> ();

	}
	
	// Update is called once per frame
	void Update () {
		//Debug.Log (Main_Segment_RB.velocity.x);
//		if (Main_Segment_RB.velocity.x < 0.3f && Main_Segment_RB.velocity.x > -0.3f ) {
//			This_Seg_RB.velocity = new Vector2(0f,This_Seg_RB.velocity.y);
//		}
//
		if(main_controller.grounded){
			if (Mathf.Abs (Main_Segment_RB.velocity.x) < Mathf.Abs (This_Seg_RB.velocity.x)) {
				This_Seg_RB.velocity = new Vector2 (Main_Segment_RB.velocity.x,This_Seg_RB.velocity.y);
			}
		}
//		if(main_controller.touching_wall){
//			if (Mathf.Abs (Main_Segment_RB.velocity.y) < Mathf.Abs (This_Seg_RB.velocity.y)) {
//				This_Seg_RB.velocity = new Vector2 (This_Seg_RB.velocity.x,-1 * Main_Segment_RB.velocity.y);
//			}
//		}
		float H_Move = Input.GetAxis("Horizontal");
		if (Mathf.Abs (H_Move) > 0f) {
			if (This_Seg_RB.velocity.x > 20f) {
				float sign = This_Seg_RB.velocity.x / This_Seg_RB.velocity.x;
				This_Seg_RB.velocity = new Vector2(20f * sign,This_Seg_RB.velocity.y);
			}
		}
	}
}
