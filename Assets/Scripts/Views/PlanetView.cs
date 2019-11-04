using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using DG.Tweening;

public class PlanetView : MonoBehaviour
{

    // The scriptableObject in which we hold the data
    public PlanetData planetData;

    // The list of each Spaceship we hold in this planet
    public List<GameObject> spaceshipsOwnedByPlanet = new List<GameObject>();

    // Planet UI
    public TextMeshProUGUI planetScoreText;
    public TextMeshProUGUI planetNameText;
    public TextMeshProUGUI planetOwnerText;
    public Image UiImage;
    public GameObject triangleIndicator;
    public GameObject UiIndicatorContainer;

    public bool planetSelectedByPlayer;

    bool shipBuilding;

    public float lineWidth = 0.01f;
    // Start is called before the first frame update
    void Start()
    {
        // Init the necessary UI elements of the planet
        InitUiElements();

        // Init the spaceships if the planet is not neutral
        if (planetData.enemyControlled || planetData.playerControlled)
        {
            CreateInitialSpaceships();
        }
    }

    private void InitUiElements()
    {        
        planetScoreText.text = ((int)planetData.score).ToString();

        planetNameText.text = planetData.planetName;

        

        if (planetData.enemyControlled)
        {
            UiImage.color = MaterialsContainer.instance.enemyImageColor;
            planetOwnerText.color = MaterialsContainer.instance.enemyColor;

            // Color the triangle indicator
            triangleIndicator.GetComponent<Renderer>().material = MaterialsContainer.instance.enemyMat;

            planetOwnerText.text = "ENEMY";
        }
        else if (planetData.playerControlled)
        {
            UiImage.color = MaterialsContainer.instance.playerImageColor;
            planetOwnerText.color = MaterialsContainer.instance.playerColor;

            // Color the triangle indicator
            triangleIndicator.GetComponent<Renderer>().material = MaterialsContainer.instance.playerMat;

            planetOwnerText.text = "PLAYER";

        }
        else
        {
            UiImage.color = MaterialsContainer.instance.neutralImageColor;
            planetOwnerText.color = MaterialsContainer.instance.neutralColor;

            // Color the triangle indicator
            triangleIndicator.GetComponent<Renderer>().material = MaterialsContainer.instance.neutralMat;

            planetOwnerText.text = "NEUTRAL";

        }
    }

    private void CreateInitialSpaceships()
    {
        for (int i = 0; i < planetData.score; i++)
        {
            CreateNewSpaceship();

        }

        StartCoroutine(UpdateScoreAndCreateShip());

    }

    private void CreateNewSpaceship()
    {
        GameObject newSpaceship = SpaceshipFactory.instance.CreateSpaceship(this);

        // Rotate a random vector to move the ship, depending on the planet radius
        Vector3 randomRotation = new Vector3(UnityEngine.Random.Range(0, 360f),
                                  UnityEngine.Random.Range(0, 360f),
                                  UnityEngine.Random.Range(0, 360f));

        newSpaceship.transform.position = transform.position;

        newSpaceship.transform.localEulerAngles = randomRotation;

        // Now add the radius depending on the y vector of the ship relative to the radius
        newSpaceship.transform.Translate(newSpaceship.transform.up *
                                          (planetData.planetRadius * UnityEngine.Random.Range(1.4f,1.6f)),
                                          Space.World);


        newSpaceship.GetComponent<Spaceship>().SetShipToRotate(this);
        newSpaceship.GetComponent<Spaceship>().spaceshipData.value = 1;

        spaceshipsOwnedByPlanet.Add(newSpaceship);
    }


    // Update is called once per frame
    void Update()
    {
        if (planetData.enemyControlled || planetData.playerControlled)
        {
            if (!shipBuilding) StartCoroutine(UpdateScoreAndCreateShip());
        }
        // Update view with updated info
        planetScoreText.text = ((int)planetData.score).ToString();

    }

    private IEnumerator UpdateScoreAndCreateShip()
    {
        //yield return new WaitForSeconds((1 / planetData.growthRate) * 2);

        shipBuilding = true;
        while (shipBuilding)
        {
            yield return new WaitForSeconds((1 / planetData.growthRate) * 2);

            planetData.score += 1;         
            CreateNewSpaceship();
            
        }
    }

    public void CreateLine()
    {
        LineRenderer lineRenderer = this.gameObject.AddComponent<LineRenderer>();

        lineRenderer.positionCount = 2;
        lineRenderer.SetPositions(new Vector3[] { transform.position, transform.position });
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;

        lineRenderer.material = MaterialsContainer.instance.lineRendererMat;

        planetSelectedByPlayer = true;
    }


    /// <summary>
    /// Method called whenever a ship collides with a planet
    /// </summary>
    /// <param name="other"></param>
    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("trigger");

        // Do something only if it is a colliding ship
        if (other.GetComponent<Spaceship>())
        {
            Spaceship spaceshipColliding = other.GetComponent<Spaceship>();

            //Check if score is less than 0 so it should change owner
            if (planetData.score == 0)
            {
                SwitchOwnerOfPlanet(spaceshipColliding);

                // Inform battle manager of new owner of planet
                BattleManager.instance.UpdateAIPlanets();
            }

            // If the collision is based on the starting planet or not the planet target
            if (spaceshipColliding.startingPlanet == this || spaceshipColliding.targetPlanet != this)
            {
                return;
            }

            // Change score depeding on ship colliding
            ManageShipCollision(spaceshipColliding);

            // Update the incoming ship indicator relative to this ship
            foreach (IncomingShipsIndicator incShipIndicator in GetComponentsInChildren<IncomingShipsIndicator>())
            {
                if (incShipIndicator == spaceshipColliding.incomingShipUiIndicator)
                {
                    incShipIndicator.numberOfIncShips--;
                    incShipIndicator.UpdateValues();
                }
            }
          
        }
    }
   

    private void ManageShipCollision(Spaceship spaceshipColliding)
    {


        //Keep the trail after collision
        spaceshipColliding.trail.transform.parent = null;

        // If it is an allied spaceship
        if ((spaceshipColliding.spaceshipData.playerControlled && planetData.playerControlled) ||
                        (spaceshipColliding.spaceshipData.enemyControlled && planetData.enemyControlled))
        {
            // Add score depending on the value
            planetData.score += spaceshipColliding.spaceshipData.value;

            //Create a new spaceship
            CreateNewSpaceship();

            // Destroy the ship and all childrens
 
            Destroy(spaceshipColliding.gameObject);
        }
        // Different alliance spaceship colliding
        else if ((spaceshipColliding.spaceshipData.playerControlled && planetData.enemyControlled) ||
                (spaceshipColliding.spaceshipData.enemyControlled && planetData.playerControlled))
        {
            // deduce score depending on the value
            planetData.score -= spaceshipColliding.spaceshipData.value;

            // Remove ship on planet
            Destroy(spaceshipsOwnedByPlanet[0]);
            spaceshipsOwnedByPlanet.Remove(spaceshipsOwnedByPlanet[0]);

            // Destroy the ship
         
            Destroy(spaceshipColliding.gameObject);
        }
        // Neutral planet
        else if (!planetData.enemyControlled && !planetData.playerControlled)
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
        InitUiElements();

        // Reset value of planet to 1
        planetData.score = 0;

    }
}
