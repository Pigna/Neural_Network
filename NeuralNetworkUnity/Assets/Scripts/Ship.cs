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
        
        //Output
        //Turn left | Positive
        //Turn right | Negative
        //Forward | Positive
        //Backwards | Negative

    }

    // Update is called once per frame
    void Update()
    {
        
        //Call neural network
    }

    private void Move(float speed)
    {
        rigidbody.velocity = transform.forward * speed;
    }

    private void Rotate(float speed)
    {
        transform.Rotate(new Vector3(0, 1, 0) * Time.deltaTime * speed, Space.World);
    }
}
