using System;
using System.Collections.Generic;
using UdpKit;
using UnityEngine;
using UnityEngine.UI;

namespace Multiplayer.Scripts.InGameMapDisplay
{
    public class MapPanelController : MonoBehaviour
    {
        public Image markPrefab;
        public WorldMapToTextureMapConverter _converter;
        
        private Dictionary<Transform, MapMarkController> _marksMap;

        private void Awake()
        {
            _marksMap = new Dictionary<Transform, MapMarkController>();
        }

        public void AddPlayerMark(Transform playerT)
        {
            var mark = AddMapMark(playerT);
            _marksMap.Add(playerT, mark);
        }
        
        public MapMarkController AddMapMark(Transform t)
        {
            var img = Instantiate(markPrefab, transform);
            var mark = img.gameObject.AddComponent<MapMarkController>();

            mark.Converter = _converter;
            mark.Target = t;

            return mark;
        }

        public void RemoveMark(Transform pT)
        {
            if (!_marksMap.TryGetValue(pT, out var mark))
                return;
            
            Destroy(mark.gameObject);
            _marksMap.Remove(pT);
        }

        public Transform debugTarget;

        [ContextMenu("AddDebugTarget")]
        public void AddDebugTarget()
        {
            AddPlayerMark(debugTarget);
        }
    }
}