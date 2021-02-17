using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SecuritySystemNS
{
    public class ControlRoomCodeMonitor : MonoBehaviour
    {
        [SerializeField] private List<Sprite> _iconSpriteList = new List<Sprite>();
        [SerializeField] private Image _monitorImage;
        [SerializeField] private float _switchInterval = 1f;

        private float _timer;
        private int currentImageIndex = 0;

        private void Start()
        {
            _monitorImage.sprite = _iconSpriteList[currentImageIndex];
        }

        private void Update()
        {
            _timer += Time.deltaTime;

            if (_timer >= _switchInterval)
            {
                currentImageIndex++;

                if (currentImageIndex > _iconSpriteList.Count - 1)
                {
                    currentImageIndex = 0;
                }

                _timer = 0;
                _monitorImage.sprite = _iconSpriteList[currentImageIndex];
            }
        }
    }
}
