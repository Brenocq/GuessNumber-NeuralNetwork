using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataManager : MonoBehaviour {
	[SerializeField] GameObject drawScreen;
	[SerializeField] GameObject showDataManager;
	private string filePath;

	void Awake () {
		filePath = Application.dataPath + "/Resources/number";
	}

	public void SaveScreen(int number){
		// Get data from the current screen
		List<int> data = drawScreen.GetComponent<Draw>().getData();
		int qtdPixels = drawScreen.GetComponent<Draw>().getPixelQtd();

		MainData mainData;

		mainData = loadOldData(number);
		mainData = updateData(mainData, data, qtdPixels);
		saveNewData(number, mainData);

		drawScreen.GetComponent<Draw>().clearScreen();
		showDataManager.GetComponent<ShowDataManager>().updateTextButton(number, mainData.screens.Count);
	}

	public MainData loadOldData(int number){
		// Read from file
		string json;
		using(StreamReader stream = new StreamReader(filePath + number + ".json")){
			json = stream.ReadToEnd();
			stream.Close();
		}

		// Convert json to object
		MainData mainData = JsonUtility.FromJson<MainData>(json);

		return mainData;
	}

	public int qtdScreenInData(int number){
		MainData mainData;
		mainData = loadOldData(number);
		return mainData.screens.Count;
	}

	public MainData updateData(MainData mainData, List<int> newData, int qtdPixels){
		// Object to store screen data
		ScreenData currScreen = new ScreenData{
			numRows = qtdPixels,
			numColumns = qtdPixels,
			data = newData,
		};

		// Add a new screen to the data
		mainData.screens.Add(currScreen);

		return mainData;
	}

	public void saveNewData(int number, MainData newMainData){
		// Write to file
		string json = JsonUtility.ToJson(newMainData);
		Debug.Log("Saving screen to number" + number + ".json");
		using(StreamWriter stream = new StreamWriter(filePath + number + ".json")){
			stream.Write(json);
			stream.Close();
		}
	}

	// Classes to store the data
	[System.Serializable]
	public class ScreenData{
		public int numRows;
		public int numColumns;
		public List<int> data;
	};

	[System.Serializable]
	public class MainData{
		public List<ScreenData> screens = new List<ScreenData>();
	};
}
