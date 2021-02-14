using System;
using UnityEngine;
using UnityEngine.UI;

public class SetMouseCursor : MonoBehaviour
{
    [Tooltip("FindObjectOfType<MouseCursorUtils>() in Start()")]
    [SerializeField]
    private MouseCursorUtils _mouseCursorUtils;
    private bool _hasMouseCursorUtils;
    
    private void Start()
    {
        _mouseCursorUtils = GameObject.FindObjectOfType<MouseCursorUtils>();
        _hasMouseCursorUtils = _mouseCursorUtils != null;
    }

    public void SetOnHoverCursor()
    {
        if (_hasMouseCursorUtils)
        {
            _mouseCursorUtils.SetOnHoverCursor();
        }
        
        BoltLog.Info($"{this} on enter Cursor");
    }

    public void SetDefaultCursor()
    {
        if (_hasMouseCursorUtils)
        {
            _mouseCursorUtils.SetDefaultCursor();
        }
        
        BoltLog.Info($"{this} on exit Cursor");
    }
}