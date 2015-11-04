using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TouchScript;
using TouchScript.Gestures;


public class GameManager_TT: MonoBehaviour {
	public GameObject playerPrefab;

	public PrizmRecordGroup recordGroup; 
	public BootstrapTT bootstrap;

	private GameObject cube;

	void Awake () {
		recordGroup = GetComponent<PrizmRecordGroup>();
		bootstrap = GetComponent<BootstrapTT> ();
	}

	public void CreateNewPlayer(string playerName, string player_id) {
		Debug.Log ("Tap the cube.");
		cube = Instantiate (playerPrefab) as GameObject;
		cube.name = playerName;
		cube.GetComponent<touchMe> ().playerID = player_id;
		cube.GetComponent<touchMe> ().dbEntry.location = "home";
		cube.GetComponent<touchMe> ().AddToRecordGroup ();
		//recordGroup.AddRecord(cube.GetComponent<touchMe>());
	}
	
	public void HandleDidChangeRecord (string arg1, DatabaseEntry arg2, IDictionary arg3, string[] arg4){
		Debug.Log ("record changed: " + arg2.location);
		if (arg2.location == "home") {
			cube.SetActive(true);
		} 
	}

	//when someone quits the game
	public void HandleDidLosePlayer(string id) {
		foreach(GameObject obj in Object.FindObjectsOfType(typeof(GameObject))){
			if(obj.tag == "Player"){
				if(obj.GetComponent<touchMe>().playerID == id){
					Destroy(obj);
				}
			}
		}

		Debug.Log ("player lost connection, object is: " + id);

	}

	void OnApplicationQuit(){
		reset ();
	}
	
	public void reset(){
		StartCoroutine (resetGame ());
	}

	IEnumerator resetGame() {
		var methodCall = Meteor.Method<ChannelResponse>.Call ("endTabletopSession", GameObject.Find ("TT_GameManager").GetComponent<TabletopInitialization>().sessionID);
		yield return (Coroutine)methodCall;
		//reset the scene, make visual indicator
	}
}