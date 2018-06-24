using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Player_Controller : NetworkBehaviour {
	//[SyncVar (hook = "OnIn_ShellChange")]
	private Rigidbody2D rigidB;
	public float move_speed = 2f;
	private bool jump_pressed = false;
	public bool grounded = false;
	public bool touching_wall = false;
	private RaycastHit2D verticalHit;
	private RaycastHit2D verticalHit_L;
	private RaycastHit2D verticalHit_R;
	private RaycastHit2D horizontalHit_R;
	private RaycastHit2D horizontalHit_R_Top;
	private RaycastHit2D horizontalHit_R_Bot;
	private RaycastHit2D horizontalHit_L;
	private RaycastHit2D horizontalHit_L_Top;
	private RaycastHit2D horizontalHit_L_Bot;
	public float jump_force = 100f;
	private LayerMask Detectable_Mask;
	private LayerMask Shell_Mask;
	private bool facingRight;
	public GameObject eyes;
	private SpriteRenderer eyes_renderer;
	public GameObject shell;
	public bool t_shell;
	// Use this for initialization


	void Start () {
		rigidB = transform.GetComponent<Rigidbody2D>();
		Detectable_Mask = LayerMask.NameToLayer("Ground");
		Shell_Mask = LayerMask.NameToLayer("Shell_Trigger");
		facingRight = true;
		eyes_renderer = eyes.GetComponent<SpriteRenderer>();
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawSphere(transform.position, 1f);
	}
	
	// Update is called once per frame
	void Update () {
		if (jump_pressed == false && Input.GetButtonDown ("Jump")) {
			//Cmd_Test ();
			jump_pressed = true;
		} else {
			jump_pressed = false;
		}

//		if (t_shell) {
//			//Debug.Log ("Touching The Shell");
//			Cmd_ChangeShellState(shell);
//		}

		if (Input.GetButtonDown("Get_In"))
		{
			//Debug.Log ("Button Pressed");

			Collider2D in_circle = Physics2D.OverlapCircle (new Vector2(transform.position.x, transform.position.y),1f,1 << Shell_Mask);
			//Gizmos.DrawSphere(transform.position, 1);
			if (in_circle != null) {
				if (in_circle.CompareTag ("Shell_T")) {
					//Do Shell Code, Command, Change Authority
					//Debug.Log ("Touched Shell");
					if (isServer) {
						Rpc_DoShellInteraction_GetIn (in_circle.gameObject.transform.parent.gameObject, in_circle.transform.position);
					} else {
						Cmd_DoShellInteraction_GetIn (in_circle.gameObject.transform.parent.gameObject, in_circle.transform.position);
					}

				}
			} else {
				//Debug.Log ("Didn't Find Shit");
			}
		}

		if (Input.GetButtonDown("Get_Out"))
		{
			Collider2D in_circle = Physics2D.OverlapCircle (transform.position,1f,1 << Shell_Mask);
			if (in_circle != null) {
				if (in_circle.gameObject.CompareTag ("Shell_T")) {
					//Do Shell Code, Command, Change Authority
					//Debug.Log("Touched Shell");
					if (isServer) {
						Rpc_DoShellInteraction_GetOut (in_circle.gameObject.transform.parent.gameObject);
					} else {
						Cmd_DoShellInteraction_GetOut (in_circle.gameObject.transform.parent.gameObject);
					}
				}
			}
		}
	}
		

//	void OnTriggerStay2D(Collider2D _coll){
//		if (_coll.gameObject.CompareTag ("Shell")) {
//			Debug.Log ("Touched Shell");
//			if (Input.GetButtonDown ("Get_In")) {
//				Debug.Log ("Sent Command");
//				Cmd_ChangeShellState (_coll.gameObject);
//			}
//		}
//	}

	void FixedUpdate(){
		Vector2 RayOrigin = new Vector2 (transform.position.x, transform.position.y);

		//Creating Vertical Rays
		verticalHit = Physics2D.Raycast (RayOrigin, Vector2.down, 0.5f, 1 << Detectable_Mask, -2, 2);
		verticalHit_L = Physics2D.Raycast (new Vector2(RayOrigin.x - 0.4f,RayOrigin.y), Vector2.down, 0.5f, 1 << Detectable_Mask, -2, 2);
		verticalHit_R = Physics2D.Raycast (new Vector2(RayOrigin.x + 0.4f,RayOrigin.y), Vector2.down, 0.5f, 1 << Detectable_Mask, -2, 2);
		Debug.DrawRay(RayOrigin,new Vector2(0,-1));
		Debug.DrawRay (new Vector2(RayOrigin.x + 0.4f,RayOrigin.y), new Vector2(0,-1));
		Debug.DrawRay (new Vector2(RayOrigin.x - 0.4f,RayOrigin.y), new Vector2(0,-1));

		//Creating Horizontal Rays
		//--- Right Facing
		Debug.DrawRay(RayOrigin,new Vector2(1,0));
		Debug.DrawRay (new Vector2(RayOrigin.x,RayOrigin.y - 0.4f), new Vector2(1,0));
		Debug.DrawRay (new Vector2(RayOrigin.x,RayOrigin.y + 0.4f), new Vector2(1,0));
		horizontalHit_R = Physics2D.Raycast (RayOrigin, Vector2.right, 0.5f, 1 << Detectable_Mask, -2, 2);
		horizontalHit_R_Top = Physics2D.Raycast (new Vector2(RayOrigin.x,RayOrigin.y + 0.4f), Vector2.right, 0.5f, 1 << Detectable_Mask, -2, 2);
		horizontalHit_R_Bot = Physics2D.Raycast (new Vector2(RayOrigin.x,RayOrigin.y - 0.4f), Vector2.right, 0.5f, 1 << Detectable_Mask, -2, 2);

		//-- Left Facing
		Debug.DrawRay(RayOrigin,Vector2.left);
		Debug.DrawRay (new Vector2(RayOrigin.x,RayOrigin.y - 0.4f), Vector2.left);
		Debug.DrawRay (new Vector2(RayOrigin.x,RayOrigin.y + 0.4f), Vector2.left);
		horizontalHit_L = Physics2D.Raycast (RayOrigin, Vector2.left, 0.5f, 1 << Detectable_Mask, -2, 2);
		horizontalHit_L_Top = Physics2D.Raycast (new Vector2(RayOrigin.x,RayOrigin.y + 0.4f), Vector2.left, 0.5f, 1 << Detectable_Mask, -2, 2);
		horizontalHit_L_Bot = Physics2D.Raycast (new Vector2(RayOrigin.x,RayOrigin.y - 0.4f), Vector2.left, 0.5f, 1 << Detectable_Mask, -2, 2);

		float H_move = Input.GetAxis ("Horizontal");
		//Debug.Log (H_move);
//		if (H_move > 0f) {
//			facingRight = true;
//		} else if (H_move < 0f){
//			facingRight = false;
//		}
		float V_move = Input.GetAxis ("Vertical");

		float movement_H = H_move * move_speed;
		//float movement_V = V_move * move_speed;

		if (verticalHit || verticalHit_L || verticalHit_R) {
			grounded = true;
		} else {
			grounded = false;
		}

		//Nasty If Statement that should be a seperate function
		if (horizontalHit_R || horizontalHit_L || horizontalHit_R_Bot || horizontalHit_R_Top || horizontalHit_L_Bot || horizontalHit_L_Top) {
			touching_wall = true;
		} else {
			touching_wall = false;
		}

		float movement_V;
		//Debug.Log (touching_wall);
		if (touching_wall) {
			movement_V = V_move * move_speed;
		} else {
			movement_V = rigidB.velocity.y;
		}

		rigidB.velocity = new Vector2(movement_H,movement_V);

		if (jump_pressed) {
			if (grounded) {
				rigidB.AddForce (new Vector2 (0f, jump_force));
				jump_pressed = false;
			}
			if (horizontalHit_L) {
				rigidB.AddForce (new Vector2 (3 * jump_force, 0f));
				jump_pressed = false;
			}
			if (horizontalHit_R) {
				rigidB.AddForce (new Vector2 (-3 * jump_force, 0f));
				jump_pressed = false;
			}
		} else {
			jump_pressed = false;
		}

		if (!touching_wall) {
			//eyes_renderer.flipY = false;
			eyes.transform.localRotation = Quaternion.Euler (0, 0, 0);
		} else {
			if (facingRight) {
				eyes.transform.localRotation = Quaternion.Euler (0, 0, 90);
				if (grounded) {
					eyes.transform.localRotation = Quaternion.Euler (0, 0, 45);
				}
			}
			if (!facingRight) {
				eyes.transform.localRotation = Quaternion.Euler (0, 0, -90);
				if (grounded) {
					eyes.transform.localRotation = Quaternion.Euler (0, 0, -45);
				}
			}

		}

		//flip if directions changes
		if (H_move > 0 && !facingRight && !touching_wall){
			facingRight = true;
			eyes_renderer.flipX = false;
		}else if(H_move < 0 && facingRight && !touching_wall){
			eyes_renderer.flipX = true;
			facingRight = false;
		}

		if (V_move > 0 && touching_wall) {
			if (facingRight) {
				eyes_renderer.flipX = false;
			} else {
				eyes_renderer.flipX = true;
			}
		}else if (V_move < 0 && touching_wall) {
			if (facingRight) {
				eyes_renderer.flipX = true;
			} else {
				eyes_renderer.flipX = false;
			}
		}
			

		//rigidB.velocity = new Vector2(movement_H,rigidB.velocity.y);
	}

	void OnCollisionEnter2D(Collision2D _collision)
	{
//		Debug.Log ("Crashing?");
//		Debug.Log (_collision.gameObject);
		if (_collision.gameObject.CompareTag ("Trigger")) 
		{
			//Bunch_Up (gameObject,transform.position);
			Color p_Color = gameObject.GetComponent<SpriteRenderer>().color;
			Color s_Color = _collision.gameObject.GetComponent<SpriteRenderer>().color;
			if (_collision.gameObject.GetComponent<ColorSwitch> ().to_set_active) {
				if (IsColorSomewhatEqual (p_Color, s_Color, 0.1f)) {
					//_collision.gameObject.GetComponent<ColorSwitch>().Wall.SetActive(true);
					if (!_collision.gameObject.GetComponent<ColorSwitch> ()._isActive) {
						//_collision.gameObject.GetComponent<ColorSwitch> ()._isActive = true;
						//collision.gameObject.GetComponent<Player_Controller> ().SyncTrigger (gameObject);
						if (isServer) {
							Rpc_Sync_Trigger_State (_collision.gameObject, true);
						} else {
							Cmd_Sync_Trigger_State (_collision.gameObject, true);
						}
//						if (isServer) {
//							Rpc_StringTest (true);
//						} else {
//							Cmd_StringTest (true);
//						}
					}
				}
			} else {
				//if (CheckRatio(p_Color) == CheckRatio(s_Color)) {
				if (IsColorSomewhatEqual(p_Color, s_Color, 0.1f)){ 
					//_collision.gameObject.GetComponent<ColorSwitch>().Wall.SetActive(false);
					if (_collision.gameObject.GetComponent<ColorSwitch> ()._isActive) {
						//_collision.gameObject.GetComponent<ColorSwitch> ()._isActive = false;
						//collision.gameObject.GetComponent<Player_Controller> ().SyncTrigger (gameObject);
						if (isServer) {
							Rpc_Sync_Trigger_State (_collision.gameObject, false);
						} else {
							Cmd_Sync_Trigger_State (_collision.gameObject, false);
						}
//						if (isServer) {
//							Rpc_StringTest (false);
//						} else {
//							Cmd_StringTest (false);
//						}
					}
				}
			}
//			//Send State Of Trigger

		}
	}

	[Command]
	void Cmd_StringTest(bool test){
		Debug.Log ("String_Test_Cmd");
		Debug.Log (test);
		Rpc_StringTest (test);
	}

	[ClientRpc]
	void Rpc_StringTest(bool test){
		Debug.Log ("String_Test_Rpc");
		Debug.Log (test);
	}

	[Command]
	void Cmd_Sync_Trigger_State(GameObject _trigger, bool _currState)
	{
		//Set Switch Bool
//		Debug.Log(_trigger.GetComponent<ColorSwitch>()._isActive);
//		Debug.Log (_currState);
		_trigger.GetComponent<ColorSwitch>()._isActive = _currState;
		Rpc_Sync_Trigger_State(_trigger,_currState);
	}

	[ClientRpc]
	void Rpc_Sync_Trigger_State(GameObject _trigger, bool _currState)
	{
		//Set Switch Bool
//		Debug.Log(_trigger.GetComponent<ColorSwitch>()._isActive);
//		Debug.Log (_currState);
		_trigger.GetComponent<ColorSwitch>()._isActive = _currState;
	}

	void Bunch_Up(GameObject slug, Vector3 _position){
		int childcount = slug.transform.childCount;
		slug.transform.position = _position;
		for (int i = 0; i < childcount; i++) {
			Transform child = slug.transform.GetChild (i);
			if (child.gameObject.CompareTag ("MainCamera") || child.gameObject.CompareTag ("Player")) {
				//do nothing
			} else {
				child.localPosition = new Vector3 (0, 0, 0);
			}
		}
	}

	void SetAlpha(GameObject toBeChanged,float alpha_value){
		SpriteRenderer _SR = toBeChanged.GetComponent<SpriteRenderer>();
		_SR.color = new Color (_SR.color.r,_SR.color.g,_SR.color.b,alpha_value);
	}

	void ChangePhysicsLayer(GameObject toBeChanged,bool interactive){
		if (interactive) {
			toBeChanged.layer = LayerMask.NameToLayer("Default");
		} else {
			toBeChanged.layer = LayerMask.NameToLayer("No_Player_Collision");
		}
	}

	[ClientRpc]
	void Rpc_DoShellInteraction_GetIn(GameObject _shell, Vector3 _shell_pos)
	{
		//Debug.Log ("Got The Rpc -- In");
		Shell_Sc Shell_script = _shell.GetComponent<Shell_Sc>();
		Shell_script.occupied = true;
		Shell_script.rider = gameObject;
		gameObject.GetComponent<ColorMixer> ().in_shell = true;
		Bunch_Up (gameObject,_shell_pos);
		ChangePhysicsLayer (_shell,true);
		SetAlpha (_shell,0.2f);
	}

	[ClientRpc]
	void Rpc_DoShellInteraction_GetOut(GameObject _shell)
	{
		//Debug.Log ("Got The Rpc -- Out");
		Shell_Sc Shell_script = _shell.GetComponent<Shell_Sc>();
		gameObject.GetComponent<ColorMixer> ().in_shell = false;
		Shell_script.occupied = false;
		Shell_script.rider = null;
		ChangePhysicsLayer (_shell,false);
		SetAlpha (_shell,1f);
	}

	[Command]
	void Cmd_DoShellInteraction_GetIn(GameObject _shell, Vector3 _shell_pos)
	{
		//Debug.Log ("Got The Command -- In");
		Shell_Sc Shell_script = _shell.GetComponent<Shell_Sc>();
		Shell_script.occupied = true;
		Shell_script.rider = gameObject;
		gameObject.GetComponent<ColorMixer> ().in_shell = true;
		Bunch_Up (gameObject,_shell_pos);
		ChangePhysicsLayer (_shell,true);
		SetAlpha (_shell,0.2f);
		Rpc_DoShellInteraction_GetIn (_shell, _shell_pos);
	}

	[Command]
	void Cmd_DoShellInteraction_GetOut(GameObject _shell)
	{
		//Debug.Log ("Got The Command -- Out");
		Shell_Sc Shell_script = _shell.GetComponent<Shell_Sc>();
		gameObject.GetComponent<ColorMixer> ().in_shell = false;
		Shell_script.occupied = false;
		Shell_script.rider = null;
		ChangePhysicsLayer (_shell,false);
		SetAlpha (_shell,1f);
		Rpc_DoShellInteraction_GetOut (_shell);
	}

	public bool IsColorSomewhatEqual(Color player, Color goal, float leniency)
	{

		if ((goal.r - leniency) > player.r || player.r > (goal.r + leniency))
			return false;

		if ((goal.g - leniency) > player.g || player.g > (goal.g + leniency))
			return false;

		if ((goal.b - leniency) > player.b || player.b > (goal.b + leniency))
			return false;

		return true;
	}
}
