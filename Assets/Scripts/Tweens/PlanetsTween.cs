using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlanetsTween : MonoBehaviour
{

    public GameObject planetsContainer;
    public void ExpandInitialPlanetsTween()
    {
        planetsContainer.transform.DOScale(1, 3).From(0);
    }
}
