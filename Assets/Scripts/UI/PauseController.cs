using UnityEngine;

public class PauseController : MonoBehaviour
{
    [SerializeField] private GameObject _pausePanel;

    private void OnEnable()
    {
        _pausePanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void Pause()
    {
        _pausePanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        _pausePanel.SetActive(false);
        Time.timeScale = 1f;
    }
}