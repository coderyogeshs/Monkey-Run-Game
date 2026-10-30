using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour
{
    [Header("Panels (used to detect the open tab)")]
    [SerializeField] private GameObject _audioPanel;
    [SerializeField] private GameObject _graphicsPanel;
    [SerializeField] private GameObject _accountPanel;

    [Header("Audio")]
    [SerializeField] private Slider _masterSlider;
    [SerializeField] private Slider _musicSlider;
    [SerializeField] private Slider _sfxSlider;
    [SerializeField] private Toggle _muteToggle;

    [Header("Graphics")]
    [SerializeField] private TMP_Dropdown _qualityDropdown;
    [SerializeField] private TMP_Dropdown _resolutionDropdown;
    [SerializeField] private Toggle _fullscreenToggle;

    [Header("Account")]
    [SerializeField] private TMP_InputField _usernameInput;
    [SerializeField] private TMP_InputField _emailInput;
    [SerializeField] private TMP_InputField _passwordInput;
    [SerializeField] private TMP_InputField _bioInput;

    [Header("Feedback (optional)")]
    [SerializeField] private TMP_Text _statusText;
    [SerializeField] private float _statusDuration = 2f;

    // PlayerPrefs keys
    private const string MasterKey = "audio_master";
    private const string MusicKey = "audio_music";
    private const string SfxKey = "audio_sfx";
    private const string MuteKey = "audio_mute";
    private const string QualityKey = "graphics_quality";
    private const string ResolutionKey = "graphics_resolution";
    private const string FullscreenKey = "graphics_fullscreen";
    private const string UsernameKey = "acc_username";
    private const string EmailKey = "acc_email";
    private const string BioKey = "acc_bio";

    // default values
    private const float DefaultMaster = 80f;
    private const float DefaultMusic = 70f;
    private const float DefaultSfx = 90f;
    private const int DefaultResolutionIndex = 1;   // 1920x1080

    // quality level 2, or the highest level the project has
    private static int DefaultQuality => Mathf.Min(2, QualitySettings.names.Length - 1);

    private Coroutine _statusRoutine;

    private void Start() => LoadAll();

    // ---------- footer buttons: act on whichever tab is open ----------

    public void Apply()
    {
        if (_audioPanel.activeInHierarchy) ApplyAudio();
        else if (_graphicsPanel.activeInHierarchy) ApplyGraphics();
        else if (_accountPanel.activeInHierarchy) ApplyAccount();
    }

    public void Reset()
    {
        if (_audioPanel.activeInHierarchy) ResetAudio();
        else if (_graphicsPanel.activeInHierarchy) ResetGraphics();
        else if (_accountPanel.activeInHierarchy) ResetAccount();
    }

    // ---------- AUDIO ----------

    public void ApplyAudio()
    {
        PlayerPrefs.SetFloat(MasterKey, _masterSlider.value);
        PlayerPrefs.SetFloat(MusicKey, _musicSlider.value);
        PlayerPrefs.SetFloat(SfxKey, _sfxSlider.value);
        PlayerPrefs.SetInt(MuteKey, _muteToggle.isOn ? 1 : 0);
        PlayerPrefs.Save();

        ApplyAudioToGame();
        ShowStatus("Audio settings saved");
    }

    public void ResetAudio()
    {
        _masterSlider.value = DefaultMaster;
        _musicSlider.value = DefaultMusic;
        _sfxSlider.value = DefaultSfx;
        _muteToggle.isOn = false;

        ApplyAudio();   // saves the defaults too
        ShowStatus("Audio reset to default");
    }

    // master volume controls everything that plays; mute sets it to 0
    private void ApplyAudioToGame()
    {
        AudioListener.volume = _muteToggle.isOn ? 0f : _masterSlider.value / 100f;
    }

    // ---------- GRAPHICS ----------

    public void ApplyGraphics()
    {
        PlayerPrefs.SetInt(QualityKey, _qualityDropdown.value);
        PlayerPrefs.SetInt(ResolutionKey, _resolutionDropdown.value);
        PlayerPrefs.SetInt(FullscreenKey, _fullscreenToggle.isOn ? 1 : 0);
        PlayerPrefs.Save();

        ApplyGraphicsToGame();
        ShowStatus("Graphics settings saved");
    }

    public void ResetGraphics()
    {
        _qualityDropdown.value = DefaultQuality;
        _resolutionDropdown.value = DefaultResolutionIndex;
        _fullscreenToggle.isOn = true;

        ApplyGraphics();
        ShowStatus("Graphics reset to default");
    }

    private void ApplyGraphicsToGame()
    {
        QualitySettings.SetQualityLevel(Mathf.Clamp(_qualityDropdown.value, 0, QualitySettings.names.Length - 1));

#if UNITY_STANDALONE && !UNITY_EDITOR
        // resolution and fullscreen only apply in a PC build
        string[] parts = _resolutionDropdown.options[_resolutionDropdown.value].text.Split('x');

        if (parts.Length == 2
            && int.TryParse(parts[0], out int width)
            && int.TryParse(parts[1], out int height))
        {
            Screen.SetResolution(width, height, _fullscreenToggle.isOn);
        }
#endif
    }

    // ---------- ACCOUNT ----------

    public void ApplyAccount()
    {
        string username = _usernameInput.text.Trim();
        string email = _emailInput.text.Trim();
        string bio = _bioInput.text.Trim();

        if (string.IsNullOrEmpty(username)) { Reject("Please enter a username", _usernameInput); return; }
        if (string.IsNullOrEmpty(email)) { Reject("Please enter your email", _emailInput); return; }
        if (!email.Contains("@") || !email.Contains(".")) { Reject("Please enter a valid email", _emailInput); return; }
        if (string.IsNullOrEmpty(_passwordInput.text)) { Reject("Please enter a password", _passwordInput); return; }
        if (string.IsNullOrEmpty(bio)) { Reject("Please fill in your bio", _bioInput); return; }

        // the password is not saved on purpose (PlayerPrefs is plain text)
        PlayerPrefs.SetString(UsernameKey, username);
        PlayerPrefs.SetString(EmailKey, email);
        PlayerPrefs.SetString(BioKey, _bioInput.text);
        PlayerPrefs.Save();

        ShowStatus("Account saved");
    }

    // red message, and the cursor jumps into the empty box
    private void Reject(string message, TMP_InputField field)
    {
        ShowStatus(message, true);
        field.ActivateInputField();
    }

    public void ResetAccount()
    {
        _usernameInput.text = "";
        _emailInput.text = "";
        _passwordInput.text = "";
        _bioInput.text = "";

        PlayerPrefs.DeleteKey(UsernameKey);
        PlayerPrefs.DeleteKey(EmailKey);
        PlayerPrefs.DeleteKey(BioKey);
        PlayerPrefs.Save();

        ShowStatus("Account cleared");
    }

    // ---------- LOAD ----------

    private void LoadAll()
    {
        _masterSlider.value = PlayerPrefs.GetFloat(MasterKey, DefaultMaster);
        _musicSlider.value = PlayerPrefs.GetFloat(MusicKey, DefaultMusic);
        _sfxSlider.value = PlayerPrefs.GetFloat(SfxKey, DefaultSfx);
        _muteToggle.isOn = PlayerPrefs.GetInt(MuteKey, 0) == 1;
        ApplyAudioToGame();

        _qualityDropdown.value = PlayerPrefs.GetInt(QualityKey, DefaultQuality);
        _resolutionDropdown.value = PlayerPrefs.GetInt(ResolutionKey, DefaultResolutionIndex);
        _fullscreenToggle.isOn = PlayerPrefs.GetInt(FullscreenKey, 1) == 1;

        _usernameInput.text = PlayerPrefs.GetString(UsernameKey, "");
        _emailInput.text = PlayerPrefs.GetString(EmailKey, "");
        _bioInput.text = PlayerPrefs.GetString(BioKey, "");

        if (_statusText != null) _statusText.text = "";
    }

    // ---------- STATUS MESSAGE ----------

    private void ShowStatus(string message, bool isError = false)
    {
        if (_statusText == null) return;

        if (_statusRoutine != null) StopCoroutine(_statusRoutine);
        _statusRoutine = StartCoroutine(StatusRoutine(message, isError));
    }

    private IEnumerator StatusRoutine(string message, bool isError)
    {
        _statusText.color = isError ? Color.red : Color.green;
        _statusText.text = message;

        // realtime, so it also works while the game is paused
        yield return new WaitForSecondsRealtime(_statusDuration);

        _statusText.text = "";
    }
}