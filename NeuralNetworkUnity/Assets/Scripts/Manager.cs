using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{

    public int MaxPopulation = 100;
    
    private int CurrentGen = 0;
    public int TimesObjectivesAchieved = 0;

    public float SimulationTime = 15.0f;
    private float ActionTime = 15.0f;

    public GameObject ship;

    List<GameObject> Ships = new List<GameObject>();

    UiController uiController;

    // Start is called before the first frame update
    void Start()
    {
        CreateShips();
        uiController = GameObject.Find("Canvas").GetComponent<UiController>();
    }

    private void CreateTarget()
    {
        UnityEngine.Random.InitState(11081993+TimesObjectivesAchieved);
        Vector3 position = new Vector3(RandomNumber(-5f, 5f, 5f), RandomNumber(-5f, 5f, 5f), RandomNumber(-5f, 5f, 5f));
        transform.position = position;
    }

    private float RandomNumber(float min, float max, float modifier)
    {
        float r = UnityEngine.Random.Range(min, max);
        if (r < 0)
            r -= modifier;
        else if (r > 0)
            r += modifier;
        return r;
    }

    // Update is called once per frame
    void Update()
    {
        //Every 15 sec filter the best performing and recreate
        if (Time.time > ActionTime)
        {
            ActionTime = Time.time + SimulationTime;
            CurrentGen++;
            uiController.UpdateGenNr(CurrentGen);
            FitnessTest();
        }
    }

    void CreateShips()
    {
        CreateTarget();
        for (int i = 0; i < MaxPopulation; i++)
        {
            //Instantiate gameobject on random location with random direction
            GameObject go_ship = InstantiateShip();

            //Add to ships
            Ships.Add(go_ship);

            Ship ship = go_ship.GetComponent<Ship>();
            ship.identification = i;
            ship.target = this.gameObject;
        }
    }

    void RecreateShips(List<Ship> ships)
    {
        CreateTarget();
        List<GameObject> newGen = new List<GameObject>();
        
        //Foreach fittest ship
        foreach(Ship ship in ships)
        {
            //Get ship, copy and mutate untill population is filled.
            Ship newShip = new Ship(ship.neuralNetwork);

            GameObject _newObj = InstantiateShip();
            _newObj.GetComponent<Ship>().target = this.gameObject;

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
        _ships.Reverse();
        
        //Get 50% closest ships
        List<Ship> fittest = _ships.GetRange(0, MaxPopulation / 2);

        //Add 
        fittest.AddRange(_ships.GetRange(0, MaxPopulation / 2));

        //Recreate 
        RecreateShips(fittest);
    }

    /// <summary>
    /// Destroy the existing generation
    /// </summary>
    void ClearExistingGen()
    {
        foreach (GameObject gameObject in Ships)
        {
            Destroy(gameObject);
        }
        Ships.Clear();
    }

    /// <summary>
    /// Create a ship
    /// </summary>
    /// <returns>Gameobject Ship</returns>
    GameObject InstantiateShip()
    {
        //Generate a random coordinate
        Vector3 position = new Vector3(0, 0, 0);

        //Generate a random rotational axis
        Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, 0));

        //Instantiate gameobject on random location with random direction
        return Instantiate(ship, position, rotation);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collision");
        TimesObjectivesAchieved++;
        other.GetComponent<Ship>().timesTargetAchieved++;
    }
}
