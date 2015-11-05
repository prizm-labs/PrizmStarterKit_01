using System.Collections;
using UnityEngine;
using SocketIO;
using SimpleJSON;
using Prizm;

[HideInInspector]
public class SocketToJSON : MonoBehaviour{
	private SocketIOComponent socket;
	public void Start() {
		socket = gameObject.GetComponent<SocketIOComponent>();
		socket.On("smarttouch-start", SmartTouch);
		socket.On("smarttouch-end", SmartTouch);
		checkJSON.readJson();
	}

	//when receiving smart touch data, call this function:
	public void SmartTouch(SocketIOEvent e){
		string RFID = e.data.GetField("tagId").str;
		string typeOfTouch = e.name;
		Vector3 smartTouchPoint = new Vector3 ();
		touchType ST;
		ST = enumerateString (typeOfTouch);
		RFID = filterRFID (RFID);
		smartTouchPoint.x = e.data.GetField("x").n;
		smartTouchPoint.y = 1080 - e.data.GetField("y").n;
		Debug.LogError (RFID);
		//Debug.Log
		RFIDEventManager.rfidDetected (RFID, ST, smartTouchPoint);
	}

	private touchType enumerateString(string str){
		if (str == "smarttouch-start") {
			return touchType.smartTouchStart;
		} else
			return touchType.smartTouchEnd;
	}

	private string filterRFID(string ID){
		if (ID.Length == 12) {
			return ID.Substring (0, ID.Length - 1);
		} else
			return ID;

	}
}