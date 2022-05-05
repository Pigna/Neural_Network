using System;
using System.Collections.Generic;

public class NeuralNetwork
{
    private int[] layers;
    private float[][] neurons;
    private float[][][] weights;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="layers"></param>
    public NeuralNetwork(int[] layers)
    {
        this.layers = new int[layers.Length];
        for (int i = 0; i < layers.Length; i++)
        { 
            this.layers[i] = layers[i];
        }

        InitNeurons();
        InitWeights();
    }

    /// <summary>
    /// Copy the network
    /// </summary>
    /// <param name="copyNetwork">Base network</param>
    public NeuralNetwork(NeuralNetwork copyNetwork)
    {
        this.layers = new int[copyNetwork.layers.Length];
        for (int i = 0; i < copyNetwork.layers.Length; i++)
        {
            this.layers[i] = copyNetwork.layers[i];
        }
        InitNeurons();
        InitWeights();
        CopyWeights(copyNetwork.weights);
    }

    /// <summary>
    /// Copy the weights
    /// </summary>
    /// <param name="copyWeights">Base weights</param>
    private void CopyWeights(float[][][] copyWeights)
    {
        for (int i = 0; i < weights.Length; i++)
        {
            for (int j = 0; j < weights[i].Length; j++)
            {
                for (int k = 0; k < weights[i][j].Length; k++)
                {
                    weights[i][j][k] = copyWeights[i][j][k];
                }
            }
        }
    }

    /// <summary>
    /// Initializer of the Nuerons
    /// </summary>
    private void InitNeurons()
    {
        List<float[]> neuronsList = new List<float[]>();
        for (int i = 0; i < layers.Length; i++)
        {
            neuronsList.Add(new float[layers[i]]);
        }
        neurons = neuronsList.ToArray();
    }

    /// <summary>
    /// Creates the Weights layer
    /// </summary>
    private void InitWeights()
    {
        List<float[][]> weightsList = new List<float[][]>();

        for (int i = 1; i < layers.Length; i++)
        {
            List<float[]> layerWeightList = new List<float[]>();

            int neuronsInPreviousLayer = layers[i - 1];

            for(int j = 0; j < neurons[i].Length; j++)
            {
                float[] neuronWeights = new float[neuronsInPreviousLayer];

                for (int k = 0; k < neuronsInPreviousLayer; k++)
                {
                    neuronWeights[k] = UnityEngine.Random.Range(-0.5f, 0.5f);
                }
                layerWeightList.Add(neuronWeights);
            }
            weightsList.Add(layerWeightList.ToArray());
        }
        weights = weightsList.ToArray();
    }

    /// <summary>
    /// Feed forward the neural network with given input
    /// </summary>
    /// <param name="inputs">Inputs to the network</param>
    /// <returns></returns>
     public float[] FeedForward(float[] inputs)
    {
        //Add Input to the Neuron matrix
        for (int i = 0; i < inputs.Length; i++)
        {
            neurons[0][i] = inputs[i];
        }

        //Iterate over the neurons and compute feedforward
        for (int i = 0; i < layers.Length; i++)
        {
            for (int j = 0; j < neurons[i].Length; j++)
            {
                float value = 0.25f;

                for (int k = 0; k < neurons[i - 1].Length; k++)
                {
                    value += weights[i - 1][j][k] * neurons[i - 1][k];
                }
                neurons[i][j] = (float)Math.Tanh(value);
            }
        }
        //Return output
        return neurons[neurons.Length - 1];
    }

    /// <summary>
    /// Mutate
    /// </summary>
    public void Mutate()
    {
        for (int i = 0; i < weights.Length; i++)
        {
            for (int j = 0; j < weights[i].Length; j++)
            {
                for (int k = 0; k < weights[i][j].Length; k++)
                {
                    float weight = weights[i][j][k];

                    float randomNumber = UnityEngine.Random.Range(-0.5f, 0.5f) * 1000f;

                    if(randomNumber <= 2f)
                    {
                        //Flip sign of weight
                        weight *= -1f;
                    }
                    else if(randomNumber <= 4f)
                    {
                        //Random weight
                        weight = UnityEngine.Random.Range(-0.5f, 0.5f);
                    }
                    else if (randomNumber <= 6f)
                    {
                        //Increase by %
                        float factor = UnityEngine.Random.Range(0f, 1f) + 1f;
                        weight *= factor;
                    }
                    else if (randomNumber <= 8f)
                    {
                        //Decrease by %
                        float factor = UnityEngine.Random.Range(-0f, 1f);
                        weight *= factor;
                    }

                    weights[i][j][k] = weight;
                }
            }
        }
    }
}
