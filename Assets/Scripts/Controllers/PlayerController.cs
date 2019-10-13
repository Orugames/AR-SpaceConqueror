using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// This class manages the inputs made by the player as selecting planets
/// to then send the information to the BattleManager
/// </summary>
public class PlayerController : MonoBehaviour
{
    public List<PlanetView> planets = new List<PlanetView>();
    public List<PlanetView> playerPlanets = new List<PlanetView>();

    public List<GameObject> planetsGO = new List<GameObject>();

    public List<PlanetView> selectedPlanets = new List<PlanetView>();
    public List<Vector3> selectedPlanetsPositions = new List<Vector3>();

    public PlanetView lastPlanetTouched = null;

    public Vector3 initialTouchPos = Vector3.zero;
    public Vector3 lastTouchPos = Vector3.zero;

    public bool planetTouched;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Input Logic
        UserInputLogic();
    }

    void UserInputLogic()
    {
      
        if ((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) || Input.GetMouseButtonDown(0))
        {
            Debug.Log("Aaaaaa");

            // Raycast & hit elements
            Ray mRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(mRay, out hit))
            {
                // null check & planet tag check
                if (hit.collider == null || hit.collider.tag != "Planet")
                {
                    return;
                }

                PlanetView selectedPlanet = hit.collider.GetComponent<PlanetView>();

                // If it is not a friendlyPlanet, return
                if (!selectedPlanet.planetData.playerControlled)
                {
                    return;
                }


                planetTouched = true;

                Debug.Log("Planet touched");


                // Call the method to create a line Renderer inside the planet touched
                initialTouchPos = hit.collider.transform.position;
                selectedPlanet.CreateLine();
                lastPlanetTouched = selectedPlanet;

                // Update lists
                selectedPlanets.Add(selectedPlanet);
                selectedPlanetsPositions.Add(hit.collider.transform.position);
            }
        }

        else if (((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved) || Input.GetMouseButton(0)) && planetTouched)
        {
            Debug.Log("ccc");

            //Move the position of the line
            Ray mRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            Vector3 touchPos = Input.mousePosition;
            touchPos.z = 1;

            lastTouchPos = Camera.main.ScreenToWorldPoint(touchPos);

            Debug.DrawLine(initialTouchPos, lastTouchPos, Color.white, 2.5f);



            // Update the raycasts positions
            for (int i = 0; i < selectedPlanetsPositions.Count; i++)
            {
                selectedPlanets[i].GetComponent<LineRenderer>().SetPositions(new Vector3[] { selectedPlanetsPositions[i], lastTouchPos });
            }

            //Check if we are touching planet
            if (Physics.Raycast(mRay, out hit))
            {
                // Check if touching another planet not already selected       
                if (hit.collider != null)
                {
                    if (hit.collider.CompareTag("Planet"))
                    {
                        PlanetView selectedPlanet = hit.collider.GetComponent<PlanetView>();

                        // If NOT friendly planet or already moused over
                        if (!selectedPlanet.planetData.playerControlled || selectedPlanet.planetSelectedByPlayer)
                        {
                            return;
                        }

                        lastPlanetTouched = selectedPlanet;
                        selectedPlanet.CreateLine();

                        // Update lists
                        selectedPlanetsPositions.Add(hit.collider.transform.position);
                        selectedPlanets.Add(selectedPlanet);

                    }
                }              
            }

        }
        else if ((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
                || Input.GetMouseButtonUp(0))
        {
            Debug.Log("ddd");

            Ray mRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(mRay, out hit))
            {
                if (hit.collider != null)
                {
                    if (hit.collider.CompareTag("Planet"))
                    {
                        PlanetView receiverPlanet = hit.collider.GetComponent<PlanetView>();

                        // Send order to BattleManager with the neccesary data
                        BattleManager.instance.SendShips(selectedPlanets, receiverPlanet, true);
                    }
                }
            }

            // Erase all lineRenderer Components from selected planets
            foreach (PlanetView planetView in selectedPlanets)
            {
                planetView.planetSelectedByPlayer = false;
                Destroy(planetView.GetComponent<LineRenderer>());
            }

            // Finally, resets all the variables used
            planetTouched = false;
            lastPlanetTouched = null;
            selectedPlanets.Clear();
            selectedPlanetsPositions.Clear();
        }

    }



}