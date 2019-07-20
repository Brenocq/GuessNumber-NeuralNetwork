using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;
using ClassesJson;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
#if UNITY_EDITOR
    using UnityEditor;
#endif

public class ShowDataManager : MonoBehaviour
{
  [SerializeField] GameObject dataManager;
  private List<int> dataSize = new List<int>();// Number os screen saved for each number
  [Header("Image")]// (ManageDataScene)
  public int selectedNum = 0;// Selected image to show
  public int minImage = 0;//    First image index
  public int qtdImagesScreen = 15;
  private MainData mainData;

  void Start(){
    for (int n=0; n<10; n++) {
      dataSize.Add(0);
    }
    updateDataSize();
    updateTextButtons();

    mainData = dataManager.GetComponent<DataManager>().loadOldData(0);
    if(SceneManager.GetActiveScene().name == "ManageDataScene"){
      updateImages();
    }
  }

  public void updateDataSize(){
    for (int n=0; n<10; n++) {
      dataSize[n] = dataManager.GetComponent<DataManager>().qtdScreenInData(n);
    }
  }

  public void updateTextButtons(){
    string buttonName = "";
    for (int n=0; n<10; n++) {
      if(SceneManager.GetActiveScene().name == "TrainScene"){
        buttonName = "Save (" + n + ")/QuantityNum";
      }else if(SceneManager.GetActiveScene().name == "ManageDataScene"){
        buttonName = "Load (" + n + ")/QuantityNum";
      }
      GameObject qtdText = GameObject.Find(buttonName);
      qtdText.GetComponent<TextMeshProUGUI>().SetText(dataSize[n].ToString());
    }
  }

  public void updateTextButton(int number, int qtdData){
    string buttonName = "";
    if(SceneManager.GetActiveScene().name == "TrainScene"){
      buttonName = "Save (" + number + ")/QuantityNum";
    }else if(SceneManager.GetActiveScene().name == "ManageDataScene"){
      buttonName = "Load (" + number + ")/QuantityNum";
    }
    GameObject qtdText = GameObject.Find(buttonName);
    qtdText.GetComponent<TextMeshProUGUI>().SetText(qtdData.ToString());
  }

  public void setMinImage(bool increase){
    if(increase){
      if(minImage+qtdImagesScreen<mainData.screens.Count){
        minImage+=qtdImagesScreen;
      }
    }else{
      if(minImage>0){
        minImage-=qtdImagesScreen;
        }
    }
    updateImages();
  }

  public void setSelectedNum(int number){
    selectedNum = number;
    minImage = 0;
    updateImages();
  }

  public void generateImage(ScreenData screenData, int number){
    int row = screenData.numRows;
    int col = screenData.numColumns;

    var texture = new Texture2D(row,col, TextureFormat.ARGB32, false);

    for (int i=0; i<col; i++) {
     for (int j=0; j<row; j++) {
       int index = j + i*row;
       Color color = screenData.data[index]==1 ? Color.black : Color.white;
       texture.SetPixel(j, i, color);
     }
    }

    // Apply all SetPixel calls
    texture.Apply();

    GameObject image = GameObject.Find("Image ("+number+")");
    image.GetComponent<RawImage>().texture = texture;
  }

  public void updateImages(){
    // Update text
    GameObject imageNumText = GameObject.Find("ImageNumText");
    int maxImage = minImage+qtdImagesScreen;
    string text = (minImage+1) + "..." + maxImage;
    imageNumText.GetComponent<Text>().text = text;

    mainData = dataManager.GetComponent<DataManager>().loadOldData(selectedNum);

    // Generate new images
    int count = 0;
    for (int i = minImage; i<minImage+qtdImagesScreen; i++) {
      if(i<mainData.screens.Count){
        generateImage(mainData.screens[i], count);
        count++;
      }
    }
  }

  public void deleteImage(int index){
    // Remove image from the List and save
    int imageNumber = minImage+index;
    mainData.screens.RemoveAt(imageNumber);
    dataManager.GetComponent<DataManager>().saveNewData(selectedNum, mainData);
    updateImages();
    updateDataSize();
    updateTextButtons();
  }
}
