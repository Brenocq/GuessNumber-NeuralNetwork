using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataManager : MonoBehaviour {
	private string screenFilePath;

	void Awake () {
		screenFilePath = Application.dataPath + "/Data/matrix.json";
	}

	public void SaveScreen(int number){
		bool[] matrix = new bool[10];
		matrix[2]=true;
		ScreenMatrix currScreen = new ScreenMatrix{
			potato = matrix[1],
		};
		string json = JsonUtility.ToJson(currScreen);
		Debug.Log(json);

		/*using(StreamWriter stream = new StreamWriter(screenFilePath)){
			stream.Write(json);
		}*/

		//ScreenMatrix loaded = JsonUtility.FromJson<ScreenMatrix>(json);
		//Debug.Log(loaded.potato);
	}

	private class ScreenMatrix{
		public bool potato;
	};
}
