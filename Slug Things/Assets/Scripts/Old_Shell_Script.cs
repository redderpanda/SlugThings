using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Old_Shell_Script :  NetworkBehaviour{
	[SyncVar (hook = "OnOccUpdate")]
	public bool occupied;
	[SyncVar (hook = "OnRideUpdate")]
	public GameObject rider;
	public GameObject N_Man;
	public NetworkManager N_Mann_C;

	// Use this for initialization
	void Start () {
		occupied = false;
		rider = null;
		N_Man = GameObject.FindGameObjectWithTag ("NetworkManager");
		N_Mann_C = N_Man.GetComponent<NetworkManager> ();
	}

	// Update is called once per frame
	void Update () {
	}

	void OnOccUpdate(bool _occ){
		occupied = _occ;
	}

	void OnRideUpdate(GameObject _ride){
		if (_ride == null) {
			if (rider != null) {
				rider.GetComponent<ColorMixer> ().in_shell = false;
			}
			occupied = false;
			rider = null;
			ChangePhysicsLayer (gameObject,false);
			SetAlpha (gameObject,1f);
			//Debug.Log("GotHere2");
		} else {
			rider = _ride;
			//Debug.Log("GotHere1");
			Debug.Log (rider);
			_ride.GetComponent<ColorMixer> ().in_shell = true;
			Bunch_Up (_ride, transform.position);
			ChangePhysicsLayer (gameObject, true);
			SetAlpha (gameObject, 0.2f);
			occupied = true;
			//Debug.Log ("got to end");
		}
	}

	[ClientRpc]
	void Rpc_DoShellInteraction_GetIn(GameObject _rider, Vector3 _pos)
	{
		Debug.Log ("Got The Rpc -- In");
		occupied = true;
		rider = _rider;
		rider.GetComponent<ColorMixer> ().in_shell = true;
		Bunch_Up (rider,transform.position);
		ChangePhysicsLayer (gameObject,true);
		SetAlpha (gameObject,0.2f);
	}

	[ClientRpc]
	void Rpc_DoShellInteraction_GetOut(GameObject _rider)
	{
		Debug.Log ("Got The Rpc -- Out");
		_rider.GetComponent<ColorMixer> ().in_shell = false;
		occupied = false;
		rider = null;
		ChangePhysicsLayer (gameObject,false);
		SetAlpha (gameObject,1f);
	}

	[Command]
	void Cmd_DoShellInteraction_GetIn(GameObject _rider, Vector3 _pos)
	{
		Debug.Log ("Got The Command -- In");
		occupied = true;
		rider = _rider;
		rider.GetComponent<ColorMixer> ().in_shell = true;
		Bunch_Up (rider,transform.position);
		ChangePhysicsLayer (gameObject,true);
		SetAlpha (gameObject,0.2f);
		Rpc_DoShellInteraction_GetIn (_rider, _pos);
	}

	[Command]
	void Cmd_DoShellInteraction_GetOut(GameObject _rider)
	{
		Debug.Log ("Got The Command -- Out");
		_rider.GetComponent<ColorMixer> ().in_shell = false;
		occupied = false;
		rider = null;
		ChangePhysicsLayer (gameObject,false);
		SetAlpha (gameObject,1f);
		Rpc_DoShellInteraction_GetOut (_rider);
	}

	//	[SyncEvent]
	void OnTriggerStay2D(Collider2D _collider){
		if (_collider.CompareTag ("Player")) {
			if (!occupied) {
				if (Input.GetButtonDown ("Get_In")) {
					//NetworkConnection obj = _collider.gameObject.GetComponent<NetworkIdentity> ().n;
					//NetworkIdentity n_work = ;


					//gameObject.GetComponent<NetworkIdentity> ().AssignClientAuthority (obj);
					//					occupied = true;
					rider = _collider.gameObject;
					//					rider.GetComponent<ColorMixer> ().in_shell = true;
					//					Bunch_Up (rider,transform.position);
					//					ChangePhysicsLayer (gameObject,true);
					//					SetAlpha (gameObject,0.2f);


					if (!isServer) {
						//Debug.Log ("Suck My Cock");
						gameObject.GetComponent<NetworkIdentity>().AssignClientAuthority (N_Mann_C.client.connection);
						Cmd_DoShellInteraction_GetIn (rider, transform.position);
					}
					//
					//					Color r_color = rider.GetComponent<SpriteRenderer> ().color;
					//					rider.GetComponent<SpriteRenderer> ().color = new Color (r_color.r,r_color.g,r_color.b,0);
					//rider.SetActive (false);
				}
			} else {
				if (Input.GetButtonDown ("Get_Out")) {
					//rider.transform.parent = null;
					//rider.SetActive (true);

					//					rider.GetComponent<ColorMixer> ().in_shell = false;
					rider = null;
					//					occupied = false;
					//					ChangePhysicsLayer (gameObject,false);
					//					SetAlpha (gameObject,1f);
					if (!isServer) {
						Cmd_DoShellInteraction_GetOut(rider);
						gameObject.GetComponent<NetworkIdentity>().RemoveClientAuthority (N_Mann_C.client.connection);
					}
				}
			}
		}
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
}
