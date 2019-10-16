using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class PlanetView : MonoBehaviour
{

    // The scriptableObject in which we hold the data
    public PlanetData planetData;

    public TextMeshPro scoreText;

    public bool planetSelectedByPlayer;

    public float lineWidth = 0.01f;
    // Start is called before the first frame update
    void Start()
    {
        scoreText.text = ((int)planetData.score).ToString();
        UpdatePlanetMaterial();
    }

    private void UpdatePlanetMaterial()
    {
        /*if (planetData.enemyControlled)
        {
            GetComponent<Renderer>().material = MaterialsContainer.instance.enemyMat;
        }
        else if (planetData.playerControlled)
        {
            GetComponent<Renderer>().material = MaterialsContainer.instance.friendlyMat;

        }
        else
        {
            GetComponent<Renderer>().material = MaterialsContainer.instance.neutralMat;

        }*/
    }

    // Update is called once per frame
    void Update()
    {
      
        // Update planet score if controlled     
        if (planetData.enemyControlled  || planetData.playerControlled)
        {
            planetData.score += Time.deltaTime / 2;
        }
        // Update view with updated info
        scoreText.text = ((int)planetData.score).ToString();

    }

 

    public void CreateLine()
    {
        LineRenderer lineRenderer = this.gameObject.AddComponent<LineRenderer>();

        lineRenderer.positionCount = 2;
        lineRenderer.SetPositions(new Vector3[] { transform.position, transform.position });
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;

        planetSelectedByPlayer = true;
    }


    /// <summary>
    /// Method called whenever a ship collides with a planet
    /// </summary>
    /// <param name="other"></param>
    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("trigger");
        if (other.GetComponent<Spaceship>())
        {
            Spaceship spaceshipColliding = other.GetComponent<Spaceship>();

            // If the collision is based on the starting planet, do nothing
            if (spaceshipColliding.startingPlanet == this)
            {
                return;
            }

            // Change score depeding on ship colliding
            ManageShipCollision(spaceshipColliding);

            //Check if score is less than 0 so it should change owner
            if (planetData.score <= 0)
            {
                SwitchOwnerOfPlanet(spaceshipColliding);

                // Inform battle manager of new owner of planet
                BattleManager.instance.UpdateAIPlanets();
            }
        }
    }
   

    private void ManageShipCollision(Spaceship spaceshipColliding)
    {
        // If it is an allied spaceship
        if ((spaceshipColliding.spaceshipData.playerControlled && planetData.playerControlled) ||
                        (spaceshipColliding.spaceshipData.enemyControlled && planetData.enemyControlled))
        {
            // Add score depending on the value
            planetData.score += spaceshipColliding.spaceshipData.value;

            // Destroy the ship
            Destroy(spaceshipColliding.gameObject);
        }
        // Different alliance spaceship colliding OR neutral planet
        else if ((spaceshipColliding.spaceshipData.playerControlled && planetData.enemyControlled) ||
                (spaceshipColliding.spaceshipData.enemyControlled && planetData.playerControlled) ||
                (!planetData.enemyControlled && !planetData.playerControlled))
        {
            // deduce score depending on the value
            planetData.score -= spaceshipColliding.spaceshipData.value;

            // Destroy the ship
            Destroy(spaceshipColliding.gameObject);

        }
    }
    private void SwitchOwnerOfPlanet(Spaceship spaceshipColliding)
    {
        // If it was out planet, switch to enemyPlanet
        if (planetData.playerControlled)
        {
            planetData.playerControlled = false;
            planetData.enemyControlled = true;
        }
        // Enemy planet to player
        else if (planetData.enemyControlled)
        {
            planetData.playerControlled = true;
            planetData.enemyControlled = false;
        }
        // Neutral planet
        else if (!planetData.enemyControlled && !planetData.playerControlled)
        {
            // Enemy ship, enemy planet
            if (spaceshipColliding.spaceshipData.enemyControlled)
            {
                planetData.playerControlled = false;
                planetData.enemyControlled = true;
            }else 
            {
                planetData.playerControlled = true;
                planetData.enemyControlled = false;
            }
        }

        // Change  material
        UpdatePlanetMaterial();

        // Reset value of planet to 1
        planetData.score = 1;

    }
}
