using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using ClassesJson;

public class DataManager : MonoBehaviour {
	[SerializeField] GameObject drawScreen;
	[SerializeField] GameObject showDataManager;
	private string filePath;

	void Awake () {
			filePath = Application.streamingAssetsPath;
	}

	void Start(){
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

		string path;
		path = Path.Combine(filePath, "number" + number + ".json");

		using(StreamReader stream = new StreamReader(path)){
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

		string path;
		path = Path.Combine(filePath,"number" + number + ".json");

		using(StreamWriter stream = new StreamWriter(path)){
			stream.Write(json);
			stream.Close();
		}
	}
}
