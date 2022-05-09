﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    Rigidbody rigidbody;

    public NeuralNetwork neuralNetwork;
    public GameObject Target;

    private float q = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();

        //Setup neural network
        //The amount of ints will determain the number of layers
        //The value of a layer will determain the number of neurons
        //The first number is the amount of input
        //The last number is the amount of output
        neuralNetwork = new NeuralNetwork(new int[] { 4, 2, 4 });

        //Output
        //Movement -> Forward | Positive  -> Backwards | Negative
        //RotationX -> Turn left | Positive -> Turn right | Negative
        //RotationY -> Turn left | Positive -> Turn right | Negative
        //RotationZ -> Turn left | Positive -> Turn right | Negative


        //Input
        //Distance to target - Float
        //Direction to target - XYZ
        //
        //DirectionSelf - XYZ
        //Current speed?

    }

    // Update is called once per frame
    void Update()
    {
        //Get direction to objective area
        Vector3 dir = DirectionToObjective();

        //Get distance to objective area
        float dist = DistanceToObjective();

        //List the Input
        float[] input = new float[] { dist, dir.x, dir.y, dir.z };

        //Call neural network with input
        float[] networkReturn = neuralNetwork.FeedForward(input);//Input
        
        //Use network output for actions
        Move(networkReturn[0]);
        Rotate(networkReturn[1], networkReturn[2], networkReturn[3]);
    }

    void FixedUpdate()
    {
        //Displays a line of the forward direction
        LineRenderer lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.SetVertexCount(2);
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, transform.forward * 2 + transform.position);
    }

    /// <summary>
    /// Forward (Positive) and backward(Negative) movement of the ship
    /// </summary>
    /// <param name="speed">Float input</param>
    private void Move(float speed)
    {
        rigidbody.velocity = transform.forward * speed;
    }

    /// <summary>
    /// Rotation of the ship, Left (Negative) and Right (Positive)
    /// X, Y, Z
    /// </summary>
    /// <param name="x">Float, X axis</param>
    /// <param name="y">Float, Y axis</param>
    /// <param name="z">Float, Z axis</param>
    private void Rotate(float x, float y, float z)
    {
        transform.Rotate(new Vector3(x, y, z) * Time.deltaTime, Space.World);
    }

    /// <summary>
    /// Get the direction to the objective
    /// </summary>
    /// <returns>Vector3 objective direction</returns>
    private Vector3 DirectionToObjective()
    {
        return Vector3.Normalize(this.transform.position - Target.transform.position);
    }

    /// <summary>
    /// Get the distance to the objective
    /// </summary>
    /// <returns>Float objective distance</returns>
    private float DistanceToObjective()
    {
        return Vector3.Distance(this.transform.position, Target.transform.position);
    }
}
