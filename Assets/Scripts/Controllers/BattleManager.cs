using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// This class is used to manage the given orders to this class into actions in the battle
/// as sending ships from planets to anothers
/// </summary>
public class BattleManager : MonoBehaviour
{

    public static BattleManager instance = null;

    public EnemyAIController enemyAIController = null;

    public GameObject shipsContainer;

    public GameObject playerSpaceshipPrefab;

    public GameObject enemySpaceshipPrefab;



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


    /// <summary>
    /// This method is used to receive the list of planets to send order to attack/aid the corresponding planet
    /// </summary>
    /// <param name="planetsAttacking"></param>
    /// <param name="planetAttacked"></param>
    public void SendShips(List<PlanetView> planetsAttacking, PlanetView planetAttacked, bool orderGivenByPlayer)
    {
        // Send spaceships from their planet to the attacked one
        foreach (PlanetView planetView in planetsAttacking)
        {
            // If it is its own planet, do nothing
            if (planetView == planetAttacked)
            {
                return;
            }

            // Cut in half the score of each planet
            planetView.planetData.score /= 2;

            // Send the number according to the half of the score of the planet
            for (int i = 0; i < planetView.planetData.score; i++)
            {

                // Tell the ship if it is an enemy ship or allied One
                GameObject spaceshipPrefab;
                if (orderGivenByPlayer) spaceshipPrefab = playerSpaceshipPrefab;
                else spaceshipPrefab = enemySpaceshipPrefab;

                // Instantiate
                GameObject newShip = Instantiate(spaceshipPrefab, shipsContainer.transform);

                // Ship positioning
                NewShipPositioning(planetAttacked, planetView, i, newShip);

                // Send the ship the destination
                newShip.GetComponent<Spaceship>().startingPlanet = planetView;
                newShip.GetComponent<Spaceship>().MoveToPlanet(planetAttacked);

            }


        }

    }

    private static void NewShipPositioning(PlanetView planetAttacked, PlanetView planetView, int i, GameObject newShip)
    {
        // Start at the center of the planet
        newShip.transform.position = planetView.transform.position;

        // Face planet
        newShip.transform.LookAt(planetAttacked.transform);
        //Rotate initial ship pos according to number of ships
        newShip.transform.Rotate(newShip.transform.forward, (360 / planetView.planetData.score) * i, Space.World);

        // Move the positions out of the radius of the planet
        newShip.transform.position += newShip.transform.up * planetView.planetData.planetRadius;
    }

    // Method used to inform the enemyAI of own planets after every change of owner
    public void UpdateAIPlanets()
    {
        enemyAIController.UpdatePlanetsList();
    }
}
