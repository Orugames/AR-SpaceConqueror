using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class IncomingShipsIndicator : MonoBehaviour
{
    // Data
    public int numberOfIncShips = 0;
    public List<Spaceship> incomingSpaceships = new List<Spaceship>();
    public PlanetsAlliance incomingShipsAlliance = PlanetsAlliance.neutral;

    // UI elements
    public TextMeshProUGUI numberOfShipsUI;
    public Image shipIcon;
    public Color incShipsColorByAlliance;

    public void InitIndicator(int numberOfIncShips, List<Spaceship> incSpaceships, PlanetsAlliance incShipsAlliance)
    {
        this.numberOfIncShips = numberOfIncShips;
        this.incomingSpaceships = incSpaceships;
        this.incomingShipsAlliance = incShipsAlliance;

        numberOfShipsUI.text = numberOfIncShips.ToString();
        
        if(incShipsAlliance == PlanetsAlliance.playerControlled)
        {
            incShipsColorByAlliance = MaterialsContainer.instance.playerColor;
        }
        else
        {
            incShipsColorByAlliance = MaterialsContainer.instance.enemyColor;

        }
        shipIcon.color = incShipsColorByAlliance;

        if (this.numberOfIncShips <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    // Update is called once per frame
    public void UpdateValues()
    {
        if (numberOfIncShips <= 0)
        {
            Destroy(this.gameObject);
        }
        numberOfShipsUI.text = numberOfIncShips.ToString();
    }
}
