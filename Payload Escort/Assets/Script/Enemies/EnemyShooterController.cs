using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using TMPro;

public class EnemyShooterController : MonoBehaviour, IEnemy , IDamageable
{
    private Transform _playerTransform;
    private Transform _payloadTransform;
    private PlayerController _playerController;
    [SerializeField] Animator animator;
    [SerializeField] GameObject _droppableObjectPrefab;
    [SerializeField] EnemyWeapon _enemyWeapon;
    [SerializeField] LayerMask _targetLayer;
    [SerializeField] NavMeshAgent _agent;
    [SerializeField] TextMeshProUGUI _enemyHPText;
    [SerializeField] Slider _enemyHPSlider;

    private int _maxHp = 7;
    public int _currentHp;
    public bool enemyIsDead = false;
    public bool isAttacking = false;
    public float detectionRange = 10f;
    public float attackRange = 2f;
    public float moveSpeed = 0.7f; // changed from 1 for the build
    public float rotationSpeed = 2f;
    public bool _targetInAttackRange;
    public bool _targetIsInMySight;
    public float _sightRange = 10f;
    public float _attackRange = 2f;
    public float _payloadAttackRange = 5f;

    private void OnEnable()
    {
        IEnemy.EnemyList.Add(this);
    }
    private void OnDestroy()
    {
        IEnemy.EnemyList.Remove(this);
    }
    private void Start()
    {
        _currentHp = _maxHp;
        UpdateHP(_currentHp, _maxHp);
        if (_payloadTransform != null)
        {
            MoveNM(_payloadTransform); 

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
        
        if (Vector3.Distance(transform.position, _payloadTransform.position) <= _payloadAttackRange)
        {
            MoveNM(_payloadTransform);
        }
        else if (Vector3.Distance(transform.position, _playerTransform.position) <= _attackRange)
        {
            MoveNM(_playerTransform);
        }

    }

    public void MoveNM(Transform targetTransform)
    {
        float Distance = Vector3.Distance(transform.position, targetTransform.position);

        if (Distance > attackRange)
        {
            _agent.isStopped = false;
            _agent.SetDestination(targetTransform.position);
        }
        else if (Distance <= attackRange)
        {
            if (targetTransform != null)
            {
                transform.LookAt(targetTransform);
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
        _agent.isStopped = true;
        isAttacking = true;
        _enemyWeapon.StartShot();
    }
    public void TakeRangedDamage(int damage)
    {
        _currentHp -= damage;
        if (_currentHp <= 0)
        {
            Die();
        }
        UpdateHP(_currentHp, _maxHp);
    }
    public void Die()
    {
        DropObjects();
        Destroy(gameObject);
    }
    void DropObjects()
    {
        //Randomize the number of objects dropped
        //int numObjectsToDrop = Random.Range(1, 6);
        int numObjectsToDrop = 25;
        

        for (int i = 0; i < numObjectsToDrop; i++)
        {

            Vector3 dropPosition = new Vector3(transform.position.x + Random.Range(0.01f, 0.3f), 0f, transform.position.z + Random.Range(0.01f, 0.3f));
            Instantiate(_droppableObjectPrefab, dropPosition, Quaternion.identity);
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
    public void UpdateHP(int currentHP, int maxHP)
    {
        _enemyHPText.text = currentHP.ToString();
        _enemyHPSlider.maxValue = maxHP;
        _enemyHPSlider.value = currentHP;
    }
}
