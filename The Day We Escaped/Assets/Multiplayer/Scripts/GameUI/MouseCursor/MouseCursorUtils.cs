using System;
using System.Collections;
using System.Collections.Generic;
using Multiplayer.Scripts.Utils;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class MouseCursorUtils : MonoBehaviour
{
    public Image crossHairImage;
    public Image onHoverCrossHairImage;

    public Texture2D onHoverCursorIcon;

    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 cursorHotspot = Vector2.zero;
    
    private void Start()
    {
       
    }
    
    [ContextMenu("ShowMouseCursor")]
    public void ShowMouseCursor()
    {
        Cursor.visible = true;
    }

    [ContextMenu("HideMouseCursor")]
    public void HideMouseCursor()
    {
        Cursor.visible = false;
    }

    [ContextMenu("UseCrossHair")]
    public void UseCrossHair()
    {
        HideMouseCursor();
        crossHairImage.gameObject.SetActive(true);
        LockCursor(true);
    }

    [ContextMenu("UseMouseCursor")]
    public void UseMouseCursor()
    {
        ShowMouseCursor();
        crossHairImage.gameObject.SetActive(false);
        ConfineCursor(false);
    }

    public void LockCursor(bool val)
    {
        Cursor.lockState = val ? CursorLockMode.Locked : CursorLockMode.None;
    }

    public void ConfineCursor(bool val)
    {
        Cursor.lockState = val ? CursorLockMode.Confined : CursorLockMode.None;
    }

    public void SetOnHoverCursor()
    {
        if (IsCursorLocked())
        {
            SetOnHoverCrossHair();
        }
        else if (Cursor.lockState == CursorLockMode.None)
        {
            Cursor.SetCursor(onHoverCursorIcon, cursorHotspot, cursorMode);
        }
    }

    public void SetDefaultCursor()
    {
        if (IsCursorLocked())
        {
            SetDefaultCrossHair();
        }
        else if (Cursor.lockState == CursorLockMode.None)
        {
            Cursor.SetCursor(null, Vector2.zero, cursorMode);
        }
    }

    private void SetOnHoverCrossHair()
    {
        crossHairImage.gameObject.SetActive(false);
        onHoverCrossHairImage.gameObject.SetActive(true);
    }

    private void SetDefaultCrossHair()
    {
        crossHairImage.gameObject.SetActive(true);
        onHoverCrossHairImage.gameObject.SetActive(false);
    }

    bool IsCursorLocked()
    {
        return Cursor.lockState == CursorLockMode.Locked || Cursor.lockState == CursorLockMode.Confined;
    }
}