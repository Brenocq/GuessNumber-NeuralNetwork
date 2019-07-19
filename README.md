# GuessNumber NeuralNetwork
<p align="center">
 <img src="https://github.com/Brenocq/GuessNumber-NeuralNetwork/blob/master/Images/TrainScene5.png" height="500">
 <img src="https://github.com/Brenocq/GuessNumber-NeuralNetwork/blob/master/Images/TrainScene2.png" height="500">
</p>

## Introduction
This project was developed in the Unity engine and consists of a program to classify human-written numbers using a neural network with 3 layers. I used the Gravit Designer to create the images in the interface.

## The Neural Network
The neural network was developed based on the book I was reading: _Make your own neural network - Tariq Rashid_ (I strongly recommend this book for beginners). I have based my code on the python code provided by the book. The input of the NN was a list with 2601 values (51x51), each value could be only 0 or 1. 10 output nodes were used to classify the numbers, the output can vary between 0 and 1. The sigmoid function was used as the activation function.
To train the data it is possible to set the number of epochs and the learning rate.

### Trainig Data
To train the neural network I used the `.json` that I had previously generated. There are 10 `.json` files where all the data about the human-written numbers are stored (these files can be found [here](https://github.com/Brenocq/GuessNumber-NeuralNetwork/tree/master/Unity/GuessNumber/Assets/Resources)). Each time the user clicks a number button (on the right in the figure below), the data of the drawn pixels is added to the `.json` file of that number. The number at the bottom of each button is the number of screens saved in the `.json` of that number.

## Scenes
### Train Scene
When this scene is loaded, the NN is trained with all the data stored in the `.json` files. In the middle of this scene there is a screen where it is possible to draw numbers with size of 51x51 pixels. On the left, the number that the NN thinks is being drawn is displayed and a graph with the output on each output node is displayed. On the right, there are buttons for adding more data to the training data.
<p align="center">
 <img src="https://github.com/Brenocq/GuessNumber-NeuralNetwork/blob/master/Images/TrainScene3.png" height="500">
</p>

### Manage Data Scene
In this scene it is possible to manage all the data stored. You can select from which number the data will be displayed and see all the stored data of that number as png images. It is possible to delete images that will not contribute to train the NN (such as a blank image sent as a mistake).
To save space, only the images being displayed in the scene exist in the hard disk. Before show the images all the 15 old images are deleted and the new 15 images are generated to be used when displaying the data.
<p align="center">
 <img src="https://github.com/Brenocq/GuessNumber-NeuralNetwork/blob/master/Images/ManageData7.png" height="500">
</p>

## How to test
There is two ways to test this program. The first way is to **install the Unity 2018.3**. The second way is to **execute the `GuessNumberNeuralNetwork.x86_64` file** in the Executable folder in your computer. 
Obs: It is only possible to edit this program using the Unity Engine, if you want to, use the first way.
<p align="center">
 <img src="https://github.com/Brenocq/GuessNumber-NeuralNetwork/blob/master/Images/UnityInterface.png" height="500">
</p>

## Future Work
 - Create a new scene to manage the NN
  - Calculate the precision of the results (separate training data from test data)
  - Create precision graphs using different learning rates/epochs/activation functions
  - Display the link weights
 - Create a new scene to set the train parameters of the NN that will be used in the train scene
