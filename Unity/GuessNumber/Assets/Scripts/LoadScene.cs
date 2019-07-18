using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
  public void loadTrainScene(){
    SceneManager.LoadScene("TrainScene");
  }
  public void loadManageDataScene(){
    SceneManager.LoadScene("ManageDataScene");
  }
}
