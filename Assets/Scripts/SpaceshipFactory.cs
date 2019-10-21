using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipFactory : MonoBehaviour
{
    public static SpaceshipFactory instance = null;

    public GameObject spaceshipContainer;


    public GameObject playerSpaceshipPrefab;

    public GameObject enemySpaceshipPrefab;

    public GameObject spaceshipPrefabSelected;
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

    public GameObject CreateSpaceship(PlanetView planetViewOwner)
    {
        GameObject newSpaceship;

        if (planetViewOwner.planetData.playerControlled)
        {
            spaceshipPrefabSelected = playerSpaceshipPrefab;
        }
        else if (planetViewOwner.planetData.enemyControlled)
        {
            spaceshipPrefabSelected = enemySpaceshipPrefab;
        }

        if (spaceshipPrefabSelected == null)
        {
            return null;
        }

        newSpaceship = GameObject.Instantiate(spaceshipPrefabSelected,Vector3.zero,Quaternion.identity, spaceshipContainer.transform);


        return newSpaceship;
    }
}
