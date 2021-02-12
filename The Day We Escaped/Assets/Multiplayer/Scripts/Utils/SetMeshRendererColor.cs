using System;
using System.Linq;
using UnityEngine;

namespace UnityTemplateProjects.Utils
{
    public class SetMeshRendererColor : MonoBehaviour
    {
        public Color color = Color.white;
        
        private void Start()
        {
            var renders = GetComponentsInChildren<Renderer>();
            if (renders.Length > 0)
            {
                renders.ToList().ForEach(r => { r.material.color = color; });
            }
        }
    }
}