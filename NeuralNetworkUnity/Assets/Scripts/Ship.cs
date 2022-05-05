using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    Rigidbody rigidbody;

    public NeuralNetwork neuralNetwork;
    public GameObject Target;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();

        //Setup neural network
        neuralNetwork = new NeuralNetwork(new int[] { 2, 2, 2 });

        //Output
        //Turn left | Positive
        //Turn right | Negative
        //Forward | Positive
        //Backwards | Negative

        //Input
        //Distance to target
        //Direction to target
        //Current speed?
        //

    }

    // Update is called once per frame
    void Update()
    {
        //List the Input
        float[] input = new float[] { 0.0f };

        //Call neural network with input
        float[] networkReturn = neuralNetwork.FeedForward(input);//Input
        
        //Use network output for actions
        Move(networkReturn[0]);
        Rotate(networkReturn[1]);
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
    /// </summary>
    /// <param name="speed"></param>
    private void Rotate(float speed)
    {
        transform.Rotate(new Vector3(0, 1, 0) * Time.deltaTime * speed, Space.World);
    }
}
