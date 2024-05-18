using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

namespace ResourceSystem
{
    public abstract class UIResourceContainerCanvas<ResourceDataType, T> : MonoBehaviour
        where ResourceDataType : ResourceData<T>
        where T : struct, IComparable
    {
        [SerializeField] private ResourceContainer<ResourceDataType, T> _connectedResourceContainer;
        [SerializeField] private List<UIResourceData<T>> _uiResourceData = new List<UIResourceData<T>>();
        [SerializeField] private bool _autoUpdateUI = true;
        private void Awake()
        {
            if(_autoUpdateUI)
                _connectedResourceContainer?.SubscribeOnOnValueChangedEvent(UpdateResourceValues);
        }

        public void ShowContainingResources()
        {
            foreach (var uiRes in _uiResourceData)
            {
                uiRes.Show();
            }
        }

        public void HideContainingResources()
        {
            foreach (var uiRes in _uiResourceData)
            {
                uiRes.Hide();
            }
        }


        public void InitValues()
        {
            foreach (var ui in _uiResourceData)
            {
                ui.Init();
            }

            foreach(var resID in _connectedResourceContainer.GetContainedResourcesID())
            {
                var resVal = _connectedResourceContainer.GetResourceCurrentValue(resID);
                UpdateResourceValues(resVal, resID);

            }
        }

        public void UpdateResourceValues(T currentValue, string id)
        {
            foreach(var ui in _uiResourceData)
            {
                ui.SetValue(_connectedResourceContainer.GetResourceCurrentValue(ui.resourceID));
            }
        }

    }


    [Serializable]
    public class UIResourceData<T> where T : struct, IComparable 
    {
        public string resourceID { get => _resource.ID; }

        [SerializeField] private ResourceSO _resource;
        [Space]
        [SerializeField] private GameObject _uiResourceParentObject;
        [SerializeField] private TextMeshProUGUI _nameText = null;
        [SerializeField] private TextMeshProUGUI _valueText = null;
        [SerializeField] private Image _resourceIcon = null;

        public virtual void Init()
        {
            _InitIcon();
            _InitName();
        }

        public void Show()
        {
            _uiResourceParentObject.SetActive(true);
        }

        public void Hide()
        {
            _uiResourceParentObject.SetActive(false);
        }

        public virtual void SetValue(T value)
        {
            if (_valueText != null)
            {
                _valueText.text = value.ToString();
            }
            else
            {
                Debug.LogWarning($"{this}>>>you trying to initialize VALUE of resource {_resource.Name}, but the TextMesh is not inited");
            }
        }

        private void _InitName()
        {
            if (_nameText != null)
            {
                _nameText.text = _resource.Name;
            }
            else
            {
                Debug.LogWarning($"{this}>>>you trying to initialize NAME of resource {_resource.Name}, but the TextMesh is not inited");
            }
        }

        private void _InitIcon()
        {
            if (_resourceIcon != null)
            {
                _resourceIcon.sprite = _resource.Icon;
            }
            else
            {
                Debug.LogWarning($"{this}>>>you trying to initialize ICON of resource {_resource.Name}, but the Image is not inited");
            }
        }
    }
}

