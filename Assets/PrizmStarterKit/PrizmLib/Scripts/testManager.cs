using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Prizm;
using SimpleJSON;
using TouchScript;

//would-be-developer's script
public class testManager: MonoBehaviour {
	public RfidBinding newObject;
	
	void Start(){
		newObject = new RfidBinding (true, "Zombie");
		newObject.smartTouchStart += STS;
		newObject.smartTouchEnd += STE;
	}

	public void STS(string ID, Vector3 touchPoint){

		Debug.LogError ("performing SmartTouchStart functions!");
		Debug.LogError ("Item "+ID+" Location "+touchPoint);
	}
	
	public void STE(string ID, Vector3 touchPoint){
		Debug.LogError ("performing SmartTouchEnd functions!");
		Debug.LogError ("Item "+ID+" Location "+touchPoint);
	}



}