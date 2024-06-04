using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LittelGrabberController : MonoBehaviour, IEnemy
{
    private Transform _playerTransform;
    private Transform _payloadTransform;
    private PlayerController _playerController;
    [SerializeField] Animator animator;
    [SerializeField] LayerMask _player;
    [SerializeField] LayerMask obstacleLayer;
    [SerializeField] GameObject _droppableObjectPrefab;

    private int _maxHp = 5;
    public int _currentHp;
    public int Damage = 3;
    public bool enemyIsDead = false;
    public bool isAttacking = false;
    public float detectionRange = 3f;
    public float attackRange = 0.1f;
    public float moveSpeed = 0.5f;
    public float rotationSpeed = 2f;
    public bool isAttackFinished = true;
    private bool isPlayerDetected;

    private void Start()
    {
        _currentHp = _maxHp;
    }
    private void Update()
    {
        if (!PlayerController.IsplayerDead)
        {
            enemeyState();
        }
    }
    private void enemeyState()
    {
        if (isAttackFinished)
        {
            animator.SetBool("isAttacking", false);
        }
        if (!isPlayerDetected)
        {
            DetectTarget();
        }
        else
        {
            MoveTowardsPlayer();
        }
    }
    public void DetectTarget()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRange);

        foreach (Collider col in hitColliders)
        {
            if (col.CompareTag("Player"))
            {
                isPlayerDetected = true;
                break;
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
            if (!IsObstacleInPath())
            {
                transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
            }
        }
        else
        {
            if (_playerTransform != null)
            {
                isAttackFinished = false;
                animator.SetBool("isAttacking", true);
            }
            else
            {
                isAttacking = false;
            }
        }
    }
    public bool IsObstacleInPath()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, detectionRange, obstacleLayer))
        {
            return true;
        }
        return false;
    }
    public void GotHit(int damage)
    {
        _currentHp -= damage;
        if (_currentHp <= 0)
        {
            Die();
        }
    }
    void DropObjects()
    {
        // Randomize the number of objects dropped
        //int numObjectsToDrop = Random.Range(1, 6);
        int numObjectsToDrop = 53;

        for (int i = 0; i < numObjectsToDrop; i++)
        {

            Vector3 dropPosition = new Vector3(transform.position.x + Random.Range(0.01f, 0.3f), 0f, transform.position.z + Random.Range(0.01f, 0.3f));
            Instantiate(_droppableObjectPrefab, dropPosition, Quaternion.identity);
        }
    }
    public void Die()
    {
        PlayerController.EnemyCount--;
        DropObjects();
        Destroy(gameObject);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            _playerController.TakeMeleeDamage(Damage);
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            _playerController.TakeMeleeDamage(Damage);
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
