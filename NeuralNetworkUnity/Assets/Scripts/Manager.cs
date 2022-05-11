using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{

    public int MaxPopulation = 10;
    
    private int CurrentGen = 0;

    public float SimulationTime = 15.0f;
    private float ActionTime = 0.0f;

    public GameObject Prefab;

    List<GameObject> Ships = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        MaxPopulation = 10;
        SimulationTime = 15f;

        CreateShips();
    }

    // Update is called once per frame
    void Update()
    {
        //Every 15 sec filter the best performing and recreate
        if (Time.time > ActionTime)
        {
            ActionTime = Time.time + SimulationTime;
            FitnessTest();
        }
    }
    void CreateShips()
    {
        for (int i = 0; i < MaxPopulation; i++)
        {
            //Instantiate gameobject on random location with random direction
            GameObject ship = InstantiateShip();

            //Add to ships
            Ships.Add(ship);

            ship.GetComponent<Ship>().Identification = i;
        }
    }

    void RecreateShips(List<Ship> ships)
    {
        List<GameObject> newGen = new List<GameObject>();
        
        //Foreach fittest ship
        foreach(Ship ship in ships)
        {
            //Get ship, copy and mutate untill population is filled.
            Ship newShip = new Ship(ship.neuralNetwork);

            GameObject _newObj = InstantiateShip();

            newGen.Add(_newObj);
        }
        //Clean-up existing gen
        ClearExistingGen();

        //Set new gen
        Ships = newGen;
    }

    void FitnessTest()
    {
        //Get ship component for all ships
        List<Ship> _ships = new List<Ship>();
        foreach (GameObject gameObject in Ships)
        {
            _ships.Add(gameObject.GetComponent<Ship>());
        }
        //Sort ships on distance to objective
        _ships.Sort();
        
        //Get 50% best performing ships
        List<Ship> fittest = _ships.GetRange(0, MaxPopulation / 2);

        //Recreate 
        RecreateShips(fittest);
    }

    void ClearExistingGen()
    {
        foreach (GameObject gameObject in Ships)
        {
            Destroy(gameObject);
        }
        Ships.Clear();
    }

    GameObject InstantiateShip()
    {
        //Generate a random coordinate
        Vector3 position = new Vector3(Random.Range(-10, 10), Random.Range(-10, 10), Random.Range(-10, 10));

        //Generate a random rotational axis
        Quaternion rotation = Quaternion.Euler(new Vector3(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360)));

        //Instantiate gameobject on random location with random direction
        return Instantiate(Prefab, position, rotation);
    }
}
