using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Multiplayer.Scripts.Utils
{
    public class CanvasUtils
    {
        public static Canvas CreateCanvas(string pName = "Canvas")
        {
            GameObject newCanvas = new GameObject(pName);
            newCanvas.layer = LayerMask.NameToLayer("UI");
            
            Canvas c = newCanvas.AddComponent<Canvas>();
            c.renderMode = RenderMode.ScreenSpaceOverlay;
            
            var cs = newCanvas.AddComponent<CanvasScaler>();
            cs.referenceResolution = new Vector2(1920, 1080);
            cs.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
            cs.matchWidthOrHeight = 0;
            
            newCanvas.AddComponent<GraphicRaycaster>();

            GameObjectUtility.EnsureUniqueNameForSibling(newCanvas.gameObject);

            return c;
        }
    }
}