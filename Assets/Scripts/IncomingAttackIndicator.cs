using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class IncomingAttackIndicator : MonoBehaviour
{
    public int numberOfIncomingShips;
    public PlanetsAlliance incomingShipsAlliance = PlanetsAlliance.enemyControlled;

    public List<Spaceship> incomingSpaceships = new List<Spaceship>();

    // UI elements
    public TextMeshProUGUI numberShipsUIText;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitIndicator(List<Spaceship> incomingSpaceships, PlanetsAlliance incomingShipsAlliance)
    {
        this.incomingSpaceships = incomingSpaceships;
        this.incomingShipsAlliance = incomingShipsAlliance;


    }
}
