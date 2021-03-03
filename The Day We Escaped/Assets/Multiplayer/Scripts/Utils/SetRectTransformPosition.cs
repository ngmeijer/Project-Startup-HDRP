using System;
using UnityEngine;

namespace Multiplayer.Scripts.Utils
{
    public class SetRectTransformPosition : MonoBehaviour
    {
        [SerializeField] private RectTransform _thisRect;

        private void Start()
        {
            if (_thisRect == null)
            {
                _thisRect = GetComponent<RectTransform>();
            }
        }

        public void SetPositionAtTransform(Transform pTransform)
        {
            _thisRect.position = pTransform.position;
        }
    }
}