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

public class FOVAdvanced : MonoBehaviour, ISubject, IObserverCB
{
    private EnemyAlertLevel _alertLevel;
    private Camera _camera;
    [SerializeField] private List<Collider> _playerColliders = new List<Collider>();
    private Light _light;
    private bool _foundTarget;
    private Transform _target;

    private float _timer;

    private FindPlayersBoltCallback _findPlayersCB;

    [Header("Security settings")]
    [SerializeField] [Range(0, 5)] private float _dangerDelay;

    [Header("Light settings")]
    [SerializeField] [Range(1, 20)] private float _range;
    [SerializeField] private float _intensity = 20f;

    public List<IObserver> _observerList { get; } = new List<IObserver>();

    public EnemyAlertLevel AlertLevel { get; private set; }

    private void Awake()
    {
        _camera = GetComponentInChildren<Camera>(true);
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

    private IEnumerator Start()
    {
        _light.range = _range;
        _light.intensity = _intensity;
        if (_camera != null)
        {
            _camera.farClipPlane = _range;
        }

        yield return new WaitForEndOfFrame();

        _findPlayersCB = FindObjectOfType<FindPlayersBoltCallback>();

        while (_findPlayersCB == null)
        {
            yield return null;
            _findPlayersCB = FindObjectOfType<FindPlayersBoltCallback>();
        }

        _findPlayersCB.AttachPlayerNetwork(this);
    }

    private void Update()
    {
        handleAlertState();
    }

    private bool isTargetInView()
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(_camera);

        _foundTarget = false;
        foreach (Collider playerCollider in _playerColliders)
        {
            _target = playerCollider.transform;
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
        }

        if (_target != null)
        {
            if (Vector3.Distance(transform.position, _target.position) <= _range)
                if (_foundTarget)
                {
                    Debug.DrawLine(transform.position, _target.position, Color.red);
                }
                else
                {
                    Debug.DrawLine(transform.position, _target.position, Color.green);
                }
        }

        return _foundTarget;
    }

    private void handleAlertState()
    {
        if (isTargetInView())
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

            NotifyObservers();
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

    public void ReceiveNetworkUpdate(List<GameObject> pPlayerList)
    {
        foreach (var player in pPlayerList)
        {
            Collider col = player.GetComponent<Collider>();
            if (!_playerColliders.Contains(col))
            {
                _playerColliders.Add(col);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, _range);
    }
}