using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetRotation : MonoBehaviour
{
    Vector3 randomRotationAxis;
    float rotSpeed;
    void Start()
    {
        //Randomize the rotation of each planet from an initial up vector
        rotSpeed = Random.Range(-0.15f, 0.15f);

        // Small variations over vertical Axis
        randomRotationAxis = new Vector3(Random.Range(0f, 0.25f),
                                                    1,
                                                    Random.Range(0f, 0.25f));
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.Rotate(randomRotationAxis, rotSpeed);
    }
}
