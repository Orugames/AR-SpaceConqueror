using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public LevelData levelDataToLoad;
    public GameObject gameCenter;

    public GameObject sunPrefab;

    public GameObject planetsContainer;
    public GameObject sunContainer;


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GenerateLevel(LevelData levelDataSent)
    {
        // Instantiate  the sun
        GameObject newSun = Instantiate(sunPrefab, Vector3.zero, Quaternion.identity);

        newSun.transform.parent = sunContainer.transform;
        newSun.transform.localScale = 0.01f * Vector3.one;
        newSun.transform.localPosition = new Vector3(0, 0.6f, 0);


        // Instantiate the planets in levelData
        for (int i = 0; i < levelDataSent.numberOfPlanets; i++)
        {
            GameObject newPlanet = PlanetFactory.instance.CreateAndInitPlanet(levelDataSent.planetsPosition[i],
                        levelDataSent.planetsShapeSettings[i], levelDataSent.planetsColourSettings[i],
                        levelDataSent.planetsName[i], levelDataSent.planetsScore[i], 0.05f, levelDataSent.planetsAlliance[i],
                        planetsContainer);



        }

        // Send the information to the other controllers
    }
}
