using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : MonoBehaviour
{

    // Update is called once per frame
    Vector3 horizontalLookAt;
   
    void Update()
    {

        //transform.LookAt((this.transform.position - Camera.main.transform.position) * 100);
        //transform.rotation = Quaternion.LookRotation(horizontalLookAt);
        this.transform.LookAt(Camera.main.transform.position);
        this.transform.Rotate(new Vector3(0, 180, 0), Space.Self);

    }
}
