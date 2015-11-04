#region License
/*
 * TestSocketIO.cs
 *
 * The MIT License
 *
 * Copyright (c) 2014 Fabio Panettieri
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */
#endregion

using System.Collections;
using UnityEngine;
using SocketIO;

public class TestSocketIO : MonoBehaviour
{
	private string RFID;
	private Vector3 smartTouchPoint;
	public GameObject RFIDManager;


	private SocketIOComponent socket;
	public void Start() 
	{
		socket = gameObject.GetComponent<SocketIOComponent>();

		socket.On("open", TestOpen);
		socket.On("smarttouch-start", SmartTouch);
		socket.On("smarttouch-end", SmartTouch);
		socket.On("error", TestError);
		socket.On("close", TestClose);
		
		//StartCoroutine("BeepBoop");
	}
	/*
	private IEnumerator BeepBoop()
	{
		// wait 1 seconds and continue
		yield return new WaitForSeconds(1);
		
		socket.Emit("beep");
		
		// wait 3 seconds and continue
		yield return new WaitForSeconds(3);
		
		socket.Emit("beep");
		
		// wait 2 seconds and continue
		yield return new WaitForSeconds(2);
		
		socket.Emit("beep");
		
		// wait ONE FRAME and continue
		yield return null;
		
		socket.Emit("beep");
		socket.Emit("beep");
	}*/

	public void TestOpen(SocketIOEvent e)
	{
		Debug.Log("[SocketIO] Open received: " + e.name + " " + e.data);
	}

	public void SmartTouch(SocketIOEvent e)
	{
		string typeOfTouch = e.name;
		//Debug.Log("[SocketIO] SmartTouch received: " + e.name + " " + e.data);
		if(typeOfTouch == "smarttouch-start" || typeOfTouch == "smarttouch-end"){
			Debug.LogError (typeOfTouch);
			if (e.data == null) { return; }
			RFID = e.data.GetField("tagId").str;
			if(RFID.Length==12){
				RFID = RFID.Substring(0,RFID.Length -1);
			}
			smartTouchPoint.x = e.data.GetField("x").n;
			smartTouchPoint.y = 1077 - e.data.GetField("y").n;
			Debug.LogError (typeOfTouch);
			Debug.LogError (RFID);
			Debug.LogError (smartTouchPoint);
			//dictionaryReference.socketToRFID (typeOfTouch, RFID, smartTouchPoint);  //this line calls the function inside RFID manager
		}

		/*else if(typeOfTouch == "smarttouch-end"){
			Debug.LogError ("smarttouch-end");
			if (e.data == null) { return; }
			RFID = e.data.GetField("tagId").str;
			smartTouchPoint.x = e.data.GetField("x").n;
			smartTouchPoint.y = 1077 - e.data.GetField("y").n;
			dictionaryReference.destroyRockRFID(RFID);
		}


		Debug.Log ("tagId: " + e.data.GetField ("tagId").str);
		Debug.Log ("x: " + e.data.GetField ("x"));
		Debug.Log ("y: " + e.data.GetField ("y"));
		 */
//		Debug.Log(
//			"#####################################################" +
//			"THIS: " + e.data.GetField("this").str +
//			"#####################################################"
//		);
	}
	
	public void TestError(SocketIOEvent e)
	{
		Debug.Log("[SocketIO] Error received: " + e.name + " " + e.data);
	}
	
	public void TestClose(SocketIOEvent e)
	{	
		Debug.Log("[SocketIO] Close received: " + e.name + " " + e.data);
	}
}
