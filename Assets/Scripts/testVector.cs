using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testVector : MonoBehaviour
{
    public GameObject spherePrefab;
    public float nElements;
    void Start()
    {
        for (int i = 0; i < nElements; i++)
        {
            GameObject newSphere = Instantiate(spherePrefab, this.transform);

            // move according to forward vector
            //newSphere.transform.Rotate(transform.right, 90);
            Vector3 planetDest = this.transform.forward;
            Debug.Log(planetDest.normalized);
            // Face planet
            newSphere.transform.LookAt(planetDest * 1000, Vector3.up);
            //Rotate initial ship pos according to number of ships
            newSphere.transform.Rotate(newSphere.transform.forward, (360 / nElements) * i,Space.World);

            // Move the positions out of the radius of the planet
            newSphere.transform.position += newSphere.transform.up;


        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
