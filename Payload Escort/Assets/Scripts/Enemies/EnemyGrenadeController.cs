using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyGrenadeController : MonoBehaviour, IEnemy
{
    public Transform PlayerTransform;
    public Transform PayloadTransform;
    private PlayerController _playerController;
    [SerializeField] EnemyWeaponGrenade _enemyWeaponGrenade;
    [SerializeField] Animator animator;
    [SerializeField] GameObject _droppableObjectPrefab;
    [SerializeField] LayerMask obstacleLayer;
    [SerializeField] NavMeshAgent _agent;

    private int _maxHp = 10;
    public int _currentHp;
    public int Damage = 7;
    public bool enemyIsDead = false;
    public bool isAttacking = false;
    public float detectionRange = 3f;
    public float attackRange = 2f;
    public float moveSpeed = 1f;
    public float rotationSpeed = 2f;
    private bool isPlayerDetected;

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
        if (/*_agent.destination == transform.position*/!isPlayerDetected)
        {
            DetectTarget();
        }
        else
        {
            MoveTowardsPlayer();
           // MoveNM();
        }
    }
    public void DetectTarget()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRange);

        foreach (Collider col in hitColliders)
        {
            if (col.CompareTag("Player"))
            {
               //_agent.destination = PlayerTransform.position;
                isPlayerDetected = true;
                break;
            }
            /*else if (col.CompareTag("PayloadController"))
            {
                //_agent.destination = PayloadTarget.transform.position;
            }*/
        }
    }

    public void MoveNM()
    {
        float distanceToTarget = Vector3.Distance(transform.position, _agent.destination);
        if (distanceToTarget > attackRange)
        {
            return;
        }
        else
        {
            if (PlayerTransform != null)
            {
                ShootGrenade();
            }
            else
            {
                _enemyWeaponGrenade.EndShot();
                isAttacking = false;
            }
        }
    }
    public void MoveTowardsPlayer()
    {
        // Rotate towards the player
        Vector3 direction = (PlayerTransform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);

        // Move towards the player
        float distanceToPlayer = Vector3.Distance(transform.position, PlayerTransform.position);
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
            if (PlayerTransform != null)
            {
                ShootGrenade();
            }
            else
            {
                _enemyWeaponGrenade.EndShot();
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
    public void ShootGrenade()
    {
        isAttacking = true;
        _enemyWeaponGrenade.StartShot();
    }

    /*public void DrawLandingCircle(Vector3 targetPosition)
    {
        // Draw a red circle on the ground at the target position
        float radius = 0.1f; // Adjust the radius of the circle
        Vector3 circlePosition = new Vector3(targetPosition.x, -0.45f, targetPosition.z);
        DebugDraw.Circle(circlePosition, Vector3.up, Color.red, radius);
    }*/
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
        PlayerTransform = playerTransform;
    }

    public void SetPayloadTarget(Transform payloadTransform)
    {
        PayloadTransform = payloadTransform;
    }
}
