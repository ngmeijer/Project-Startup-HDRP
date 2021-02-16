using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class KeypadCodeValidator : MonoBehaviour
{
    private Dictionary<string, int> _codeSpriteMaps;

    public string UnlockCode = "LENIN";

    public UnityEvent NotifyCodeAccepted;
    public UnityEvent NotifyAfterAccepted;
    
    public UnityEvent NotifyCodeRejected;
    public UnityEvent NotifyAfterRejected;

    [SerializeField] private Image[] _symbolImages;
    [SerializeField] private Sprite[] _symbolSprites;

    [SerializeField] private float _delayValue = 1f;

    private void Start()
    {
        _codeSpriteMaps = new Dictionary<string, int>()
        {
            {"A", 0},
            {"B", 1},
            {"E", 2},
            {"I", 3},
            {"K", 4},
            {"L", 5},
            {"N", 6},
            {"O", 7},
            {"S", 8},
            {"W", 9},
        };
    }

    public void OnCodeCompleted(string val)
    {
        StartCoroutine(WaitForDelayBeforeCodeValidation(val));
    }

    public void ReceiveCodeChars(string val)
    {
        int index = 0;

        foreach (var letter in val)
        {
            if (_codeSpriteMaps.ContainsKey(letter.ToString()))
            {
                _symbolImages[index].gameObject.SetActive(true);
                _symbolImages[index].sprite = _symbolSprites[_codeSpriteMaps[letter.ToString()]];
            }

            index++;
        }
    }

    private IEnumerator WaitForDelayBeforeCodeValidation(string val)
    {
        if (val == UnlockCode)
        {
            //Door will open
            NotifyCodeAccepted?.Invoke();
            yield return new WaitForSeconds(_delayValue);
            NotifyAfterAccepted?.Invoke();
        }
        else if (val != UnlockCode)
        {
            NotifyCodeRejected?.Invoke();
            yield return new WaitForSeconds(_delayValue);
            NotifyAfterRejected?.Invoke();

            foreach (var image in _symbolImages)
            {
                image.gameObject.SetActive(false);
            }
        }
    }
}