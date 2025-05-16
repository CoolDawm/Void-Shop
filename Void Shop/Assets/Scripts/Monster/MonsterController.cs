using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;

public class MonsterController : MonoBehaviour
{
    [Header("Patrol Settings")]
    [SerializeField] private List<Transform> _patrolPoints;
    [SerializeField] private float _patrolSpeed = 2f;
    [SerializeField] private float _waitTimeAtPoint = 1f;
    [SerializeField] private float _pointReachedThreshold = 0.5f;

    [Header("Detection Settings")]
    [SerializeField] private float _detectionRadius = 10f;
    [SerializeField] private float _detectionAngle = 90f;
    [SerializeField] private LayerMask _detectionMask;
    [SerializeField] private float _detectionCheckInterval = 0.2f;

    [Header("Chase Settings")]
    [SerializeField] private float _chaseSpeed = 4f;
    [SerializeField] private float _chaseDurationAfterLost = 5f;

    [Header("Attack Settings")]
    [SerializeField] private float _attackRange = 1.5f;
    [SerializeField] private float _attackRate = 1f;
    [SerializeField] private int _attackDamage = 10;

    private NavMeshAgent _agent;
    private Transform _player;
    private Transform _currentPatrolTarget;
    private float _chaseTimer = 0f;
    private float _nextAttackTime = 0f;
    private Vector3 _lastKnownPlayerPosition;
    private bool _playerDetected;
    private bool _isWaiting = false;

    private enum MonsterState { Patrolling, Chasing, Attacking }
    private MonsterState _currentState = MonsterState.Patrolling;

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _player = GameObject.FindGameObjectWithTag("Player").transform;

        if (_patrolPoints.Count == 0)
        {
            Debug.LogWarning("No patrol points assigned to monster!");
            _patrolPoints.Add(new GameObject("DefaultPatrolPoint").transform);
            _patrolPoints[0].position = transform.position;
        }

        _agent.speed = _patrolSpeed;
        _agent.stoppingDistance = _pointReachedThreshold;
        SelectNewPatrolPoint();

        StartCoroutine(DetectionRoutine());
    }

    private IEnumerator DetectionRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(_detectionCheckInterval);
            CheckPlayerVisibility();
        }
    }

    private void Update()
    {
        switch (_currentState)
        {
            case MonsterState.Patrolling:
                UpdatePatrol();
                break;

            case MonsterState.Chasing:
                UpdateChase();
                break;

            case MonsterState.Attacking:
                UpdateAttack();
                break;
        }
    }

    private void UpdatePatrol()
    {
        if (_isWaiting) return;

        if (!_agent.pathPending && _agent.remainingDistance <= _agent.stoppingDistance)
        {
            if (!_agent.hasPath || _agent.velocity.sqrMagnitude == 0f)
            {
                StartCoroutine(WaitAtPoint());
            }
        }
    }

    private IEnumerator WaitAtPoint()
    {
        _isWaiting = true;
        yield return new WaitForSeconds(_waitTimeAtPoint);
        SelectNewPatrolPoint();
        _isWaiting = false;
    }

    private void SelectNewPatrolPoint()
    {
        if (_patrolPoints.Count == 0) return;

        if (_patrolPoints.Count > 1)
        {
            List<Transform> availablePoints = new List<Transform>(_patrolPoints);
            if (_currentPatrolTarget != null) availablePoints.Remove(_currentPatrolTarget);

            _currentPatrolTarget = availablePoints[Random.Range(0, availablePoints.Count)];
        }
        else
        {
            _currentPatrolTarget = _patrolPoints[0];
        }

        _agent.SetDestination(_currentPatrolTarget.position);
    }

    private void CheckPlayerVisibility()
    {
        _playerDetected = false;

        float distanceToPlayer = Vector3.Distance(transform.position, _player.position);
        if (distanceToPlayer > _detectionRadius) return;

        Vector3 directionToPlayer = (_player.position - transform.position).normalized;
        float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);
        if (angleToPlayer > _detectionAngle / 2f) return;

        if (Physics.Raycast(transform.position, directionToPlayer, out RaycastHit hit,
            _detectionRadius, _detectionMask))
        {
            if (hit.transform == _player)
            {
                _playerDetected = true;
                _lastKnownPlayerPosition = _player.position;
                _chaseTimer = 0f;

                if (_currentState == MonsterState.Patrolling)
                {
                    ChangeState(MonsterState.Chasing);
                }
            }
        }
    }

    private void UpdateChase()
    {
        _agent.SetDestination(_lastKnownPlayerPosition);
        _agent.speed = _chaseSpeed;

        if (!_playerDetected)
        {
            _chaseTimer += Time.deltaTime;
            if (_chaseTimer >= _chaseDurationAfterLost)
            {
                ChangeState(MonsterState.Patrolling);
            }
        }

        if (Vector3.Distance(transform.position, _player.position) <= _attackRange)
        {
            ChangeState(MonsterState.Attacking);
        }
    }

    private void UpdateAttack()
    {
        // Поворот к игроку
        Vector3 directionToPlayer = (_player.position - transform.position).normalized;
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            Quaternion.LookRotation(directionToPlayer),
            Time.deltaTime * 10f
        );

        if (Vector3.Distance(transform.position, _player.position) > _attackRange * 1.2f)
        {
            ChangeState(MonsterState.Chasing);
            return;
        }

        if (Time.time >= _nextAttackTime)
        {
            Attack();
            _nextAttackTime = Time.time + 1f / _attackRate;
        }
    }

    private void Attack()
    {
        _player.GetComponent<PlayerHealthUI>().TakeDamage(_attackDamage);
    }

    private void ChangeState(MonsterState newState)
    {
        if (_currentState == newState) return;

        Debug.Log($"State changed from {_currentState} to {newState}");

        switch (_currentState)
        {
            case MonsterState.Attacking:
                _agent.isStopped = false;
                break;
        }

        switch (newState)
        {
            case MonsterState.Patrolling:
                _agent.speed = _patrolSpeed;
                SelectNewPatrolPoint();
                break;

            case MonsterState.Attacking:
                _agent.isStopped = true;
                break;
        }

        _currentState = newState;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _detectionRadius);

        Vector3 leftAngle = Quaternion.Euler(0, -_detectionAngle / 2, 0) * transform.forward * _detectionRadius;
        Vector3 rightAngle = Quaternion.Euler(0, _detectionAngle / 2, 0) * transform.forward * _detectionRadius;
        Gizmos.DrawLine(transform.position, transform.position + leftAngle);
        Gizmos.DrawLine(transform.position, transform.position + rightAngle);

        if (_player != null)
        {
            Gizmos.color = _playerDetected ? Color.red : Color.green;
            Gizmos.DrawLine(transform.position, _player.position);
        }
    }
}