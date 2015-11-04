using UnityEngine;
using System.Collections;

public class BootstrapHH : MonoBehaviour {
	PrizmRecordGroup recordGroup;
	ClientInitialization clientInit;
	GameManager_HH gameManager;
	
	void Awake() {
		recordGroup = GameObject.Find ("HH_GameManager").GetComponent<PrizmRecordGroup> ();
		clientInit = GameObject.Find ("HH_GameManager").GetComponent<ClientInitialization> ();
		gameManager = GameObject.Find ("HH_GameManager").GetComponent<GameManager_HH> ();


	}
	
	public IEnumerator Bootstrap() {
		yield return StartCoroutine (clientInit.MeteorInit ());


	}
}
