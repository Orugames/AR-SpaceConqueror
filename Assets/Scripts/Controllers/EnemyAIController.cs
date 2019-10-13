using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAIController : MonoBehaviour
{

    public List<PlanetView> planets = new List<PlanetView>();
    public List<PlanetView> enemyPlanets = new List<PlanetView>();
    public List<PlanetView> nonEnemyPlanets = new List<PlanetView>();

    public List<GameObject> planetsGO = new List<GameObject>();

    public GameObject allPlanetsContainer;

    public float AITurnTime;

    public bool battleStarted;
    public bool AIDecisionMakingRunning;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (battleStarted)
        {
            battleStarted = false;
            AIDecisionMakingRunning = true;

            UpdatePlanetsList();

            StartCoroutine(EnemyAITurn());
        }
    }
    public IEnumerator EnemyAITurn()
    {
        while (AIDecisionMakingRunning || enemyPlanets.Count == 0)
        {
            //Find my strongest Planet
            PlanetView source = null;
            float sourceScore = 0;

            // Find my strongest planet
            foreach (PlanetView planetView in enemyPlanets)
            {
                // Get the score of the planet relative to the current power and the growth rate 
                //
                //    x                1
                // -------   =   --------------
                // x + 1/Gr      1 + 1/(Gr * x)
                //
                float score = 1 / (1 + (1 / planetView.planetData.growthRate * planetView.planetData.score));

                // if this new planet is better, chose it
                if (score > sourceScore && planetView.planetData.score > 8)
                {
                    sourceScore = score;
                    source = planetView;
                }

            }

            //Then find the weakest friendly or neutral planet with best growth rate

            //
            //         1
            //   --------------
            //      1 + Gr * x
            //

            PlanetView dest = null;
            float destScore = 0;
            foreach (PlanetView planetView in nonEnemyPlanets)
            {
                float score = 1 / (1 + (planetView.planetData.growthRate * planetView.planetData.score));
                if (score > destScore)
                {
                    destScore = score;
                    dest = planetView;
                }
            }

            // Finally take the decision to attack

            // null check
            if (dest != null && source != null)
            {
                Debug.Log(dest.name + source.name);
                // If we pass the minimun requirements, send the order to attack
                if ((source.planetData.score > dest.planetData.score * 1.75f && dest.planetData.playerControlled) ||
                    (source.planetData.score > dest.planetData.score * 1.15f && !dest.planetData.playerControlled))
                {
                    Debug.Log("ATTACK ORDER ENEMY AI");

                    List<PlanetView> sourceAsList = new List<PlanetView>();
                    sourceAsList.Add(source);

                    BattleManager.instance.SendShips(sourceAsList, dest, false);
                }
            }

            yield return new WaitForSeconds(AITurnTime);

        }

    }

    public void UpdatePlanetsList()
    {
        // get a list of every planet ingame
        List<PlanetView> numberOfPlanets = new List<PlanetView>(allPlanetsContainer.GetComponentsInChildren<PlanetView>());

        // Get planets flagged as enemyPlanets and not as
        foreach (PlanetView planetView in numberOfPlanets)
        {
            // enemy planets
            if (planetView.planetData.enemyControlled && !enemyPlanets.Contains(planetView))
            {
                enemyPlanets.Add(planetView);
            }

            // neutral and friendly ones
            if (!planetView.planetData.enemyControlled && !nonEnemyPlanets.Contains(planetView))
            {
                nonEnemyPlanets.Add(planetView);
            }
        }

        // Remove all planets that are not from  the enemy on the list of planets
        for (int i = enemyPlanets.Count - 1; i >= 0; i--)
        {
            if (!enemyPlanets[i].planetData.enemyControlled) enemyPlanets.RemoveAt(i);
        }

        // Remove all planets that are not from  the non enemy on the list of planets
        for (int i = nonEnemyPlanets.Count - 1; i >= 0; i--)
        {
            if (nonEnemyPlanets[i].planetData.enemyControlled) nonEnemyPlanets.RemoveAt(i);
        }
    }
}
