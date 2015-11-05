using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Prizm;
using SimpleJSON;
using TouchScript;
using UnityEngine.UI;
using System.Linq;

public class CreateDestroy_Reticle : MonoBehaviour {

	public RfidBinding newObject_Zombie;
	public RfidBinding newObject_pokeball;
	public RfidBinding newObject_card;
	private List<RfidBinding> myList_of_RFID_Objects = new List<RfidBinding>();
	public GameObject reticlePrefab;
	public GameObject mainPanel;
	
	void Start(){
		newObject_Zombie = new RfidBinding (true, "Zombie");
		newObject_pokeball = new RfidBinding (true, "pokeball");
		newObject_card = new RfidBinding (true, "card");
		
		myList_of_RFID_Objects.Add (newObject_Zombie);
		myList_of_RFID_Objects.Add (newObject_pokeball);
		myList_of_RFID_Objects.Add (newObject_card);
		
		foreach (RfidBinding rf_Object in myList_of_RFID_Objects) {
			rf_Object.smartTouchStart += STS;
			rf_Object.smartTouchEnd += STE;
		}
	}

	void OnDisable(){
		foreach (RfidBinding rf_Object in myList_of_RFID_Objects) {
			rf_Object.smartTouchStart -= STS;
			rf_Object.smartTouchEnd -= STE;
		}
	}
	
	public void STS(string ID, Vector3 touchPoint, RfidBinding rf_Object){
		
		Debug.LogError ("performing SmartTouchStart functions!");
		Debug.LogError ("Item "+rf_Object.attribute[0]+" Location "+touchPoint);

		GameObject my_ret = Instantiate(reticlePrefab, new Vector3(touchPoint.x-960, touchPoint.y-540, 70), Quaternion.Euler(0, 0, 0)) as GameObject;
		my_ret.name = "reticle_" + rf_Object.attribute[0];
		my_ret.transform.SetParent (mainPanel.transform);
//		if (touchPoint.x-960 < 0) {
//			my_ret.GetComponent<ParticleSystem>().startColor = Color.yellow;
//
//		}
	}
	
	public void STE(string ID, Vector3 touchPoint, RfidBinding rf_Object){
		Debug.LogError ("performing SmartTouchEnd functions!");
		Debug.LogError ("Item "+ID+" Location "+touchPoint);
		GameObject my_retDestroy = mainPanel.transform.FindChild ("reticle_" + ID).gameObject;
		foreach (Transform child in mainPanel.transform.GetComponentsInChildren<Transform>().ToList()) {

			if(child.gameObject.tag == "Reticle" && child.gameObject.tag != "p_Manager" && child.gameObject.tag != "BB"){
				Debug.Log ("tag"+child.gameObject.tag);
				Destroy (child.gameObject);
			}
		}
	}
}
