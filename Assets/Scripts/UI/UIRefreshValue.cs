using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIRefreshValue : MonoBehaviour
{
    [Header("ASSIGN UI WHICH WILL REFRESH")]
    [SerializeField] private TextMeshProUGUI _refreshText;
    [SerializeField] private Slider _slider;
    [SerializeField] string _addComplementText;
    [Header("THIS UI NEEDS SAVE/LOAD?")]
    [SerializeField] private bool _useSaveLoad;
    [SerializeField] private string _keyToSaveLoad;
    [SerializeField] private int _defaultValue;

    private void OnEnable()
    {
        if(_useSaveLoad)
        {
            int loadedValue = PlayerPrefs.GetInt(_keyToSaveLoad, _defaultValue);

            if(_refreshText != null)
                RefreshTextWithValue(loadedValue);

            if (_slider != null)
                RefreshSliderWithValue(loadedValue);
        }
    }

    public void RefreshTextWithValue(float value)
    {
        int valueInt = (int)value;
        _refreshText.text = _addComplementText + valueInt.ToString();
    }

    public void AddValueToSave(float value)
    {
        SaveManager.Instance.AddChangeKeyToSave(_keyToSaveLoad, (int)value);
    }

    public void RefreshSliderWithValue(float value)
    {
        _slider.value = value;
    }
}
