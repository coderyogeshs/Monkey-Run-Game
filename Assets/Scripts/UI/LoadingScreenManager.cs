using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class LoadingScreenManager : MonoBehaviour
{
    [Header("Loading UI")]
    [SerializeField] private Image loadingFill;
    [SerializeField] private TMP_Text percentageText;

    [Header("Scene Settings")]
    [SerializeField] private string nextSceneName = "MainMenu";

    [Header("Loading Settings")]
    [SerializeField] private float minimumLoadingTime = 2f;

    private void Start()
    {
        StartCoroutine(LoadNextScene());
    }

    private IEnumerator LoadNextScene()
    {
        float elapsedTime = 0f;

        AsyncOperation operation =
            SceneManager.LoadSceneAsync(nextSceneName);

        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            elapsedTime += Time.deltaTime;

            // Unity loading progress ranges from 0 to 0.9
            float sceneProgress =
                Mathf.Clamp01(operation.progress / 0.9f);

            // Minimum loading duration
            float timeProgress =
                Mathf.Clamp01(elapsedTime / minimumLoadingTime);

            // Calculate displayed progress
            float progress = Mathf.Min(sceneProgress, timeProgress);

            // Update loading bar
            loadingFill.fillAmount = progress;

            // Update percentage
            percentageText.text =
                Mathf.RoundToInt(progress * 100f) + "%";

            // Activate scene after loading completes
            if (operation.progress >= 0.9f &&
                elapsedTime >= minimumLoadingTime)
            {
                loadingFill.fillAmount = 1f;
                percentageText.text = "100%";

                yield return new WaitForSeconds(0.3f);

                operation.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}
