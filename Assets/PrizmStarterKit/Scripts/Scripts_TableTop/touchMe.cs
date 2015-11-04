using UnityEngine;
using System.Collections;
using TouchScript;
using TouchScript.Gestures;
using System;

public class touchMe : PrizmRecord {
	public string playerID = "";

	protected override void Awake() {
		base.Awake ();
	}


	void OnEnable(){
		GetComponent<TapGesture> ().Tapped += tapHandler;
	}

	void OnDisable(){
		GetComponent<TapGesture> ().Tapped -= tapHandler;
	}

	private void tapHandler(object sender, EventArgs e){
		dbEntry.location = "Player";
		Debug.Log (dbEntry.location);
		needsUpdate = true;
		StartCoroutine(GameObject.Find ("GameManager").GetComponent<PrizmRecordGroup> ().SyncAll ());
		gameObject.SetActive (false);
	}

	public void setName(string n){
		playerID = n;
	}

}
