using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class LookAtYAxis : MonoBehaviour
{
    Vector3 horizontalLookAt;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        horizontalLookAt = new Vector3(Camera.main.transform.position.x,
                                this.transform.position.y,
                                Camera.main.transform.position.z);
        transform.LookAt(horizontalLookAt);

    }
}
