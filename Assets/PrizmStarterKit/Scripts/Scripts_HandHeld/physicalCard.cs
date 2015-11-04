using UnityEngine;
using System;
using System.Collections;
using TouchScript;
using TouchScript.Gestures;

//handles card moving to destination
public class physicalCard : MonoBehaviour {
	public string backColor = "";
	public string number = "";
	public string suit = "";
	
	private float rotationSpeed = 300.0f;
	public float moveSpeed = 2.0f;
	public GameObject target = null;

	public void moveToTarget(GameObject target){
		StopAllCoroutines ();
		StartCoroutine (moving (target));
	}	

	IEnumerator moving(GameObject target){
		float elapsedtime = 0;
		while (transform.position != target.transform.position && elapsedtime < 0.5f) {
			transform.position = Vector3.Lerp (transform.position, target.transform.position, elapsedtime/0.5f);
			transform.Rotate (Vector3.up * Time.deltaTime * rotationSpeed);
			elapsedtime += Time.deltaTime;
			yield return null;
		}
		transform.rotation = target.transform.rotation;
		//yield return null;
	}

	public string cardName(){
		return backColor + "" + number + "" + suit;
	}
	public void setCardName(string color, string num, string st){
		backColor = color;
		number = num;
		suit = st;
	}	                
}