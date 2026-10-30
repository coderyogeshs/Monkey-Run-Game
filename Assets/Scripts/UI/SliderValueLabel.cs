using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderValueLabel : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private TMP_Text _valueText;

    private void OnEnable()
    {
        _slider.onValueChanged.AddListener(UpdateText);
        UpdateText(_slider.value);
    }

    private void OnDisable()
    {
        _slider.onValueChanged.RemoveListener(UpdateText);
    }

    private void UpdateText(float value)
    {
        _valueText.text = Mathf.RoundToInt(value).ToString();
    }
}