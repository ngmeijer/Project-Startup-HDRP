using System;
using UnityEngine;

namespace Multiplayer.Scripts.InGameMapDisplay
{
    public class MapMarkController : MonoBehaviour
    {
        [SerializeField] private RectTransform _rect;

        [SerializeField] private WorldMapToTextureMapConverter _converter;
        private bool _isConverterNull;

        [SerializeField] private Transform _target;
        private bool _isTargetNull;

        private void Start()
        {
            if (_rect == null)
                _rect = GetComponent<RectTransform>();
        }

        private void Update()
        {
            if (_isConverterNull || _isTargetNull)
                return;
            
            var pos2D = _converter.WorldPositionToMapPosition(_target.position);
            _rect.localPosition = new Vector3(pos2D.x, pos2D.y, 0);
        }

        public WorldMapToTextureMapConverter Converter
        {
            get => _converter;
            set
            {
                _converter = value;
                _isConverterNull = value == null;
            }
        }
        
        public Transform Target
        {
            get => _target;
            set
            {
                _target = value;
                _isTargetNull = value == null;
            }
        }
    }
}