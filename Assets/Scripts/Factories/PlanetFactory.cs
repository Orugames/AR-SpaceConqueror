using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetFactory : MonoBehaviour
{
    public static PlanetFactory instance = null;

    [Range(0,100)]
    public int planetsResolution;

    public GameObject planetPrefab;
    
    void Awake()
    {
        //Check if instance already exists
        if (instance == null)

            //if not, set instance to this
            instance = this;

        //If instance already exists and it's not this:
        else if (instance != this)

            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);

    }

    public GameObject CreateAndInitPlanet(Vector3 planetPosition, ShapeSettings shapeSettings, ColourSettings colourSettings,
                                    string name, int score, float radius, PlanetsAlliance planetAlliance, GameObject parentGO)
    {
        GameObject newPlanet;

        // Instantiate
        newPlanet = GameObject.Instantiate(planetPrefab, Vector3.zero, Quaternion.identity, parentGO.transform);

        // Move
        newPlanet.transform.localPosition = planetPosition;

        // set the data on its planetData script
        PlanetData newPlanetData = newPlanet.GetComponent<PlanetData>();
        newPlanetData.name = name;
        newPlanetData.score = score;
        newPlanetData.planetRadius = radius;
        newPlanetData.growthRate = 1;

        // ALLIANCE, needs massive refactor
        if (planetAlliance == PlanetsAlliance.playerControlled)
        {
            newPlanetData.playerControlled = true;
        } 
        else if (planetAlliance == PlanetsAlliance.enemyControlled)
        {
            newPlanetData.enemyControlled = true;
        }

        // Then generate the planet mesh with the shape and colour  settings sent
        Planet planetGenerator = newPlanet.GetComponentInChildren<Planet>();
        planetGenerator.shapeSettings = shapeSettings;
        planetGenerator.colourSettings = colourSettings;
        //planetGenerator.resolution = planetsResolution;


        planetGenerator.GeneratePlanet();


        return newPlanet;
    }
}
