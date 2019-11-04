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
    public void HideLevelSelector()
    {

        for (int i = 0; i < levelSelectorGO.transform.childCount; i++)
        {
            levelSelectorGO.transform.GetChild(i).transform.DOScaleZ(0, 0.75f).OnComplete(() => 
            {
                levelSelectorGO.SetActive(false);
            }
            );

        }
    }
}
