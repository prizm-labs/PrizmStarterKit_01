using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Prizm;
using SimpleJSON;
using TouchScript;
using UnityEngine.UI;

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
	
	public void STS(string ID, Vector3 touchPoint){
		
		Debug.LogError ("performing SmartTouchStart functions!");
		Debug.LogError ("Item "+ID+" Location "+touchPoint);

		GameObject my_ret = Instantiate(reticlePrefab, new Vector3(touchPoint.x-960, touchPoint.y-540, 70), Quaternion.Euler(0, 0, 0)) as GameObject;
		my_ret.transform.SetParent (mainPanel.transform);
	}
	
	public void STE(string ID, Vector3 touchPoint){
		Debug.LogError ("performing SmartTouchEnd functions!");
		Debug.LogError ("Item "+ID+" Location "+touchPoint);
	}
}
