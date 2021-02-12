using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SecuritySystemNS
{
    public class SecuritySystem : MonoBehaviour
    {
        [SerializeField] private Camera _camera;

        [Header("Security Settings")]
        [Range(0.001f, 2f)]
        [SerializeField]
        private float _updateInterval = 1f;

        private float timer;

        private void Update()
        {
            //Meh, could also use a coroutine with delay for this, what do you want?
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