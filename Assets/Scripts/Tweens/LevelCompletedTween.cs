using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class LevelCompletedTween : MonoBehaviour
{
    public CanvasGroup levelCompletedCanvasGroup;

    public void ShowLevelCompletedScreen()
    {
        levelCompletedCanvasGroup.DOFade(1, 1).From(0);
        levelCompletedCanvasGroup.transform.DOScaleY(levelCompletedCanvasGroup.transform.localScale.y, 1).From(0);
    }
    public void HideLevelCompletedGoBack()
    {
        levelCompletedCanvasGroup.DOFade(0, 2).From(1).OnComplete(() =>
        {
            levelCompletedCanvasGroup.gameObject.SetActive(false);
            //MainGameController.instance.StartBattle();
        });
        levelCompletedCanvasGroup.transform.DOScaleY(0, 1).From(levelCompletedCanvasGroup.transform.localScale.y);

    }
    public void HideLevelCompletedNextLevel()
    {
        levelCompletedCanvasGroup.DOFade(0, 2).From(1).OnComplete(() =>
        {
            levelCompletedCanvasGroup.gameObject.SetActive(false);
            //MainGameController.instance.StartBattle();
        });
        levelCompletedCanvasGroup.transform.DOScaleY(0, 1).From(levelCompletedCanvasGroup.transform.localScale.y);

    }
}
