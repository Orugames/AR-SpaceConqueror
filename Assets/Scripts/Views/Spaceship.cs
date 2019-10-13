using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spaceship : MonoBehaviour
{
    public SpaceshipData spaceshipData;

    public PlanetView startingPlanet;

    bool moveOrder;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (moveOrder)
        {
            transform.Translate(transform.forward * spaceshipData.speed * Time.deltaTime, Space.World);
        }
    }

    public void MoveToPlanet(PlanetView planetView)
    {
        // Bezier movement towards planet

        transform.LookAt(planetView.transform.position);
        moveOrder = true;
    }
}
