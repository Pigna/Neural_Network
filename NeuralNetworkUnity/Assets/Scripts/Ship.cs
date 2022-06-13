using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour, IComparable<Ship>
{
    Rigidbody rigidbody;

    public NeuralNetwork neuralNetwork;
    public GameObject target;
    public Vector3 oldPos;
    public int identification;

    private Renderer rederer;

    public Gradient gradient;

    private float minDistanceToTarget = 100f;
    public int timesTargetAchieved = 0;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        rederer = GetComponent<Renderer>();

        //Output
        //Movement -> Forward | Positive  -> Backwards | Negative
        //RotationX -> Turn left | Positive -> Turn right | Negative
        //RotationY -> Turn left | Positive -> Turn right | Negative
        //RotationZ -> Turn left | Positive -> Turn right | Negative


        //Input
        //Distance to target - Float
        //Direction to target - XYZ
        //DirectionSelf - XYZ
        //Current speed?

        //Set old location, needed to calculate speed
        oldPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //Get direction to objective area
        Vector3 dir = DirectionToObjective();

        //Get distance to objective area
        float dist = DistanceToObjective();
        UpdateColor(dist);

        //Get current movement speed
        float speed = Speed();

        //List the Input
        float[] input = new float[] {
            dist,
            dir.x, dir.y, dir.z,
            transform.forward.x, transform.forward.y, transform.forward.z,
            speed
        };

        //Call neural network with input
        float[] networkReturn = neuralNetwork.FeedForward(input);//Input

        //Use network output for actions
        Move(networkReturn[0]);
        Rotate(networkReturn[1], networkReturn[2], networkReturn[3]);
    }

    //Fixed update
    void FixedUpdate()
    {
        //Displays a line of the forward direction
        LineRenderer lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.SetVertexCount(2);
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, transform.forward * 2 + transform.position);
    }

    /// <summary>
    /// Constructor
    /// </summary>
    public Ship()
    {
        //Setup neural network
        //The amount of ints will determain the number of layers
        //The value of a layer will determain the number of neurons
        //The first number is the amount of input
        //The last number is the amount of output
        neuralNetwork = new NeuralNetwork(new int[] { 8, 8, 6, 4 });
    }

    /// <summary>
    /// Setup a ship with existing network and mutate
    /// </summary>
    /// <param name="neuralNetwork">NeuralNetwork, Existing NeuralNetwork</param>
    public Ship(NeuralNetwork neuralNetwork)
    {
        this.neuralNetwork = new NeuralNetwork(neuralNetwork);
        this.neuralNetwork.Mutate();
    }

    /// <summary>
    /// Forward (Positive) and backward(Negative) movement of the ship
    /// </summary>
    /// <param name="speed">Float input</param>
    private void Move(float speed)
    {
        rigidbody.velocity = transform.forward * (speed+2f);
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
        transform.Rotate(new Vector3(x, y, z) * Time.deltaTime * 25, Space.World);
    }

    /// <summary>
    /// Get the direction to the objective
    /// </summary>
    /// <returns>Vector3 objective direction</returns>
    private Vector3 DirectionToObjective()
    {
        return Vector3.Normalize(this.transform.position - target.transform.position);
    }

    /// <summary>
    /// Get the distance to the objective
    /// </summary>
    /// <returns>Float objective distance</returns>
    public float DistanceToObjective()
    {
        float _distance = Vector3.Distance(this.transform.position, target.transform.position);

        //Set minimum achieved distance to objective
        if (_distance < minDistanceToTarget)
            minDistanceToTarget = _distance;

        return _distance;
    }

    /// <summary>
    /// Get the current speed of the ship
    /// </summary>
    /// <returns>Float speed</returns>
    private float Speed()
    {
        var speed = Vector3.Distance(oldPos, transform.position);

        oldPos = transform.position;
        return speed;
    }

    public float Score()
    {
        float score = (100f * timesTargetAchieved) + (100f - minDistanceToTarget);
        Debug.Log("Id:" + identification + "Distance to obj: " + DistanceToObjective() + "Min distance: " + minDistanceToTarget + "Score: " + score);
        return score;
    }

    /// <summary>
    /// Script to compare the scores of two ships, used for sorting the list of fitness
    /// </summary>
    /// <param name="other">Ship to compare to.</param>
    /// <returns></returns>
    public int CompareTo(Ship other)
    {
        //Compare the score
        return Score().CompareTo(other.Score());
    }

    /// <summary>
    /// Update the color of the ship depending on the score. Using the range of a gradient.
    /// </summary>
    /// <param name="input">Either Score or distance to target</param>
    private void UpdateColor(float input)
    {
        float gradientPosition = Mathf.InverseLerp(0, 25, input);
        rederer.material.color = gradient.Evaluate(gradientPosition);
    }
}