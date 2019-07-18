using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ClassesJson
{
	// Classes to store the data
	[System.Serializable]
	public class ScreenData{
		public int numRows;
		public int numColumns;
		public List<int> data;
	};
	// Classes to store ScreenData
	[System.Serializable]
	public class MainData{
		public List<ScreenData> screens = new List<ScreenData>();
	};
}
