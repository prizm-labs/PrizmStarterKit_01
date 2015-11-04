using UnityEngine;
using SimpleJSON;
using System.IO;

public class JSONTest : MonoBehaviour {
	private TextAsset textFile;
	private string jsonPath;
	private StreamReader sr;
	// Use this for initialization
	void Start () {
		readJson ();
		//accessData (j);
	}

	public void readJson(){
		jsonPath = Application.streamingAssetsPath + "/rfidpieces.json";
		//textFile = (TextAsset)Resources.Load (jsonPath, typeof(TextAsset));
		//Debug.LogError (textFile.text);
		//var textFile = (TextAsset)File.ReadAllText(Application.streamingAssetsPath + "/tag-manifest-sample") as TextAsset;
		sr = new StreamReader (jsonPath);
		//textFile = new File.OpenText (jsonPath);
		//textFile = (TextAsset)Path.get(Application.streamingAssetsPath + "/tag-manifest-sample");

		//textFile = Application.streamingAssetsPath ();

		var j = JSON.Parse (sr.ReadToEnd());
		string id = j ["objects"] [0] ["tags"] [0] ["id"].Value;
		Debug.LogError (id);
		Debug.LogError(j.GetType());
	}

}
