using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public enum EnemyAlertLevel
{
    Idle,
    Suspicious,
    Aware,
};

public class FOVAdvanced : MonoBehaviour, ISubject
{
    private EnemyAlertLevel _alertLevel;
    private Camera _camera;
    private List<Collider> _playersFound = new List<Collider>();
    private Light _light;
    private bool _foundTarget;

    private float _timer;
    public List<IObserver> _observerList { get; } = new List<IObserver>();

    public EnemyAlertLevel AlertLevel { get; private set; }

    [Header("Security settings")]
    [SerializeField] [Range(0, 5)] private float _dangerDelay;

    [Header("Light settings")]
    [SerializeField] [Range(1, 20)] private float _range;
    [SerializeField] private float _intensity = 20f;

    private void Awake()
    {
        _camera = GetComponentInChildren<Camera>();
        if (_camera == null)
            this.gameObject.AddComponent<Camera>();

        _light = GetComponentInChildren<Light>();
        if (_light == null)
        {
            Transform child = this.gameObject.transform.GetChild(0);
            child.gameObject.AddComponent<Light>();
            _light = GetComponentInChildren<Light>();
        }
    }

    private void Start()
    {
        _light.range = _range;
        _light.intensity = _intensity;
        _camera.farClipPlane = _range;

        findTargetsInScene();
    }

    private void Update()
    {
        handleAlertState();
    }

