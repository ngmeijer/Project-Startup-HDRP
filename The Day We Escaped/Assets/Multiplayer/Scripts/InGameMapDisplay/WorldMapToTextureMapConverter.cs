using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Multiplayer.Scripts.InGameMapDisplay
{
    [ExecuteInEditMode]
    public class WorldMapToTextureMapConverter : MonoBehaviour
    {
        public Transform mapWorldOriginPoint;
        public Transform mapWorldEndPoint;

        [SerializeField] private Vector3 _worldOrigin;
        [SerializeField] private Vector3 _worldEnd;

        public Vector2 _mapTextureSize;
        
        private void Start()
        {
            Initialize();
        }

        void Initialize()
        {
            _worldOrigin = mapWorldOriginPoint.position;
            _worldEnd = mapWorldEndPoint.position;
        }

        public Vector2 WorldPositionToMapPosition(Vector3 pWorldPosition)
        {
            //Interpolation from world to Texture
            float worldXTheta = (pWorldPosition.x - _worldOrigin.x) / (_worldEnd.x - _worldOrigin.x);
            float middleXTheta = worldXTheta * (_mapTextureSize.x - 0);
            float pX = middleXTheta + 0;

            //Interpolation from world to Texture for Z/Y, inverse Origin End
            float worldZTheta = (pWorldPosition.z - _worldOrigin.z) / (_worldEnd.z - _worldOrigin.z);
            float middleZTheta = worldZTheta * (_mapTextureSize.y - 0);
            float pY = middleZTheta + 0;

            return new Vector2(pX, pY);
        }
    }
}