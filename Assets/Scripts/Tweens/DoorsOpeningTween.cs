using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DoorsOpeningTween : MonoBehaviour
{

    public GameObject plane1;
    public GameObject plane2;
    public GameObject plane3;
    public GameObject plane4;
    public GameObject plane5;
    public GameObject plane6;

    public bool EditorOrder;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (EditorOrder)
        {
            EditorOrder = false;
            OpenDoorsTween();
        }
    }

    public void OpenDoorsTween()
    {
        plane1.transform.DORotate(plane1.transform.eulerAngles + new Vector3(0, 0, 45), 5).SetDelay(2);
        plane2.transform.DORotate(plane2.transform.eulerAngles + new Vector3(0, 0, 45), 5).SetDelay(2);
        plane3.transform.DORotate(plane3.transform.eulerAngles + new Vector3(0, 0, 45), 5).SetDelay(2);
        plane4.transform.DORotate(plane4.transform.eulerAngles + new Vector3(0, 0, 45), 5).SetDelay(2);
        plane5.transform.DORotate(plane5.transform.eulerAngles + new Vector3(0, 0, 45), 5).SetDelay(2);

        plane6.transform.DORotate(plane6.transform.eulerAngles + new Vector3(0, 0, 45), 5).SetDelay(2).OnComplete(() => 
        TweensManager.instance.planetsTween.ExpandInitialPlanetsTween());
    }
}
