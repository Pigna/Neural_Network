using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{

    public int MaxPopulation = 10;
    public int SimulationTime = 15;
    public GameObject Prefab;

    List<GameObject> Ships = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        MaxPopulation = 10;
        SimulationTime = 15;

        CreateShips();

        StartCoroutine("Mutate");
    }

    // Update is called once per frame
    void Update()
    {
        //Create ships
        //CreateShips();

        //Filter best ones
        //Mutate

    }
    void CreateShips()
    {
        for (int i = 0; i < MaxPopulation; i++)
        {
            //Generate a random coordinate
            Vector3 position = new Vector3(Random.Range(-10, 10), Random.Range(-10, 10), Random.Range(-10, 10));

            //Generate a random rotational axis
            Quaternion rotation = Quaternion.Euler(new Vector3(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360)));

            //Instantiate gameobject on random location with random direction
            GameObject ship = Instantiate(Prefab, position, rotation);

            //Add to ships
            Ships.Add(ship);
            ship.GetComponent<Ship>().Identification = i;
        }
    }

    void RecreateShips(List<GameObject> ships)
    {
        //Foreach remaining ship
        foreach(GameObject ship in ships)
        {
            //Get ship, copy and mutate untill population is filled.
        }
    }

    void DistanceShips()
    {
        Ships.Sort();

        Debug.Log("Results:");
        foreach (GameObject ship in Ships)
        {
            Debug.Log(ship.GetComponent<Ship>().Identification + ": " + ship.GetComponent<Ship>().DistanceToObjective());
        }
    }

    IEnumerator Mutate()
    {
        for (; ; )
        {
            DistanceShips();
            // execute block of code here
            yield return new WaitForSeconds(1.0f);
        }
    }
}