    private void findTargetsInScene()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
        {
            Collider col = player.GetComponent<Collider>();
            _playersFound.Add(col);
        }
    }

    private bool isTargetInView()
    {
        Transform target = null;
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(_camera);

        foreach (Collider playerCollider in _playersFound)
        {
            target = playerCollider.transform;
            if (GeometryUtility.TestPlanesAABB(planes, playerCollider.bounds))
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, (playerCollider.transform.position - transform.position), out hit))
                {
                    if (hit.collider.gameObject.CompareTag("Player"))
                    {
                        _foundTarget = true;
                    }
                }
            }
            else
            {
                _foundTarget = false;
            }
        }

        if (target != null)
        {
            if (Vector3.Distance(transform.position, target.position) <= _range)
                if (_foundTarget)
                {
                    Debug.DrawLine(transform.position, target.position, Color.red);
                }
                else
                {
                    Debug.DrawLine(transform.position, target.position, Color.green);
                }
        }

        return _foundTarget;
    }

    private void handleAlertState()
    {
        if (isTargetInView() == true)
        {
            _timer += Time.deltaTime;
            if (_timer < _dangerDelay)
            {
                _alertLevel = EnemyAlertLevel.Suspicious;
                _light.color = Color.yellow;
            }

            if (_timer > _dangerDelay)
            {
                _alertLevel = EnemyAlertLevel.Aware;
                _light.color = Color.red;
            }
        }
        else
        {
            _light.color = Color.white;
            _alertLevel = EnemyAlertLevel.Idle;
            _light.color = Color.white;
            _timer = 0;
        }
    }

    public void Attach(IObserver pObserver)
    {
        Debug.Log($"ISubject {this.GetType()}: Observer has been attached.");
        _observerList.Add(pObserver);
    }

    public void Detach(IObserver pObserver)
    {
        Debug.Log($"ISubject {this.GetType()}: Observer has been detached.");
        _observerList.Remove(pObserver);
    }

    public void NotifyObservers()
    {
        Debug.Log($"ISubject {this.GetType()}: all Observers are being updated.");
        foreach (IObserver observer in _observerList)
        {
            observer.UpdateObservers();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, _range);
    }

    #region 

    //private enum FOVDirection
    //{
    //    Horizontal,
    //    Vertical,
    //};

    //[SerializeField] private FOVDirection FOVDir;

    //[SerializeField] [Range(1, 50)] private int _amountOfRaysHorizontal;
    //[SerializeField] [Range(2, 25)] private int _amountOfRaysVertical;

    //private int usedAmountOfRays;

    //[SerializeField] [Range(1, 20)] private float _viewZRange;
    //[SerializeField] [Range(1, 100)] private float _fov;

    //[SerializeField] [Range(-0.05f, 0.05f)] private float _scanSpeed;

    //[SerializeField] private Mesh mesh;

    //[SerializeField] private LayerMask _targetMask;
    //[SerializeField] private LayerMask _obstacleMask;

    //[SerializeField] private Camera cam;

    //private float distanceBetweenRaysX;
    //private float distanceBetweenRaysY;

    //private float oldPosX;
    //private float oldPosY;

    //private Rect frustumSize;
    //[SerializeField] private List<Vector3> directionsList = new List<Vector3>();
    //public Vector3 direction = new Vector3();
    //private float oldRotationY;

    //private void Start()
    //{
    //    Vector3 topLeftFrustumPos = new Vector3();
    //}

    ////Only for testing & scene view. 
    //private void OnDrawGizmos()
    //{
    //    Gizmos.matrix = transform.localToWorldMatrix;

    //    cam.farClipPlane = _viewZRange;
    //    cam.fieldOfView = _fov;

    //    float frustumHeight = 1.0f * _viewZRange * Mathf.Tan(cam.fieldOfView * 0.5f * Mathf.Deg2Rad);
    //    float frustumWidth = frustumHeight * cam.aspect;

    //    //Y-axis, TOP & BOTTOM
    //    frustumSize.yMin = frustumHeight;
    //    frustumSize.yMax = -frustumHeight;

    //    //X-axis LEFT & RIGHT
    //    frustumSize.xMin = -frustumWidth;
    //    frustumSize.xMax = frustumWidth;

    //    Gizmos.color = Color.red;

    //    switch (FOVDir)
    //    {
    //        case FOVDirection.Horizontal:
    //            oldPosX = -frustumWidth;
    //            distanceBetweenRaysX = (frustumWidth * 2) / _amountOfRaysHorizontal;

    //            oldPosY -= _scanSpeed;

    //            for (int x = 0; x < _amountOfRaysHorizontal; x++)
    //            {
    //                oldPosX += distanceBetweenRaysX;
    //                Vector3 directionVector3 = new Vector3(oldPosX, oldPosY, _viewZRange);
    //                Gizmos.DrawLine(Vector3.zero, directionVector3);
    //            }

    //            if (oldPosY <= frustumSize.yMax || oldPosY >= frustumSize.yMin)
    //                _scanSpeed *= -1;

    //            Gizmos.DrawLine(Vector3.zero, new Vector3(-frustumWidth, oldPosY, _viewZRange));
    //            Gizmos.DrawLine(Vector3.zero, new Vector3(frustumWidth, oldPosY, _viewZRange));
    //            break;

    //        case FOVDirection.Vertical:
    //            oldPosY = -frustumHeight * 2;

    //            distanceBetweenRaysY = (frustumHeight * 4) / _amountOfRaysVertical;

    //            oldPosX -= _scanSpeed;

    //            for (int y = 0; y < _amountOfRaysVertical; y++)
    //            {
    //                oldPosY += distanceBetweenRaysY;
    //                Vector3 direction = new Vector3(oldPosY, oldPosY, _viewZRange);
    //                Gizmos.DrawLine(Vector3.zero, direction);
    //            }


    //            if (oldPosX <= frustumSize.xMin || oldPosX >= frustumSize.xMax)
    //                _scanSpeed *= -1;

    //            Gizmos.DrawLine(Vector3.zero, new Vector3(oldPosX, frustumHeight * 2, _viewZRange));
    //            Gizmos.DrawLine(Vector3.zero, new Vector3(oldPosX, -frustumHeight * 2, _viewZRange));
    //            break;
    //    }
    //}

    //private void Update()
    //{
    //    findVisibleTargets();
    //}

    //private void findVisibleTargets()
    //{
    //    float newRotationY = transform.rotation.y * Mathf.Rad2Deg;

    //    float degreesMoved = newRotationY - oldRotationY;

    //    float yPos = frustumSize.yMin;

    //    Vector3 currentRotation = rotateAroundPoint(transform.position, direction, degreesMoved);

    //    Debug.DrawRay(transform.position,  new Vector3(transform.localRotation.x, transform.localRotation.y, transform.localRotation.z) * 3, Color.cyan);

    //    oldRotationY = transform.rotation.y * Mathf.Rad2Deg;

    //    ////Find local position of a point on the frustum to find the correct position for a ray/line!
    //    //Debug.DrawLine(transform.position, transform.localPosition + directionsList[0], Color.green);
    //}

    //private Vector3 rotateAroundPoint(Vector3 sun, Vector3 planet, float angle)
    //{
    //    angle *= Mathf.Deg2Rad;

    //    float x = Mathf.Cos(angle) * (sun.x - planet.x) - Mathf.Sin(angle) * (sun.y - planet.y) + planet.x;
    //    float y = Mathf.Sin(angle) * (sun.x - planet.x) - Mathf.Cos(angle) * (sun.y - planet.y) + planet.y;

    //    return new Vector3(x, y);
    //}

    #endregion
}