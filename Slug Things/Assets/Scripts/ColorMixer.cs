using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ColorMixer : NetworkBehaviour {
    Color curr_color;
	Color _temp_curr_color;
    public int colors_mixed = 1;
	//Hmm
//	[SyncVar(hook = "On_Shell_Change")]
	public bool in_shell;

//	void On_Shell_Change(bool _In_Shell){
//		if (isServer) {
//			Rpc_SyncIn_Shell (gameObject,_In_Shell);
//		} else {
//			Cmd_SyncIn_Shell (gameObject,_In_Shell);
//		}
//	}

	[Command]
	void Cmd_SyncIn_Shell(GameObject _player,bool _In_Shell)
	{
		_player.GetComponent<ColorMixer> ().in_shell = _In_Shell;
		Rpc_SyncIn_Shell (_player,_In_Shell);
	}

	[ClientRpc]
	void Rpc_SyncIn_Shell(GameObject _player,bool _In_Shell)
	{
		_player.GetComponent<ColorMixer> ().in_shell = _In_Shell;
	}

    //private void Awake()
    //{
    //    GameObject gc = GameObject.FindGameObjectWithTag("Game Controller");
    //    Level_1_Game_Manager L1 = gc.GetComponent<Level_1_Game_Manager>();
    //    if(L1 != null)
    //    {
    //        if(L1.player1 == null)
    //        {
    //            L1.player1 = this.gameObject;
    //        }
    //        else
    //        {
    //            L1.player2 = this.gameObject;
    //        }
    //    }
    //}

    // Use this for initialization

	void On_Shell_Entry(bool _in_shell){
		in_shell = _in_shell;
	}

    void Start()
    {
		in_shell = false;

        curr_color = GetComponent<SpriteRenderer>().color;

		try{
			GameObject gc = GameObject.FindGameObjectWithTag("GameController");
			Level_1_Game_Manager L1 = gc.GetComponent<Level_1_Game_Manager>();
			if (L1 != null)
			{
				if (L1.player1 == null)
				{
					L1.player1 = this.gameObject;
				}
				else
				{
					L1.player2 = this.gameObject;
				}
			}
			Level_2_Game_Manager L2 = gc.GetComponent<Level_2_Game_Manager>();
			if (L2 != null)
			{
				if (L2.player1 == null)
				{
					L2.player1 = this.gameObject;
				}
				else
				{
					L2.player2 = this.gameObject;
				}
			}
		}catch{
			Debug.Log ("No Game Manager In Scene");
		}
        
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<SpriteRenderer>().color = curr_color;

    }

	[Command]
	void Cmd_SyncUpColors(Color _color){
		curr_color = _color;
		Rpc_SyncUpColors (_color);
	}

	[ClientRpc]
	void Rpc_SyncUpColors(Color _color){
		curr_color = _color;
	}

	[Command]
	void Cmd_SyncUp_PlayerColors(GameObject _player, Color _color){
		_player.gameObject.GetComponent<ColorMixer>().curr_color = _color;
		Rpc_SyncUp_PlayerColors (_player,_color);
	}

	[ClientRpc]
	void Rpc_SyncUp_PlayerColors(GameObject _player, Color _color){
		_player.gameObject.GetComponent<ColorMixer>().curr_color = _color;
	}

//	[SyncEvent]
    private void OnCollisionEnter2D(Collision2D collision)
    {
		if (!in_shell) {
			if (collision.gameObject.tag == "Player") {
				Color object_color = collision.gameObject.GetComponent<SpriteRenderer> ().color;
				if (curr_color != object_color) {
					float red, blue, green = 0f;
					float max = 0f;
					float min = 1000f;
					red = curr_color.r + collision.gameObject.GetComponent<SpriteRenderer> ().color.r;
					blue = curr_color.b + collision.gameObject.GetComponent<SpriteRenderer> ().color.b;
					green = curr_color.g + collision.gameObject.GetComponent<SpriteRenderer> ().color.g;
					//Debug.Log("red: " + red);
					//Debug.Log("blue: " + blue);
					//Debug.Log("green: " + green);
					if (red > max)
						max = red;
					if (red < min && red > 0f)
						min = red;
					if (green > max)
						max = green;
					if (green < min && green > 0f)
						min = green;
					if (blue > max)
						max = blue;
					if (blue < min && blue > 0f)
						min = blue;
					//Debug.Log("min: " + min);
					//Debug.Log("max: " + max);
					if (min != max) {
						_temp_curr_color = new Color 
							(Mathf.Clamp (red / max, 0f, 1f), Mathf.Clamp (green / max, 0f, 1f), Mathf.Clamp (blue / max, 0f, 1f));
//						curr_color = new Color 
//							(Mathf.Clamp (red / max, 0f, 1f), Mathf.Clamp (green / max, 0f, 1f), Mathf.Clamp (blue / max, 0f, 1f));
					} else {
						_temp_curr_color = new Color (red, green, blue);
//						curr_color = new Color (red, green, blue);
					}

					if (isServer) {
						Rpc_SyncUp_PlayerColors (collision.gameObject,_temp_curr_color);
					} else {
						Cmd_SyncUp_PlayerColors (collision.gameObject,_temp_curr_color);
					}
					//red = 0f;
					//blue = 0f;
					//green = 0f;
				}
			} else if (collision.gameObject.tag == "ColorableTile") {
				collision.gameObject.GetComponent<SpriteRenderer> ().color = 
					new Color(curr_color.r, curr_color.g, curr_color.b, collision.gameObject.GetComponent<SpriteRenderer> ().color.a);
			}
		}
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
		if (!in_shell) {
			if (collision.gameObject.tag == "Cleaner") {
				curr_color = Color.black;
			} else if (collision.gameObject.tag == "Add Color") {
				curr_color = collision.gameObject.GetComponent<SpriteRenderer> ().color;
				if (isServer) {
					Rpc_SyncUp_PlayerColors (gameObject, curr_color);
				} else {
					//client
					Cmd_SyncUp_PlayerColors (gameObject, curr_color);
				}

			} else if (collision.gameObject.tag == "ColorableTile") {
				collision.gameObject.GetComponent<SpriteRenderer> ().color =
					new Color(curr_color.r, curr_color.g, curr_color.b, collision.gameObject.GetComponent<SpriteRenderer> ().color.a);
			}
		}
    }

}
