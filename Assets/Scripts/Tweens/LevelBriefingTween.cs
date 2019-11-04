using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class LevelBriefingTween : MonoBehaviour
{
    public CanvasGroup levelBrienfingCanvasGroup;

    public void ShowBriefingScreen()
    {
        levelBrienfingCanvasGroup.DOFade(1, 1).From(0);
        levelBrienfingCanvasGroup.transform.DOScaleY(levelBrienfingCanvasGroup.transform.localScale.y, 1).From(0);
    }
    public void HideBriefingScreen()
    {
        levelBrienfingCanvasGroup.DOFade(0, 2).From(1).OnComplete(() => 
        {
            levelBrienfingCanvasGroup.gameObject.SetActive(false);
            MainGameController.instance.StartBattle();
        });
        levelBrienfingCanvasGroup.transform.DOScaleY(0, 1).From(levelBrienfingCanvasGroup.transform.localScale.y);

        // Show floor renderer
        TweensManager.instance.floorShaderTween.ShowFloor();


    }
}
