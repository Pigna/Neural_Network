using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{

    public int MaxPopulation = 10;
    public int SimulationTime = 15;

    // Start is called before the first frame update
    void Start()
    {
        MaxPopulation = 10;
        SimulationTime = 15;
    }

    // Update is called once per frame
    void Update()
    {
        //Create ships
        CreateShips();

        //Filter best ones
        //Mutate

    }

    void CreateShips()
    {

    }
}
