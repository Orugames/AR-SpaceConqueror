using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Spaceship : MonoBehaviour
{
    public SpaceshipData spaceshipData;

    public TrailRenderer trail;

    public PlanetView startingPlanet;

    bool moveOrder;
    public bool rotateOrder;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (rotateOrder)
        {
            transform.RotateAround(startingPlanet.transform.position, transform.right, 40 * Time.deltaTime);

            //transform.Translate(transform.forward * spaceshipData.speed * Time.deltaTime, Space.World);
        } else if (moveOrder)
        {

        }
    }

    public void MoveToPlanet(PlanetView planetView, Vector3[] path)
    {
        rotateOrder = false;
        // Get the time to get to last position, time = distance / speed
        float timeToGoTowardsPlanet = Vector3.Distance(path[1], path[0])/spaceshipData.speed;

        // Bezier movement towards planet
        //transform.LookAt(planetView.transform.position);
        moveOrder = true;

        // To move at the same  speed regarding different distances, is just time = distance / speed
        transform.DOPath(path, timeToGoTowardsPlanet, PathType.CatmullRom,PathMode.Full3D,25).SetEase(Ease.Linear).SetLookAt(0);
    }
    private void OnDestroy()
    {
        trail.transform.parent = null;
        trail.autodestruct = true;
        trail = null;
    }
    public void SetShipToRotate(PlanetView startingPlanet)
    {
        this.startingPlanet = startingPlanet;
        rotateOrder = true;
    }
}
