using UnityEditor;
using UnityEngine;
using UnityEngine.U2D;
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
            
            return c;
        }

        public static Image CreateImageWithSprite(string pName, string pSpriteName, Vector2 pPos, Vector2 pSize, SpriteAtlas pAtlas, Canvas pCanvas)
        {
            var imageGameObject = new GameObject(pName, typeof(Image));
            var image = imageGameObject.GetComponent<Image>();
            image.rectTransform.SetParent(pCanvas.transform);
            image.rectTransform.localPosition = Vector3.zero;
            image.rectTransform.sizeDelta = Vector2.one * 8;
                
            var sprite = pAtlas.GetSprite(pSpriteName);
            image.sprite = sprite;
                
            image.raycastTarget = false;

            return image;
        }
    }
}