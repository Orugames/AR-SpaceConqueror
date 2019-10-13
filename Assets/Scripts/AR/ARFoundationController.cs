using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;



public class ARFoundationController : MonoBehaviour
{
    public GameObject prePlacementGO;

    public GameObject gameCenter;

    //public GameObject hologramMessageGO;
    //public Text hologramMessageText;
    
    //Initial animation to help players put the center on the table
    public GameObject UIAnimationPlacement;

    //ARFoundation elements
    ARPlaneManager m_PlaneManager;
    static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();
    ARRaycastManager m_RaycastManager;
    ARReferencePointManager m_ReferencePointManager;


    void Awake()
    {
        m_RaycastManager = GetComponent<ARRaycastManager>();
        m_PlaneManager = GetComponent<ARPlaneManager>();
        m_ReferencePointManager = GetComponent<ARReferencePointManager>();

    }


    void Update()
    {
        CheckARPlayerPlaneInteraction();
    }

    private void CheckARPlayerPlaneInteraction()
    {
        Vector2 centerOfScreen = new Vector2(Screen.width / 2, Screen.height / 2);

        //Raycast the center of screen until it hits a detected plane, then position the hologram prefab in it
        if (m_RaycastManager.Raycast(centerOfScreen, s_Hits, TrackableType.PlaneWithinPolygon))
        {

            // Raycast hits are sorted by distance, so the first one
            // will be the closest hit.
            var hit = s_Hits[0];

            //Get the plane currently raycasting
            ARPlane plane = m_PlaneManager.GetPlane(hit.trackableId);

            //Activate the preset prefab, disable the final prefab
            prePlacementGO.SetActive(true);
            gameCenter.SetActive(false);

            prePlacementGO.transform.position = hit.pose.position;
            //hologramPrefab.transform.rotation = hit.pose.rotation;

            //Hologram perpendicular to plane
            Vector3 LookCameraVector = new Vector3(Camera.main.transform.position.x,
                                                            hit.pose.position.y,
                                                            Camera.main.transform.position.z);

            prePlacementGO.transform.LookAt(LookCameraVector);
            prePlacementGO.transform.Rotate(0, 180, 0, Space.Self);

            //Rotate the message so it looks the player
            //hologramMessageText.text = "Try to place it on a table";
            //hologramMessageGO.transform.LookAt(Camera.main.transform);
            //hologramMessageGO.transform.Rotate(0, 180, 0, Space.Self);

            //Logic to put the final prefab in place
            PlaceCenterOfMeeting(hit, plane);
            
        }
    }

    private void PlaceCenterOfMeeting(ARRaycastHit hit, ARPlane plane)
    {
        RaycastHit hitRaycast;

        //If we do not touch the screen, dont instantiate
        if (Input.touchCount < 1)
            return;

        var touch = Input.GetTouch(0);

        if (touch.phase != TouchPhase.Began)
            return;


        Ray raycast = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);

        if (Physics.Raycast(raycast, out hitRaycast) && (hitRaycast.collider is BoxCollider))
        {
            gameCenter.SetActive(true);
            prePlacementGO.SetActive(false);

            //If the touched plane is horizontal
            if (plane.alignment == PlaneAlignment.HorizontalDown ||
                plane.alignment == PlaneAlignment.HorizontalUp)
            {
                //Similar to anchors, it helps the device to maintain position and rotation of the placed objects
                ARReferencePoint referencePoint = m_ReferencePointManager.AddReferencePoint(hit.pose);

                gameCenter.transform.position = hit.pose.position;

                //perpendicular to plane
                Vector3 LookCameraVector = new Vector3(Camera.main.transform.position.x,
                                                       hit.pose.position.y,
                                                       Camera.main.transform.position.z);

                gameCenter.transform.LookAt(LookCameraVector);

                //Last needed movements
                gameCenter.transform.parent = referencePoint.transform;

                //tween animation to scale the object
                gameCenter.transform.localScale = Vector3.zero;
                gameCenter.transform.DOScale(Vector3.one, 1).From(0);


                //Disable this script so it won't be triggered again
                //Disable detected planes
                ARPlaneManager planeManager = GetComponent<ARPlaneManager>();

                foreach (ARPlane planeAR in planeManager.trackables)
                {
                    planeAR.gameObject.SetActive(false);

                }
                planeManager.enabled = false;
                UIAnimationPlacement.SetActive(false);
                this.enabled = false;


            }
            else //Vertical
            {

            }



        }

    }

 
}







