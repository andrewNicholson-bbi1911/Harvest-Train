using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ResourceSystem;
using TMPro;

public class UIIntResourceValueCard : MonoBehaviour
{
    [SerializeField] private IntResourceContainer _trackingResourceContainer;
    [SerializeField] private List<UIIntResCardData> _trackingResourcesUIData;

    private void Start()
    {
        _trackingResourceContainer.SubscribeOnOnValueChangedEvent(UpdateUI);
        ForceUIUpdate();
    }

    private void ForceUIUpdate()
    {
        foreach(var trackRes in _trackingResourcesUIData)
        {
            trackRes.UpdateUI(_trackingResourceContainer.GetResourceCurrentValue(trackRes.ResourceID));
        }
    }

    private void UpdateUI(int val, string id)
    {
        foreach(var uiData in _trackingResourcesUIData.FindAll(x => x.ResourceID == id))
        {
            uiData.UpdateUI(val);
        }
    }
}

[System.Serializable]
public class UIIntResCardData{

    public string ResourceID { get => _trackingResource.ID; }

    [SerializeField] private ResourceSO _trackingResource;
    [Tooltip("use <value> to mark where place the value of tracking resource")]
    [SerializeField] private int _showingExtraValue = 0;
    [SerializeField] private string _pattern = "<value>";
    [SerializeField] private TextMeshProUGUI _valueText;
    [SerializeField] private TextMeshProUGUI _resourceNameText = null;
    [SerializeField] private Image _resourceIcons = null;

    public void UpdateUI(int value)
    {
        value += _showingExtraValue;

        _valueText.text = _pattern.Replace("<value>", ValueToStringConverter.GetBeautifulIntText(value));
        if (_resourceNameText != null)
        {
            _resourceNameText.text = _trackingResource.name;
        }
        if (_resourceIcons != null)
        {
            _resourceIcons.sprite = _trackingResource.Icon;
        }
    }
}
