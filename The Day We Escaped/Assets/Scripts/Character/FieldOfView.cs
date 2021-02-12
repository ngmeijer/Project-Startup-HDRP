using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    private float minRange = 0.01f;

    //[Header("FOV")]
    [Range(0, 20)] public float _viewZRange = 5f;
    ////[Range(0, 5)] [SerializeField] private float ViewX = 1f;
    //[Range(0, 100)] [SerializeField] private float _fovRadius = 85f;

    [SerializeField] private Mesh mesh;
    [Range(0, 360)] public float _viewAngle = 50f;

    [SerializeField] private LayerMask targetMask;
    [SerializeField] private LayerMask obstacleMask;

    private List<Transform> visibleTargetsList = new List<Transform>();

    private void findVisibleTargets()
    {

    }

    public Vector3 DirectionFromAngle(float angleInDegs, bool isGlobal)
    {
        if (!isGlobal)
            angleInDegs += transform.eulerAngles.y;

        return new Vector3(Mathf.Sin(Mathf.Deg2Rad * angleInDegs), 0, Mathf.Cos(angleInDegs * Mathf.Deg2Rad));
    }
}