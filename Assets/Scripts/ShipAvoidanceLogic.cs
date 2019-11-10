using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ShipAvoidanceLogic : MonoBehaviour
{

    public GameObject nativePlanet;
    public GameObject target;
    public float speed;
    public float rotationSpeed;
    public float raycastRange;

    public float timeToRaycast;
    public float dissolveSpeed = 0.1f;

    private RaycastHit hit;

    public bool attackOrderGiven = false;
    public bool obstacleDetected = false;

    Coroutine moveTowardsObjective;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
      
    }

    public IEnumerator MoveTowardsObjective()
    {
        // If not given the order or not target, do nothing
        if (!attackOrderGiven || target == null)
        {
            yield return new WaitForSeconds(timeToRaycast);
        }

        while (true)
        {
            // If not an obstacle in sight, aim towards the target
            if (!obstacleDetected)
            {
                Vector3 relativePos = target.transform.position - transform.position;
                Quaternion rotation = Quaternion.LookRotation(relativePos);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime);
            }

            // Move towards target
            transform.Translate(Vector3.forward * Time.deltaTime * speed);

            // Then create the raycast
            Transform ray = transform;

            // If we detect an obstacle rotate a random direction until no detection
            if (Physics.Raycast(ray.position, transform.forward, out hit, raycastRange))
            {
                // If we detect something that is  not our target or native planet, is an obstacle
                if (hit.collider.gameObject != nativePlanet && hit.collider.gameObject != target)
                {
                    obstacleDetected = true;

                    // Rotate it a random vector
                    Vector3 randomVector = new Vector3(Random.Range(0f, 1f),
                                                       Random.Range(0f, 1f),
                                                       Random.Range(0f, 1f));

                    transform.Rotate(randomVector * Time.deltaTime * rotationSpeed);
                }
                else
                {
                    obstacleDetected = false;

                }

            }
            else
            {
                obstacleDetected = false;

            }

            // Use to debug the Physics.RayCast.
            Debug.DrawRay(ray.position, transform.forward * 20, Color.blue, 2);
            //yield return new WaitForSeconds(timeToRaycast);
            yield return new WaitForEndOfFrame();
        }

       
    }



    public IEnumerator CheckIfNotCollidingWithPlanet()
    {
        // Draw a raycast towards the target planet
        bool collidingWithNativePlanet = true;

        
        // If it collides with our planet it means we are still not able to travel to our target
        while (collidingWithNativePlanet)
        {
            Debug.DrawRay(transform.position,target.transform.position - transform.position, Color.red,2);

            RaycastHit hit;
            if (Physics.Raycast(transform.position, target.transform.position - transform.position, out hit))
            {
                // If colliding with planet
                if (hit.collider.gameObject == nativePlanet)
                {
                    Debug.Log("Nave chocando con planeta original");
                    collidingWithNativePlanet = true;
                }
                else
                {
                    collidingWithNativePlanet = false;
                    GetComponent<Spaceship>().rotateOrder = false;
                    //transform.DOPath(new Vector3[] { transform.position, target.transform.position }, 5, PathType.CatmullRom, PathMode.Full3D, 25).SetEase(Ease.Linear).SetLookAt(0);
                }
                // If not, free from rotation

                yield return new WaitForEndOfFrame();

            }

        }

        // Stop the rotate order
        yield return new WaitForEndOfFrame();
        // Start the coroutine to move towards target
        attackOrderGiven = true;
        moveTowardsObjective = StartCoroutine(MoveTowardsObjective());

    }

    public IEnumerator DissolveAndDestroy(List<Material> shipMaterials)
    {
        float time = 0;

        StopCoroutine(moveTowardsObjective);

        // Dissolve the ship
        while (time < 0.20f)
        {
            transform.Translate(Vector3.forward * Time.deltaTime * speed/4);

            foreach (Material material in shipMaterials)
            {
                material.SetFloat("_DissolveAmount", time);
                time += dissolveSpeed;

            }
            yield return new WaitForEndOfFrame();

        }

        // After dissolve, destroy this gameobject
        Destroy(this.gameObject);
        yield return new WaitForEndOfFrame();

    }

    public IEnumerator UndissolveShip(List<Material> shipMaterials, TrailRenderer trail)
    {
        trail.enabled = false;

        float time = 0.15f;

        while (time > 0)
        {

            foreach (Material material in shipMaterials)
            {
                material.SetFloat("_DissolveAmount", time);
                time -= dissolveSpeed / 4;

            }
            yield return new WaitForEndOfFrame();

        }
        trail.enabled = true;

        yield return new WaitForEndOfFrame();

    }

}
