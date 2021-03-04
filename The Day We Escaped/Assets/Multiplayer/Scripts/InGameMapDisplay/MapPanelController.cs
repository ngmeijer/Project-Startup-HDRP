using System;
using System.Collections.Generic;
using Multiplayer.Scripts.Observers;
using Multiplayer.Scripts.Utils;
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
            LocalEvents.instance.AddListener<PlayerLocalEvent>(PlayerLocalEventHandler);
            LocalEvents.instance.AddListener<EnemyLocalEvent>(EnemyLocalEventHandler);
        }

        public void AddPlayerMark(Transform playerT)
        {
            var c = ColorsUtils.GetRandomColorExcept(Color.red);

            var mark = AddMapMark(playerT, c);
            _marksMap.Add(playerT, mark);
        }

        public void AddEnemyMArk(Transform enemyT)
        {
            var mark = AddMapMark(enemyT, Color.red);
            _marksMap.Add(enemyT, mark);
        }

        public MapMarkController AddMapMark(Transform t, Color pColor)
        {
            var img = Instantiate(markPrefab, transform);
            img.color = pColor;

            var mark = img.gameObject.AddComponent<MapMarkController>();

            mark.Converter = _converter;
            mark.Target = t;

            return mark;
        }

        public void RemoveMapMark(Transform pT)
        {
            if (!_marksMap.TryGetValue(pT, out var mark))
                return;

            Destroy(mark.gameObject);
            _marksMap.Remove(pT);
        }

        private void OnDestroy()
        {
            LocalEvents.instance.RemoveListener<PlayerLocalEvent>(PlayerLocalEventHandler);
            LocalEvents.instance.RemoveListener<EnemyLocalEvent>(EnemyLocalEventHandler);
        }

        private void PlayerLocalEventHandler(PlayerLocalEvent e)
        {
            switch (e.eventType)
            {
                case PlayerLocalEvent.EventType.Attached:
                    AddPlayerMark(e.playerGameObject.transform);
                    break;
                case PlayerLocalEvent.EventType.Detached:
                    RemoveMapMark(e.playerGameObject.transform);
                    break;
                default:
                    break;
            }
        }

        private void EnemyLocalEventHandler(EnemyLocalEvent e)
        {
            switch (e.eventType)
            {
                case EnemyLocalEvent.EventType.Attached:
                    AddEnemyMArk(e.enemyGameObject.transform);
                    break;
                case EnemyLocalEvent.EventType.Detached:
                    RemoveMapMark(e.enemyGameObject.transform);
                    break;
                default:
                    break;
            }
        }
    }
}