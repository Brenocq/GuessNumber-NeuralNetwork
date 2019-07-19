using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using ClassesJson;

public class NeuralNetwork : MonoBehaviour
{
  [SerializeField] GameObject dataManager;
  [SerializeField] GameObject drawScreen;
  private int iNodes  = 2601;// 51*51
  private int hNodes = 100;
  private int oNodes = 10;
  private float learningRate = 0.2f;
  private int qtdEpoch = 5;
  private float[,] wih;
  private float[,] who;
  private List<InputData> allData = new List<InputData>();
  private MainData mainData;

  void Start(){
    wih = new float[hNodes,iNodes];
    who = new float[oNodes,hNodes];
    // Populate weights
    float range = 1/Mathf.Sqrt(iNodes);
    for (int i = 0; i<wih.GetLength(0); i++) {
      for (int j = 0; j<wih.GetLength(1); j++) {
        wih[i,j] = UnityEngine.Random.Range(-range, range);
      }
    }

    range = 1/Mathf.Sqrt(hNodes);
    for (int i = 0; i<who.GetLength(0); i++) {
      for (int j = 0; j<who.GetLength(1); j++) {
        who[i,j] = UnityEngine.Random.Range(-range, range);
      }
    }

    //----- Train neural network -----//

    // Add all the data to a List
    for (int i = 0; i<10; i++) {
      mainData = dataManager.GetComponent<DataManager>().loadOldData(i);
      for (int j=0; j<mainData.screens.Count; j++) {
        InputData input = new InputData{
          number = i,
          data = mainData.screens[j].data,
        };
        allData.Add(input);
      }
    }
    for(int epoch=0;epoch<qtdEpoch;epoch++){
      shuffle(allData);// Randomize the data
      for (int i = 0; i<allData.Count; i++) {
        float[,] input = ListToMatrix(allData[i].data);
        train(input, generateTarget(allData[i].number));
      }
    }
    StartCoroutine(processScreen());
  }

  void Update(){

  }

  IEnumerator processScreen(){
    while(true){
      List<int> data = drawScreen.GetComponent<Draw>().getData();
      int bestNumber = 0;
      float bestNumberProb = 0;
      float[,] inputs = ListToMatrix(data);
      float[,] output = query(inputs);
      // Find best number
      for (int i=0; i<output.GetLength(0); i++) {
        if(output[i,0]>bestNumberProb){
          bestNumber = i;
          bestNumberProb = output[i,0];
        }
      }
      // Change bar sizes
      for (int i=0; i<output.GetLength(0); i++) {
        Slider bar = GameObject.Find("Bar ("+i+")").GetComponent<Slider>();
        bar.value = output[i,0];
      }

      // Show number
      GameObject resultText = GameObject.Find("Guess");
      resultText.GetComponent<TextMeshProUGUI>().SetText(bestNumber.ToString());

      yield return new WaitForSeconds(0.1f);
    }
  }

  private void train(float[,] inputs, float[,] targets){
    float[,] hidden_inputs = dot(wih, inputs);
    float[,] hidden_outputs = activation_function(hidden_inputs);

    float[,] final_inputs = dot(who, hidden_outputs);
    float[,] final_outputs = activation_function(final_inputs);

    float[,] output_errors = sub(targets, final_outputs);
    float[,] hidden_errors = dot(transpose(who), output_errors);

    who = add(who, product(learningRate,dot(product(product(output_errors,final_outputs), sub(1.0f,final_outputs)),transpose(hidden_outputs))));
    wih = add(wih, product(learningRate,dot(product(product(hidden_errors,hidden_outputs), sub(1.0f,hidden_outputs)),transpose(inputs))));
  }

  private float[,] query(float[,] inputs){
    float[,] hidden_inputs = dot(wih, inputs);
    float[,] hidden_outputs = activation_function(hidden_inputs);
    float[,] final_inputs = dot(who, hidden_outputs);
    float[,] final_outputs = activation_function(final_inputs);
    return final_outputs;
  }

  private float[,] generateTarget(int number){
    float[,] target = new float[10,1];
    for (int i = 0; i<10; i++){
      target[i,0]=0.01f;
    }
    target[number,0]=0.99f;
    return target;
  }

