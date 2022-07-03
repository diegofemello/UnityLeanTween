using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelTween : MonoBehaviour
{
    [SerializeField]
    GameObject backPanel, star1, star2, star3, score, coins, gems, colorWheel, levelSucess;

    [SerializeField]
    Button homeButton, replayButton;

    void Start()
    {
        LeanTween.rotateAround(colorWheel, Vector3.forward, -360f, 10f).setLoopClamp();
        LeanTween.scale(levelSucess, new Vector3(1.5f, 1.5f, 1.5f), 2f).setDelay(.5f).setEase(LeanTweenType.easeOutElastic).setOnComplete(LevelComplete);
        LeanTween.moveLocal(levelSucess, new Vector3(-30f, 747f, 2f), 0.7f).setDelay(2f).setEase(LeanTweenType.easeInOutCubic);
        LeanTween.scale(levelSucess, new Vector3(1f, 1f, 1f), 2f).setDelay(1.7f).setEase(LeanTweenType.easeInOutCubic);

    }

    void LevelComplete()
    {

        LeanTween.moveLocal(backPanel, new Vector3(0f, -267f, 0f), 0.7f).setDelay(.5f).setEase(LeanTweenType.easeOutCirc).setOnComplete(StarsAnim);
        LeanTween.scale(homeButton.gameObject, new Vector3(1f, 1f, 1f), 2f).setDelay(.8f).setEase(LeanTweenType.easeOutElastic);
        LeanTween.scale(replayButton.gameObject, new Vector3(1f, 1f, 1f), 2f).setDelay(.9f).setEase(LeanTweenType.easeOutElastic);
        LeanTween.alpha(score.GetComponent<RectTransform>(), 1f, .5f).setDelay(1f);
        LeanTween.alpha(coins.GetComponent<RectTransform>(), 1f, .5f).setDelay(1.1f);
        LeanTween.alpha(gems.GetComponent<RectTransform>(), 1f, .5f).setDelay(1.2f);
    }


    void StarsAnim()
    {
        LeanTween.scale(star1, new Vector3(1f, 1f, 1f), 2f).setEase(LeanTweenType.easeOutElastic);
        LeanTween.scale(star2, new Vector3(1f, 1f, 1f), 2f).setDelay(.1f).setEase(LeanTweenType.easeOutElastic);
        LeanTween.scale(star3, new Vector3(1f, 1f, 1f), 2f).setDelay(.2f).setEase(LeanTweenType.easeOutElastic);

    }
}