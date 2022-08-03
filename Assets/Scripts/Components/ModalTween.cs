using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ModalTween : MonoBehaviour
{
    [SerializeField]
    Button yesBtn, noBtn;

    [SerializeField]
    TextMeshProUGUI description;

    [HideInInspector]
    public Action yesAction, noAction;

    void Awake()
    {
        transform.localScale = new Vector3(0f, 0f, 0f);
        yesBtn.transform.localScale = new Vector3(0f, 0f, 0f);
        noBtn.transform.localScale = new Vector3(0f, 0f, 0f);
    }

    public void SetActions(Action yes, Action no = null)
    {
        yesAction = yes;
        noAction = no;
    }

    public void UpdateText(string title)
    {
        description.text = title;
    }

    public void PainelEnable()
    {
        LeanTween.moveLocal(gameObject, new Vector3(0f, 0f, 0f), 0.5f).setEase(LeanTweenType.easeOutCirc);
        LeanTween.scale(gameObject, new Vector3(2f, 2f, 1f), .5f).setEase(LeanTweenType.easeOutCirc);
        LeanTween.scale(yesBtn.gameObject, new Vector3(1.5f, 1.5f, 1.5f), .5f).setDelay(.3f).setEase(LeanTweenType.easeOutCirc);
        LeanTween.scale(noBtn.gameObject, new Vector3(1.5f, 1.5f, 1.5f), .5f).setDelay(.4f).setEase(LeanTweenType.easeOutCirc);
    }

    public void YesButton()
    {
        Exit(yesAction);
    }

    public void NoButton()
    {
        Exit(noAction);
    }

    void Exit(Action action)
    {
        LeanTween.scale(yesBtn.gameObject, new Vector3(0f, 0f, 0f), .5f).setEase(LeanTweenType.easeOutCirc);
        LeanTween.scale(noBtn.gameObject, new Vector3(0f, 0f, 0f), .5f).setDelay(.1f).setEase(LeanTweenType.easeOutCirc);
        LeanTween.moveLocal(gameObject, new Vector3(0f, -615f, 0f), 0.5f).setDelay(.1f).setEase(LeanTweenType.easeInQuart);
        LeanTween.scale(gameObject, new Vector3(0f, 0f, 0f), .5f).setDelay(.1f).setEase(LeanTweenType.easeInQuart).setOnComplete(action);

        Destroy(gameObject, 2f);
    }

}
