using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipAvoidanceLogic : MonoBehaviour
{

    public GameObject target;
    public float speed;
    public float rotationSpeed;
    public float raycastRange;
    private RaycastHit hit;

    public bool attackOrderGiven = false;
    public bool obstacleDetected = false;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // If not given the order, do nothing
        if (!attackOrderGiven)
        {
            return;
        }

    }
}
