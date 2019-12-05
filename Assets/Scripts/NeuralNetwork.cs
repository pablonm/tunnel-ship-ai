using UnityEngine;
using System;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

public class NeuralNetwork
{
    int inputSize, hiddenLayerSize, outputSize;
    Matrix<double> hiddenWeights, outputWeights;
    Vector<double> hiddenBias, outputBias;

    public NeuralNetwork(int input, int hidden, int output)
    {
        inputSize = input;
        hiddenLayerSize = hidden;
        outputSize = output;
        
        hiddenWeights = DenseMatrix.Build.Random(inputSize, hiddenLayerSize);
        outputWeights = DenseMatrix.Build.Random(hiddenLayerSize, outputSize);

        hiddenBias = DenseVector.Build.Random(hiddenLayerSize);
        outputBias = DenseVector.Build.Random(outputSize);

        SetWeightsAndBias(GetWeightsAndBias());
    }

    public Vector<double> Think(Vector<double> inputs)
    {
        Vector<double> z = _Relu(inputs * hiddenWeights + hiddenBias);
        return _Softmax(z * outputWeights + outputBias);
    }

    public Vector<double> GetWeightsAndBias()
    {
        return _MergeNVectors(
            hiddenWeights.ReduceRows(_Merge2Vectors),
            outputWeights.ReduceRows(_Merge2Vectors),
            hiddenBias,
            outputBias
            );
    }

    public void SetWeightsAndBias(Vector<double> vector)
    {
        int hiddenWeightsSize = inputSize * hiddenLayerSize;
        int outputWeightsSize = hiddenLayerSize * outputSize;
        Vector<double> hiddenWeightsVector = vector.SubVector(0, hiddenWeightsSize);
        Vector<double> outputWeightsVector = vector.SubVector(hiddenWeightsSize, outputWeightsSize);
        hiddenWeights = DenseMatrix.Build.Dense(inputSize, hiddenLayerSize, (i, j) => hiddenWeightsVector[i * hiddenLayerSize + j]);
        outputWeights = DenseMatrix.Build.Dense(hiddenLayerSize, outputSize, (i, j) => outputWeightsVector[i * outputSize + j]);
        hiddenBias = vector.SubVector(hiddenWeightsSize + outputWeightsSize, hiddenLayerSize);
        outputBias = vector.SubVector(hiddenWeightsSize + outputWeightsSize + hiddenLayerSize, outputSize);
    }

    private Vector<double> _Relu(Vector<double> vector)
    {
        return vector.PointwiseMaximum(0);
    }

    private Vector<double> _Softmax(Vector<double> vector)
    {
        Vector<double> vecExp = DenseVector.Build.Dense(vector.Count, i => Math.Exp(vector[i]));
        double vecExpSum = vecExp.Sum();
        return DenseVector.Build.Dense(vecExp.Count, i => vecExp[i] / vecExpSum);
    }

    private Vector<double> _Merge2Vectors(Vector<double> a, Vector<double> b)
    {
        return a.ToColumnMatrix().Stack(b.ToColumnMatrix()).Column(0);
    }

    private Vector<double> _MergeNVectors(params Vector<double>[] vectors)
    {
        Vector<double> concat = null;
        foreach (Vector<double> vector in vectors)
            if (concat != null) concat = _Merge2Vectors(concat, vector);
            else concat = vector;
        return concat;
    }
}
