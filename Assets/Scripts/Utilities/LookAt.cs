using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(this.transform.position - Camera.main.transform.position);
    }
}
