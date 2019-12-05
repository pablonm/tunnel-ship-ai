# Tunnel Spaceship AI

This is a little project about genetic algorithms and neural networks. The goal is to make a spaceship learn how to fly through a procedurally generated tunnel. [Here is an overview video of the project](www.google.com)

## The model

Each spaceship has a neural network. The neural network has 5 inputs: left, right, up, down and front discances to the nearest tunnel wall. Then a hidden layer of size 8, and an output layed of size 4. Each output corresponds to one of the possible actions of the ship (turn left, right, up or down).

## The genetic algorithm

In this case I'm using generations of 1000 individuals each. The selection is very simple, I just pick the 5% best ships based in their score. The crossover is also very simple, I combine random pairs of the selected ships to create new ones. There is also a bit of mutation.

## Unity

Basically the project is divided in two scenes: the training scene, where the model gets trained; and the playing scene, where the trained model can play inifitely. The traininig parameters (generations, population size, batches, etc) can be changed in the editor. The genetic algorithm parameters can be edited in the Evolution class.

## End result

![](preview.gif)