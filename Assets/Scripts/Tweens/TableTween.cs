using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TableTween : MonoBehaviour
{
    public GameObject holotable;
    public GameObject floorHologram;
    public GameObject invisiblePlane;

    // Start is called before the first frame update
    void Start()
    {
        ShowHolotable();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowHolotable()
    {
        invisiblePlane.transform.DOLocalMoveY(2.5f, 5f).From(0.49f).SetEase(Ease.Linear).OnComplete(() => 
        {
            TweensManager.instance.floorShaderTween.ShowFloorWithAlphaChange();
        });
    }
}
