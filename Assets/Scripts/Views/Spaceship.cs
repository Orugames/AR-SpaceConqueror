﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Spaceship : MonoBehaviour
{
    public SpaceshipData spaceshipData;
    public GameObject spaceshipMesh;
    ShipAvoidanceLogic shipAvoidanceLogic;

    public TrailRenderer trail;

    public PlanetView startingPlanet;
    public PlanetView targetPlanet;

    public bool attackOrder;
    public bool rotateOrder;

    public IncomingShipsIndicator incomingShipUiIndicator;

    public List<Material> shipMaterials;

    // Start is called before the first frame update
    void Start()
    {
        shipAvoidanceLogic = GetComponent<ShipAvoidanceLogic>();

        // Store all the materials on this ship to iterate over them
        Material[] materialsArray = spaceshipMesh.GetComponent<MeshRenderer>().materials;
        shipMaterials = new List<Material>(materialsArray);

        StartCoroutine(shipAvoidanceLogic.UndissolveShip(shipMaterials, trail));


    }

    // Update is called once per frame
    void Update()
    {
        if (rotateOrder)
        {
            transform.RotateAround(startingPlanet.transform.position, transform.right, 40 * Time.deltaTime);

            //transform.Translate(transform.forward * spaceshipData.speed * Time.deltaTime, Space.World);
        }
    }

    public void MoveToPlanet(PlanetView planetAttacked, Vector3[] path)
    {

        targetPlanet = planetAttacked;
        // Get the time to get to last position, time = distance / speed
        float timeToGoTowardsPlanet = Vector3.Distance(path[1], path[0])/spaceshipData.speed;

        // Bezier movement towards planet
        //transform.LookAt(planetView.transform.position);
        //attackOrder = true;
        shipAvoidanceLogic.nativePlanet = startingPlanet.gameObject;
        shipAvoidanceLogic.target = planetAttacked.gameObject;

        // Start the coroutine to attack
        StartCoroutine(shipAvoidanceLogic.CheckIfNotCollidingWithPlanet());

        // To move at the same  speed regarding different distances, is just time = distance / speed
        //transform.DOPath(path, timeToGoTowardsPlanet, PathType.CatmullRom,PathMode.Full3D,25).SetEase(Ease.Linear).SetLookAt(0);
    }
    private void OnDestroy()
    {
        if (trail.transform.parent != null) trail.transform.parent = null;

        trail.autodestruct = true;
    }
    public void SetShipToRotate(PlanetView startingPlanet)
    {
        this.startingPlanet = startingPlanet;
        rotateOrder = true;
    }

    public void DissolveAndDestroy()
    {
       StartCoroutine(shipAvoidanceLogic.DissolveAndDestroy(shipMaterials));
    }


}
