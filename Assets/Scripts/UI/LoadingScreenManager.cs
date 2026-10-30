using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class LoadingScreen : MonoBehaviour
{
    [Header("Loading UI")]
    [SerializeField] private GameObject loadingCanvas;
    [SerializeField] private GameObject mainMenuCanvas;
    [SerializeField] private Image loadingFill;
    [SerializeField] private TMP_Text percentageText;

    [Header("Settings")]
    [SerializeField] private float loadingDuration = 3f;

    private void Start()
    {
        if (loadingCanvas == null ||
            mainMenuCanvas == null ||
            loadingFill == null ||
            percentageText == null)
        {
            Debug.LogError("LoadingScreen: One or more Inspector references are missing!", this);
            return;
        }

        loadingCanvas.SetActive(true);
        mainMenuCanvas.SetActive(false);

        StartCoroutine(LoadGame());
    }

    private IEnumerator LoadGame()
    {
        float elapsed = 0f;

        loadingFill.fillAmount = 0f;

        while (elapsed < loadingDuration)
        {
            elapsed += Time.deltaTime;

            float progress = Mathf.Clamp01(elapsed / loadingDuration);

            loadingFill.fillAmount = progress;

            percentageText.text =
                Mathf.RoundToInt(progress * 100f) + "%";

            yield return null;
        }

        loadingFill.fillAmount = 1f;
        percentageText.text = "100%";

        yield return new WaitForSeconds(0.5f);

        loadingCanvas.SetActive(false);
        mainMenuCanvas.SetActive(true);
    }
}