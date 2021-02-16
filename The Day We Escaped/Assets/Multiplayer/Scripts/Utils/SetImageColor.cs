using UnityEngine;
using UnityEngine.UI;

namespace Multiplayer.Scripts.Utils
{
    /// <summary>
    /// To use with UnityEvents, not a good solution, UnityEvents don't accept Color as parameter
    /// so it's been done with int
    /// </summary>
    public class SetImageColor : MonoBehaviour
    {
        //TODO: make a inspectorEditor to create a interface to UnityEvents accept Color

        [SerializeField] private Transform _target;

        public Color[] colors;
        
        private void Start()
        {
            if (_target == null)
            {
                _target = this.transform;
            }
        }

        public void SetColor(int index)
        {
            Image image = null;
            if (_target != null)
            {
                image = _target.GetComponent<Image>();
                if (image != null)
                {
                    SetColorFromColorsField(image, index);
                }
                else
                    Debug.LogWarning($"{this.gameObject} without a target image");
            }
            else
                Debug.LogWarning($"{this.gameObject} without a target image");
        }

        public void SetChildrenColor(int index)
        {
            if (_target == null)
            {
                Debug.LogWarning($"{this.gameObject} without a target image");
                return;
            }

            var images = _target.GetComponentsInChildren<Image>();
            var color = ColorsUtils.ConvertIntToColor((uint) index);
            for (int i = 0; i < images.Length; i++)
            {
               SetColorFromColorsField(images[i], index);
            }
        }

        private void SetColorFromColorsField(Image image, int index)
        {
            if (index >= 0 && index < colors.Length)
                image.color = colors[index];
            else
                Debug.LogWarning($"{this.gameObject} incorrect color index");
        }
    }
}