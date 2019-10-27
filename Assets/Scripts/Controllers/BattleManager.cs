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
                GameObject newShip = planetView.spaceshipsOwnedByPlanet[i];

                // Clean each list after the order
                planetView.spaceshipsOwnedByPlanet.Remove(newShip);

                Vector3 p0 = newShip.transform.position;
                Vector3 p1 = planetAttacked.transform.position;

                Vector3[] path = { p0, p1};

                newShip.GetComponent<Spaceship>().MoveToPlanet(planetAttacked,path);

            }




        }

    }
    
    // Method used to inform the enemyAI of own planets after every change of owner
    public void UpdateAIPlanets()
    {
        enemyAIController.UpdatePlanetsList();
    }
}
