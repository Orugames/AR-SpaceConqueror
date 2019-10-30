using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LevelSelectorTween : MonoBehaviour
{
    public GameObject levelSelectorGO;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ExpandLevelSelector()
    {
        levelSelectorGO.transform.DORotate(new Vector3(0, 360, 0), 3, RotateMode.LocalAxisAdd);
        levelSelectorGO.transform.DOScale(0.2f,3).From(0);
    }
}
