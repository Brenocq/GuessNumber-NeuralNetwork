using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShowDataManager : MonoBehaviour
{
  [SerializeField] GameObject dataManager;
  private List<int> dataSize = new List<int>();// Number os screen saved for each number

  void Start()
  {
    for (int n=0; n<10; n++) {
      dataSize.Add(0);
    }
    updateDataSize();
    updateTextButtons();
  }

  void Update()
  {
  }

  public void updateDataSize(){
    for (int n=0; n<10; n++) {
      dataSize[n] = dataManager.GetComponent<DataManager>().qtdScreenInData(n);
    }
  }

  public void updateTextButtons(){
    for (int n=0; n<10; n++) {
      GameObject qtdText = GameObject.Find("Save ("+n+")/QuantityNum");
      qtdText.GetComponent<TextMeshProUGUI>().SetText(dataSize[n].ToString());
    }
  }

  public void updateTextButton(int number, int qtdData){
    GameObject qtdText = GameObject.Find("Save ("+number+")/QuantityNum");
    qtdText.GetComponent<TextMeshProUGUI>().SetText(qtdData.ToString());
  }

}
