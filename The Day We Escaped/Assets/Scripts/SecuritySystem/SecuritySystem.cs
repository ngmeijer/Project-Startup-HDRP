using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SecuritySystemNS
{
    public class SecuritySystem : MonoBehaviour
    {
        [SerializeField] private Camera _camera;

        [Header("Security Settings")]
        [Range(0.5f, 5f)]
        [SerializeField]
        private float _updateInterval = 1f;

        private float timer;

        private void Start()
        {
            _camera.enabled = false;
        }

        private void Update()
        {
            timer += Time.deltaTime;

            if (timer >= _updateInterval)
            {
                updateMap();
                timer = 0;
            }
        }

        private void updateMap()
        {
            _camera.Render();
        }
    }
}