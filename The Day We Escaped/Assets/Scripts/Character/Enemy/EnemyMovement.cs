using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Enemy
{
    public class EnemyMovement : Bolt.EntityBehaviour<IEnemyState>, IObserver
    {
        private EnemyAlertLevel alertLevel;
        private NavMeshAgent _agent;
        private Transform _currentTarget;
        private Transform _targetCandidate;
        private bool _arrivedAtTarget;
        private float _timer;

        [SerializeField]
        [Tooltip("How long should the NPC remain idle at the target point?")]
        [Range(0, 30)]
        private float _delayBeforeMoving;

        [SerializeField]
        [Tooltip("The NPC will not be able to go to a point that's within this range.")]
        [Range(0, 20)]
        private float _minDistanceToNewPoint = 5f;

        [SerializeField]
        [Tooltip("The NPC will not be able to go to a point that's outside this range.")]
        [Range(0, 30)]
        private float _maxDistanceToNewPoint = 15f;

        private List<Transform> _patrolPoints = new List<Transform>();

        [SerializeField]
        [Tooltip("The parent of the patrol points the NPC can go to.")]
        private GameObject _patrolPointsParent;

        private FOVAdvanced _fov;
        [SerializeField] private Transform _meshTransform;
        private bool _entityIsAttached;

        public override void Attached()
        {
            state.SetTransforms(state.EnemyTransform, this.transform);

            _fov = GetComponent<FOVAdvanced>();
            _fov.Attach(this);

            _agent = GetComponent<NavMeshAgent>();
            if (_agent == null)
            {
                this.gameObject.AddComponent<NavMeshAgent>();
                _agent = GetComponent<NavMeshAgent>();
            }

            if (entity.IsOwner && _patrolPointsParent != null)
            {
                findPossibleWaypoints();
                setNewDestination();
            }

            _entityIsAttached = true;
        }

        private void Update()
        {
            if (!_entityIsAttached)
                return;

            if (!BoltNetwork.IsServer || _patrolPointsParent == null)
            {
                return;
            }

            if (checkIfArrived() && _delayBeforeMoving != 0)
            {
                _timer += Time.deltaTime;
            }

            if (_timer >= _delayBeforeMoving && checkIfArrived())
            {
                setNewDestination();
                _timer = 0;
            }

            Debug.DrawLine(transform.position, _currentTarget.position);
        }

        private void findPossibleWaypoints()
        {
            int amountOfWaypoints = _patrolPointsParent.transform.childCount;

            for (int index = 0; index < amountOfWaypoints; index++)
            {
                Transform child = _patrolPointsParent.transform.GetChild(index);
                _patrolPoints.Add(child);
            }
        }

        private void setNewDestination()
        {
            _arrivedAtTarget = false;

            Transform newPoint = findNewTargetPoint();

            if (checkNewTargetPointDistance(newPoint) == false)
            {
                newPoint = findNewTargetPoint();
            }

            _agent.SetDestination(newPoint.position);
        }

        private Transform findNewTargetPoint()
        {
            int index = Random.Range(0, _patrolPoints.Count);

            Transform newTarget = _patrolPoints[index];

            _currentTarget = newTarget;

            return newTarget;
        }

        private bool checkNewTargetPointDistance(Transform newTarget)
        {
            bool withinGivenRanges = false;
            _targetCandidate = newTarget;
            float distance = Vector3.Distance(transform.position, newTarget.position);

            if (distance >= _minDistanceToNewPoint && distance <= _maxDistanceToNewPoint)
            {
                withinGivenRanges = true;
            }

            if (distance < _minDistanceToNewPoint || distance > _maxDistanceToNewPoint)
            {
                findNewTargetPoint();
                withinGivenRanges = false;
            }

            return withinGivenRanges;
        }

        private bool checkIfArrived()
        {
            bool arrived = false;

            float distance = Vector3.Distance(transform.position, _currentTarget.position);

            if (distance < 0.5f)
            {
                arrived = true;
            }

            return arrived;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _minDistanceToNewPoint);

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, _maxDistanceToNewPoint);

            if (_targetCandidate != null)
            {
                Gizmos.color = Color.magenta;
                Gizmos.DrawSphere(_targetCandidate.position, 0.8f);

                foreach (Transform target in _patrolPoints)
                {
                    if (target != _targetCandidate)
                    {
                        Gizmos.color = Color.grey;
                        Gizmos.DrawSphere(target.position, 0.5f);
                    }
                }
            }
        }

        public void UpdateObservers()
        {
            Debug.Log("Player has been detected!");

            switch (_fov.AlertLevel)
            {
                case EnemyAlertLevel.Idle:
                    break;
                case EnemyAlertLevel.Suspicious:
                    break;
                case EnemyAlertLevel.Aware:
                    break;
            }
        }
    }
}