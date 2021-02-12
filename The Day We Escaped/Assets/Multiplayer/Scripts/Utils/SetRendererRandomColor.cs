using System;
using System.Linq;
using UnityEngine;

namespace UnityTemplateProjects.Utils
{
    public class SetRendererRandomColor : MonoBehaviour
    {
        private void Start()
        {
            var renders = GetComponentsInChildren<Renderer>();
            if (renders.Length > 0)
            {
                var randColor = ColorsUtils.GetRandomColor();
                renders.ToList().ForEach(r => { r.material.color = randColor; });
            }
        }
    }
}