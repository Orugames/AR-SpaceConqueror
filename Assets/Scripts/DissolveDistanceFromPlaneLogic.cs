using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissolveDistanceFromPlaneLogic : MonoBehaviour
{
    public GameObject sphere;
    public GameObject plane;

    public Material sphereMat;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        sphereMat.SetVector("_Location", plane.transform.position);

        sphereMat.SetVector("_RotationAxis", plane.transform.eulerAngles);
        

    }
}
