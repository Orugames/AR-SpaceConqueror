using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using System.Collections;
using System.Collections.Generic;
using UnityEngine;



using UnityEngine;
using System.Collections;
public class Avoidance : MonoBehaviour
{
    // Fix a range how early u want your enemy detect the obstacle.
    public int range = 100;
    public float speed = 2;
    public bool isThereAnyThing = false;
    // Specify the target for the enemy.
    public GameObject target;
    public float rotationSpeed = 15;
    private RaycastHit hit;
    // Use this for initialization
    void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {
        //Look At Somthly Towards the Target if there is nothing in front.
        if (!isThereAnyThing)
        {
            Vector3 relativePos = target.transform.position - transform.position;
            Quaternion rotation = Quaternion.LookRotation(relativePos);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime);
        }
        // Enemy translate in forward direction.
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
        //Checking for any Obstacle in front.
        // Two rays left and right to the object to detect the obstacle.
        Transform leftRay = transform;
        Transform rightRay = transform;
        //Use Phyics.RayCast to detect the obstacle
        if (Physics.Raycast(leftRay.position + (transform.right * 1), transform.forward, out hit, range) || Physics.Raycast(rightRay.position - (transform.right * 1), transform.forward, out hit, range))
        {
            if (hit.collider.gameObject.CompareTag("Obstacle"))
            {
                isThereAnyThing = true;
                transform.Rotate(Vector3.up * Time.deltaTime * rotationSpeed);
            }
        }
        // Now Two More RayCast At The End of Object to detect that object has already pass the obsatacle.
        // Just making this boolean variable false it means there is nothing in front of object.
        if (Physics.Raycast(transform.position - (transform.forward * 1), transform.right, out hit, 10) ||
         Physics.Raycast(transform.position - (transform.forward * 1), -transform.right, out hit, 10))
        {
            if (hit.collider.gameObject.CompareTag("Obstacle"))
            {
                isThereAnyThing = false;
            }
        }
        // Use to debug the Physics.RayCast.
        Debug.DrawRay(transform.position + (transform.right * 1), transform.forward * 20, Color.red);
        Debug.DrawRay(transform.position - (transform.right * 1), transform.forward * 20, Color.red);
        Debug.DrawRay(transform.position - (transform.forward * 1), -transform.right * 20, Color.yellow);
        Debug.DrawRay(transform.position - (transform.forward * 1), transform.right * 20, Color.yellow);
    }
}
