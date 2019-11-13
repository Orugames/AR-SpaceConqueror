using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceFromPlaneShaderLogic : MonoBehaviour
{
    public GameObject dissolvingPlane;
    public Transform planeTransform;

    public GameObject holotable;
    public Renderer holotableRenderer;
    Material holotableMaterial;

    void Start()
    {
        holotableMaterial = holotableRenderer.material;
    }

    // Update is called once per frame
    void Update()
    {
        holotableMaterial.SetVector("_DistanceFromPlane", planeTransform.position);
    }
}
