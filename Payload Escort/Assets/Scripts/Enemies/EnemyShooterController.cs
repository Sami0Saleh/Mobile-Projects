using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyShooterController : MonoBehaviour, IEnemy
{
    private Transform _playerTransform;
    private Transform _payloadTransform;
    private PlayerController _playerController;
    [SerializeField] Animator animator;
    [SerializeField] GameObject _droppableObjectPrefab;
    [SerializeField] EnemyWeapon _enemyWeapon;
    [SerializeField] LayerMask _targetLayer;
    [SerializeField] NavMeshAgent _agent;

    private int _maxHp = 7;
    public int _currentHp;
    public int Damage = 7;
    public bool enemyIsDead = false;
    public bool isAttacking = false;
    public float detectionRange = 10f;
    public float attackRange = 2f;
    public float moveSpeed = 1f;
    public float rotationSpeed = 2f;
    public bool _targetInAttackRange;
    public bool _targetIsInMySight;
    public float _sightRange = 10f;
    public float _attackRange = 2f;
    public float _payloadAttackRange = 5f;

    private void Start()
    {
        _currentHp = _maxHp;
        if (_payloadTransform != null)
        {
            MoveNMToPayload();
        }
    }
    private void Update()
    {
        enemeyState();
    }
    private void enemeyState()
    {
         DetectTarget();
    }
    public void DetectTarget()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRange);

        bool playerDetected = false;
        bool payloadDetected = false;

        foreach (Collider col in hitColliders)
        {
            if (col.CompareTag("Player"))
            {
                playerDetected = true;
            }
            else if (col.CompareTag("payload"))
            {
                payloadDetected = true;
            }
        }

        if (payloadDetected && Vector3.Distance(transform.position, _payloadTransform.position) <= _payloadAttackRange)
        {
            MoveNMToPayload();
        }
        else if (playerDetected && Vector3.Distance(transform.position, _playerTransform.position) <= _attackRange)
        {
            MoveNMToPlayer();
        }
        
    }

    public void MoveNMToPlayer()
    {
        _targetIsInMySight = Physics.CheckSphere(transform.position, _sightRange, _targetLayer);
        _targetInAttackRange = Physics.CheckSphere(transform.position, _attackRange, _targetLayer);

        if (_targetIsInMySight && !_targetInAttackRange)
        {
            _agent.SetDestination(_playerTransform.position);
        }
        else if (_targetIsInMySight && _targetInAttackRange)
        {
            if (_playerTransform != null)
            {
                transform.LookAt(_playerTransform);
                RangeAttackTarget();
            }

            else
            {
                _enemyWeapon.EndShot();
                isAttacking = false;
            }
        }
    }
    public void MoveNMToPayload()
    {
        _targetIsInMySight = Physics.CheckSphere(transform.position, _sightRange, _targetLayer);
        _targetInAttackRange = Physics.CheckSphere(transform.position, _payloadAttackRange, _targetLayer);

        if (_targetIsInMySight && !_targetInAttackRange)
        {
            _agent.SetDestination(_payloadTransform.transform.position);
        }
        else if (_targetIsInMySight && _targetInAttackRange)
        {
            if (_payloadTransform != null)
            {
                transform.LookAt(_payloadTransform);
                RangeAttackTarget();
            }
            else
            {
                _enemyWeapon.EndShot();
                isAttacking = false;
            }
        }
    }
    public void MoveTowardsPlayer()
    {
        // Rotate towards the player
        Vector3 direction = (_playerTransform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);

        // Move towards the player
        float distanceToPlayer = Vector3.Distance(transform.position, _playerTransform.position);
        if (distanceToPlayer > attackRange)
        {
            // Check for obstacles in front of the enemy
            
        }
        /*else
        {
            if (_playerTransform != null)
            {
                RangeAttackTarget();
            }
            
        }*/
    }
    public void RangeAttackTarget()
    {
        _agent.velocity = Vector3.zero;
        isAttacking = true;
        _enemyWeapon.StartShot();
    }
    public void GotHit(int damage)
    {
        _currentHp -= damage;
        if (_currentHp <= 0)
        {
            Die();
        }
    }
    public void Die()
    {
        PlayerController.EnemyCount--;
        DropObjects();
        Destroy(gameObject);
    }
    void DropObjects()
    {
        // Randomize the number of objects dropped
        //int numObjectsToDrop = Random.Range(1, 6);
        int numObjectsToDrop = 53;

        for (int i = 0; i < numObjectsToDrop; i++)
        {
            
            Vector3 dropPosition = new Vector3(transform.position.x + Random.Range(0.01f, 0.3f), 0f, transform.position.z + Random.Range(0.01f,0.3f));
            Instantiate(_droppableObjectPrefab, dropPosition, Quaternion.identity);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "bullet")
        {
            GotHit(_playerController.PlayerStats.Damage);
        }
    }

    public void SetPlayer(PlayerController player)
    {
        _playerController = player;
    }

    public void SetPlayerTransform(Transform playerTransform)
    {
        _playerTransform = playerTransform;
    }

    public void SetPayloadTarget(Transform payloadTransform)
    {
        _payloadTransform = payloadTransform;
    }
}
