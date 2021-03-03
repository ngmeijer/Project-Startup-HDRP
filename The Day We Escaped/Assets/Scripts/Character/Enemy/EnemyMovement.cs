using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Enemy
{
    public class EnemyMovement : Bolt.EntityBehaviour<IEnemyState>, IObserver
    {
        private EnemyAlertLevel alertLevel;
        private NavMeshAgent agent;
        private Transform lastTarget;
        private Transform currentTarget;
        private Transform nextTarget;
        private bool arrivedAtTarget;
        private float timer;

        [SerializeField]
        [Tooltip("How long should the NPC remain idle at the target point?")]
        [Range(0, 30)]
        private float delayBeforeMoving;

        [SerializeField]
        [Tooltip("The NPC will not be able to go to a point that's within this range.")]
        [Range(0, 20)]
        private float minDistanceToNewPoint = 5f;

        [SerializeField]
        [Tooltip("The NPC will not be able to go to a point that's outside this range.")]
        [Range(0, 30)]
        private float maxDistanceToNewPoint = 5f;

        private List<Transform> _patrolPoints = new List<Transform>();

        [SerializeField]
        [Tooltip("The parent of the patrol points the NPC can go to.")]
        private GameObject _patrolPointsParent;

        private FOVAdvanced fov;
        [SerializeField] private Transform _karlTransform;
        private bool _entityIsAttached;

        public override void Attached()
        {
            state.SetTransforms(state.EnemyTransform, this.transform);

            fov = GetComponent<FOVAdvanced>();
            fov.Attach(this);

            agent = GetComponent<NavMeshAgent>();
            if (agent == null)
            {
                this.gameObject.AddComponent<NavMeshAgent>();
                agent = GetComponent<NavMeshAgent>();
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

            if (checkIfArrived() && delayBeforeMoving != 0)
            {
                timer += Time.deltaTime;
            }

            if (timer >= delayBeforeMoving && checkIfArrivedAtTarget())
            {
                setNewDestination();
                timer = 0;
            }

            Debug.DrawLine(transform.position, currentTarget.position);
        }

        private void findPossibleWaypoints()
        {
            //Only executed at start.
            int amountOfWaypoints = _patrolPointsParent.transform.childCount;

            for (int index = 0; index < amountOfWaypoints; index++)
            {
                Transform child = _patrolPointsParent.transform.GetChild(index);
                _patrolPoints.Add(child);
            }
        }

        private void setNewDestination()
        {
            arrivedAtTarget = false;

            lastTarget = currentTarget;

            Transform newPoint = findNewTargetPoint();

            if (checkNewTargetPointDistance(newPoint) == false)
            {
                newPoint = findNewTargetPoint();
            }

            currentTarget = newPoint;

            agent.SetDestination(newPoint.position);
        }

        private Transform findNewTargetPoint()
        {
            int index = Random.Range(0, _patrolPoints.Count);

            Transform newTarget = _patrolPoints[index];

            //currentTarget = newTarget;

            return newTarget;
        }

        private bool checkNewTargetPointDistance(Transform pNewTarget)
        {
            bool withinGivenRanges = false;
            nextTarget = pNewTarget;
            float distance = Vector3.Distance(transform.position, pNewTarget.position);

            if (distance >= minDistanceToNewPoint && distance <= maxDistanceToNewPoint)
            {
                withinGivenRanges = true;
            }

            if (distance < minDistanceToNewPoint || distance > maxDistanceToNewPoint)
            {
                findNewTargetPoint();
                withinGivenRanges = false;
            }

            return withinGivenRanges;
        }

        private bool checkNewTargetHistory(Transform pLastTarget)
        {
            bool wasPlayersLastTarget = false;



            return wasPlayersLastTarget;
        }

        private bool checkIfArrivedAtTarget()
        {
            bool arrived = false;

            float distance = Vector3.Distance(transform.position, currentTarget.position);

            if (distance < 0.5f)
            {
                arrived = true;
            }

            return arrived;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, minDistanceToNewPoint);

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, maxDistanceToNewPoint);

            if (nextTarget != null)
            {
                Gizmos.color = Color.magenta;
                Gizmos.DrawSphere(nextTarget.position, 1f);

                foreach (Transform target in _patrolPoints)
                {
                    if (target != nextTarget)
                    {
                        Gizmos.color = Color.grey;
                        Gizmos.DrawSphere(target.position, 0.3f);
                    }
                }
            }
        }

        public void UpdateObservers()
        {
            Debug.Log("Player has been detected!");

            switch (fov.AlertLevel)
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