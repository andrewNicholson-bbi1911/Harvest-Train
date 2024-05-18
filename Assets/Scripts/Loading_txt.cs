using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Loading_txt : MonoBehaviour
{
    [SerializeField] private float _changePeriod = 0.2f;
    [SerializeField] private int _dotsAmount = 3;
    [SerializeField] private string _loadingText = "Loading";
    [SerializeField] private TextMeshProUGUI _text;

    private void Awake()
    {
        StartCoroutine(LoadingTextUpdate());
    }

    private IEnumerator LoadingTextUpdate()
    {
        int dots = 1;
        string _dts = ".";
        while (true)
        {
            _text.text = $"{_loadingText}{ _dts}";
            yield return new WaitForSeconds(_changePeriod);
            dots++;
            _dts += ".";
            dots %= (_dotsAmount+1);
            if (dots == 0)
            {
                dots = 1;
                _dts = ".";
            }
        }
    }
}
