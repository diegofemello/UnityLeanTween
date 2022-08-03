using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingManager : SingletonPersistent<LoadingManager>
{
    public bool m_isLoading = false;
    private Animator _animatorComponent;
    [SerializeField] GameObject requestLoading;
    [SerializeField] Image errorMessage;
    [SerializeField] TextMeshProUGUI errorText;

    private bool showingError = false;

    private int _index = 0;

    private void OnEnable()
    {
        _animatorComponent = GetComponent<Animator>();
    }

    public void NextLevel(EnumScenes scene)
    {
        //SuperiorMenuBarScreen._instance.Hide();
        _index = (int)scene;

        RevealLoadingScreen();
        if (!m_isLoading)
        {
            m_isLoading = true;
            Time.timeScale = 0f;
        }
    }

    public void RequestLoader(bool active)
    {
        requestLoading.SetActive(active);
    }

    public void ShowError(string error, float duration = 2f)
    {
        if (!showingError)
        {
            showingError = true;
            errorMessage.gameObject.SetActive(true);
            errorText.text = error;
            StartCoroutine(Fade(errorMessage.gameObject, duration));
        }
    }

    IEnumerator Fade(GameObject obj, float duration)
    {
        yield return new WaitForSeconds(duration);
        obj.SetActive(false);

        showingError = false;
        yield return null;
    }


    public void ReloadLevel()
    {
        NextLevel((EnumScenes)CurrentLevel());
    }

    IEnumerator LoadLevel(int index)
    {
        yield return null;

        AsyncOperation operation = SceneManager.LoadSceneAsync(index, LoadSceneMode.Single);
        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            yield return new WaitForSecondsRealtime(0.8f);

            if (operation.progress.Equals(.9f))
            {
                operation.allowSceneActivation = true;
            }

            yield return new WaitForEndOfFrame();
            HideLoadingScreen();
        }

    }

    public int CurrentLevel()
    {
        return SceneManager.GetActiveScene().buildIndex;
    }

    public void RevealLoadingScreen()
    {
        _animatorComponent.Play("Reveal");
    }

    public void HideLoadingScreen()
    {
        _animatorComponent.Play("Hide");
    }

    public void OnFinishedReveal()
    {
        StartCoroutine(LoadLevel(_index));
    }

    public void OnFinishedHide()
    {
        Time.timeScale = 1f;
        m_isLoading = false;
    }
}