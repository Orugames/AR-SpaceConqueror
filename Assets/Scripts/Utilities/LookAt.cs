using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : MonoBehaviour
{

    // Update is called once per frame
    Vector3 horizontalLookAt;
   
    void Update()
    {
        horizontalLookAt = new Vector3(this.transform.position.x - 2 * Camera.main.transform.position.x,
                                        this.transform.position.y,
                                        this.transform.position.z - 2 * Camera.main.transform.position.z);
        transform.LookAt((this.transform.position - Camera.main.transform.position) * 100);
        //transform.rotation = Quaternion.LookRotation(horizontalLookAt);
        this.transform.LookAt(Camera.main.transform.position);
        this.transform.Rotate(new Vector3(0, 180, 0), Space.Self);

    }
}
