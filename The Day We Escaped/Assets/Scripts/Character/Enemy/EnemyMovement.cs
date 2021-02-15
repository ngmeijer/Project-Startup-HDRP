using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Enemy
{
    public class EnemyMovement : MonoBehaviour, IObserver
    {
        private EnemyAlertLevel alertLevel;
        private NavMeshAgent agent;
        private Transform currentTarget;
        private Transform targetCandidate;
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

        private void Start()
        {
            fov = GetComponent<FOVAdvanced>();
            fov.Attach(this);

            agent = GetComponent<NavMeshAgent>();
            if (agent == null)
            {
                this.gameObject.AddComponent<NavMeshAgent>();
                agent = GetComponent<NavMeshAgent>();
            }

            findPossibleWaypoints();

            setNewDestination();
        }

        private void Update()
        {
            //Choose random point from list of patrolPoints. Then based on which one was chosen, choose a new one after arriving on point, or after certain duration.

            if (checkIfArrived() && delayBeforeMoving != 0)
            {
                timer += Time.deltaTime;
            }

            if (timer >= delayBeforeMoving && checkIfArrived())
            {
                setNewDestination();
                timer = 0;
            }

            Debug.DrawLine(transform.position, currentTarget.position);
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
            arrivedAtTarget = false;

            Transform newPoint = findNewTargetPoint();

            while (checkNewTargetPointDistance(newPoint) == false)
            {
                newPoint = findNewTargetPoint();
            }

            agent.SetDestination(newPoint.position);
        }

        private Transform findNewTargetPoint()
        {
            int index = Random.Range(0, _patrolPoints.Count);

            Transform newTarget = _patrolPoints[index];

            currentTarget = newTarget;

            return newTarget;
        }

        private bool checkNewTargetPointDistance(Transform newTarget)
        {
            bool withinGivenRanges = false;
            targetCandidate = newTarget;
            float distance = Vector3.Distance(transform.position, newTarget.position);

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

        private bool checkIfArrived()
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

            if (targetCandidate != null)
            {
                Gizmos.color = Color.magenta;
                Gizmos.DrawSphere(targetCandidate.position, 0.8f);

                foreach (Transform target in _patrolPoints)
                {
                    if (target != targetCandidate)
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
        }
    }
}