  private float[,] ListToMatrix(List<int> data){
    float[,] input = new float[iNodes,1];
    for (int i = 0; i<iNodes; i++) {
      input[i,0] = (float)data[i];
    }
    return input;
  }

  private float[,] activation_function(float[,] input){
    float[,] output = new float[input.GetLength(0), input.GetLength(1)];

    if(input.GetLength(1)==1){
      for (int i = 0; i < input.GetLength(0); i++) {
        // Sigmoid function
        output[i,0] = 1.0f / (1.0f + Mathf.Exp(-input[i,0]));
      }
    }
    return output;
  }

  //----- Matrix calculations -----//
  private float[,] dot(float[,] A, float[,] B){
    float[,] R = new float[A.GetLength(0), B.GetLength(1)];
    if(A.GetLength(1)==B.GetLength(0)){
      for (int i = 0; i < A.GetLength(0); i++) {
        for (int j = 0; j < A.GetLength(1); j++) {
          for (int k = 0; k < B.GetLength(1); k++) {
            R[i,k] += A[i,j]*B[j,k];
          }
        }
      }
    }else{
      Debug.LogError("Wrong size to make dot product of matrices");
    }
    return R;
  }

  private float[,] product(float[,] A, float[,] B){
    float[,] R = new float[A.GetLength(0), A.GetLength(1)];
    if(A.GetLength(0)==B.GetLength(0) && A.GetLength(1)==B.GetLength(1)){
      for (int i = 0; i < A.GetLength(0); i++) {
        for (int j = 0; j < A.GetLength(1); j++) {
            R[i,j] = A[i,j]*B[i,j];
        }
      }
    }else{
      Debug.LogError("Wrong size to make product of matrices");
    }
    return R;
  }

  private float[,] product(float num, float[,] A){
    float[,] R = new float[A.GetLength(0), A.GetLength(1)];

    for (int i = 0; i < A.GetLength(0); i++) {
      for (int j = 0; j < A.GetLength(1); j++) {
          R[i,j] =num*A[i,j];
      }
    }
    return R;
  }

  private float[,] transpose(float[,] A){
    float[,] R = new float[A.GetLength(1), A.GetLength(0)];
    for (int i = 0; i < A.GetLength(0); i++) {
      for (int j = 0; j < A.GetLength(1); j++) {
        R[j,i] = A[i,j];
      }
    }
    return R;
  }

  private float[,] sub(float[,] A, float[,] B){
    float[,] R = new float[A.GetLength(0), A.GetLength(1)];
    if(A.GetLength(0)==B.GetLength(0) && A.GetLength(1)==B.GetLength(1)){
      for (int i = 0; i < A.GetLength(0); i++) {
        for (int j = 0; j < A.GetLength(1); j++) {
          R[i,j] = A[i,j]-B[i,j];
        }
      }
    }else{
      Debug.LogError("Wrong size to subtract matrices");
    }
    return R;
  }

  private float[,] add(float[,] A, float[,] B){
    float[,] R = new float[A.GetLength(0), A.GetLength(1)];
    if(A.GetLength(0)==B.GetLength(0) && A.GetLength(1)==B.GetLength(1)){
      for (int i = 0; i < A.GetLength(0); i++) {
        for (int j = 0; j < A.GetLength(1); j++) {
          R[i,j] = A[i,j]+B[i,j];
        }
      }
    }else{
      Debug.LogError("Wrong size to add matrices");
    }
    return R;
  }

  private float[,] sub(float num, float[,] A){
    float[,] R = new float[A.GetLength(0), A.GetLength(1)];
    for (int i = 0; i < A.GetLength(0); i++) {
      for (int j = 0; j < A.GetLength(1); j++) {
        R[i,j] = num - A[i,j];
      }
    }
    return R;
  }

  public class InputData{
    public int number;
    public List<int> data;
  };

  private void shuffle(List<InputData> ts) {
    var count = ts.Count;
    var last = count - 1;
    for (var i = 0; i < last; ++i) {
        var r = UnityEngine.Random.Range(i, count);
        var tmp = ts[i];
        ts[i] = ts[r];
        ts[r] = tmp;
    }
  }
}
