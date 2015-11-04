using UnityEngine;
using System.Collections;

public class BootstrapTT : MonoBehaviour {

	PrizmRecordGroup recordGroup;
	GameManager_TT gameManager;
	TabletopInitialization tabletopInit;
	
	void Awake () {
		recordGroup = GetComponent<PrizmRecordGroup>();
		gameManager = GetComponent<GameManager_TT> ();
		tabletopInit = GetComponent<TabletopInitialization> ();
	}

	void Start() {
		StartCoroutine (Bootstrap ());

	}
	public IEnumerator Bootstrap() {
		yield return StartCoroutine (tabletopInit.MeteorInit ());					//Establishes connection to database, creates collections, runs sync routine, etc.
	}
}
