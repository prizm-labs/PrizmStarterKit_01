using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using SimpleJSON;
using SocketIO;

//would-be-prizm-sdk
/// <summary>
/// This namespace is prizm's core algorithm that combines both multitouch and rfid.
/// </summary>
/// 
namespace Prizm {
	public enum touchType{smartTouchStart, smartTouchEnd};
	public delegate void rfidDetected(string ID, Vector3 location);


	/// <summary>
	/// This class is for creating a new RFID binding that associates an RFID's attribute and triggered event.
	/// </summary>
	public class RfidBinding{
		//public touchType smartTouches;
		public bool enableBinding;
		public Dictionary<string, GameObject>Properties = new Dictionary<string, GameObject> (); 
		public Dictionary<string, GameObject>Instantiated = new Dictionary<string, GameObject> ();
		public List<string> attribute = new List<string>();

		public rfidDetected smartTouchStart;
		public rfidDetected smartTouchEnd;

		public RfidBinding(){
			BindManager.bindedObjects.Add (this);
		}

		public RfidBinding(params string[] att){
			enableBinding = true;
			for (int i = 0; i<att.Length; i++) {
				attribute.Add(att[i]);
			}
			BindManager.bindedObjects.Add (this);
		}

		public RfidBinding(bool enableB, params string[] att){
			enableBinding = enableB;
			for (int i = 0; i<att.Length; i++) {
				attribute.Add(att[i]);
			}
			BindManager.bindedObjects.Add (this);
		}
	}

	public static class BindManager{
		public static List<RfidBinding> bindedObjects = new List<RfidBinding>();

		public static RfidBinding checkBindings(JSONNode JSONatt, touchType smartTouchType){
			//Debug.LogError ("Checking rfid bindings that contains attributes...");
			RfidBinding rfidObject = null;
			for (int i = 0; i < bindedObjects.Count; i++) {
				RfidBinding temp = bindedObjects [i];
				for (int k = 0; k<JSONatt.Count; k++) {
					string tempAttribute = JSONatt [k].Value;
					if (temp.attribute.Contains (tempAttribute)) {
						i = bindedObjects.Count;
						k = JSONatt.Count;
						//Debug.LogError ("Attributes found!...'" + tempAttribute + "', checking if binding is enabled and functions exists!");
						if(temp.enableBinding && 
							((smartTouchType == touchType.smartTouchStart && temp.smartTouchStart != null) ||
						 (smartTouchType == touchType.smartTouchEnd && temp.smartTouchEnd != null))){
							rfidObject = temp;
						}

					} else {
						//Debug.LogError ("No bindings exists with attributes, " + tempAttribute);
						rfidObject = null; //object exits, but not enabled or does not contain executable function
					}
				}
			}
			return rfidObject;
		}

	}

	public static class RFIDEventManager{

		public static void rfidDetected(string ID, touchType ST, Vector3 location){
			RfidBinding rfidObject;
			if (checkJSON.compareJSON (ID, ST, location, out rfidObject)) {
				if (ST == touchType.smartTouchEnd) {
					rfidObject.smartTouchEnd (ID, location);
				} else if (ST == touchType.smartTouchStart) {
					rfidObject.smartTouchStart (ID, location);
				}
			} else
				Debug.LogError ("Unregistered RFID: " + ID);
		}

	}

	public static class checkJSON{
		private static TextAsset textFile;
		private static string jsonPath;
		private static StreamReader sr;
		private static JSONClass j;
		public static rfidDetected noTagFound;

		public static bool compareJSON(string ID, touchType ST, Vector3 location, out RfidBinding rfidObject){
			////Debug.LogError ("Comparing socket input with JSON file and RfidBindings");
			JSONNode attributes = findTagInJSON (ID, location);
			rfidObject = BindManager.checkBindings (attributes, ST);
			if (attributes != null && rfidObject != null){
				return true;
			} else 
				return false;
		}

		public static void readJson(){
			jsonPath = Application.streamingAssetsPath + "/rfidpieces.json";
			sr = new StreamReader (jsonPath);
			j = JSON.Parse (sr.ReadToEnd()) as JSONClass;
		}

		//find requested tag and returns all values of attribute in the form JSONNode
		public static JSONNode findTagInJSON(string ID, Vector3 location){
			JSONNode attributes = null;
			//Debug.LogError ("Searching tag in JSON file with ID: " + ID);
			if (j != null) {
				for (int i = 0; i < j["rfidPieces"].Count; i++) {
					for (int k = 0; k < j["rfidPieces"][i]["tags"].Count; k++) {
						for(int l = 0; l<j["rfidPieces"][i]["tags"][k]["id"].Count; l++){
							string tempValue = j ["rfidPieces"] [i] ["tags"] [k] ["id"][l].Value;
							if (tempValue == ID) {
								//Debug.LogError ("Tag found, " + tempValue + " at: manifest object # " + i +", tag object # " + k +" id #: " + l + ", is: " + ID);
								attributes = j ["rfidPieces"] [i] ["tags"] [k] ["attributes"];

								//attributes found, end all loop iterations by setting conditions to max value.
								k = j["rfidPieces"][i]["tags"].Count;
								i = j["rfidPieces"].Count;
							} else {
								//Debug.LogError ("Manifest object # " + i +", tag object # " + k +" id #: " + l + ", is not: " + ID);
							}
						}
					}
				}
			} else {
				//Debug.LogError ("j is null. Must read JSON file first. Use checkJSON.readJson()");
			}
			return attributes;
		}
	}
}