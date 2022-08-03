using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameScreen : MonoBehaviour
{
    [SerializeField] Button quitBtn;
    [SerializeField] TextMeshProUGUI gameText;

    [SerializeField] ModalTween modalConfirm;

    void Awake()
    {
        quitBtn.transform.localScale = new Vector3(0f, 0f, 0f);
        gameText.transform.localScale = new Vector3(0f, 0f, 0f);
    }

    void Start()
    {
        LeanTween.scale(quitBtn.gameObject, new Vector3(1f, 1f, 1f), .5f).setEase(LeanTweenType.easeOutCirc);
        LeanTween.scale(gameText.gameObject, new Vector3(1f, 1f, 1f), .5f).setEase(LeanTweenType.easeOutCirc);
    }

    
    public void QuitButton()
    {
        var panelTest = Instantiate(modalConfirm, parent: transform).GetComponent<ModalTween>();
        string description = "Deseja retornar ao menu do jogo?";

        panelTest.SetActions(YesButton, NoButton);
        panelTest.UpdateText(description);
        panelTest.PainelEnable();

        LeanTween.scale(quitBtn.gameObject, new Vector3(0f, 0f, 0f), .5f).setEase(LeanTweenType.easeOutCirc);
    }

    public void YesButton()
    {
        LeanTween.scale(gameText.gameObject, new Vector3(0f, 0f, 0f), .5f).setEase(LeanTweenType.easeOutCirc).setOnComplete(LoadMainMenu);
    }

    public void NoButton()
    {
        LeanTween.scale(quitBtn.gameObject, new Vector3(1f, 1f, 1f), .5f).setDelay(.7f).setEase(LeanTweenType.easeOutCirc);
    }

    void LoadMainMenu()
    {
        LoadingManager.Instance.NextLevel(EnumScenes.MainMenu);
    }
}
