using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System.Linq;

public class PlayerController : MonoBehaviour, IDamageable
{
    public PlayerStats playerStats;

    [SerializeField] List<IEnemy> _enemies = new List<IEnemy>();
    private GameObject _currentEnemy;

    [Header("Transform")]
    [SerializeField] Transform _payloadTransform;

    [Header("Camera")]
    public Vector3 CamPosition;
    [SerializeField] Transform _camTransform;

    [Header("Animator")]
    [SerializeField] Animator _animator;

    [Header("Weapon")]
    public PlayerWeapon Weapon;
    
    [SerializeField] FloatingJoystick _joystick;

    [Header("Detection")]
    [SerializeField] LineRenderer _detectionRangeCircle;
    [SerializeField] float _detectionRange;

    [Header("Movement")]
    [SerializeField] float _moveSpeed;
    [SerializeField] bool _isMoving = false;

    [Header("Rotation")]
    [SerializeField] float _rotationSpeed;
    private Quaternion originalrotation;

    public Coins Coin;


    public int Level = 0;

    
    private void Start()
    {
        originalrotation = transform.rotation;
    }
    void Update()
    {
        UpdateDetectionRangeCircle();
        Move();
        DetectEnemy();
    }
    public void Move()
    {
        Vector3 inputDirection = new Vector3(_joystick.Horizontal, 0f, _joystick.Vertical);
        if (inputDirection.magnitude > 0)
        {
            _isMoving = true;
            Quaternion targetRotation = Quaternion.LookRotation(inputDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
            _animator.SetBool("isMoving", true);
        }
        else
        {
            _isMoving = false;
            _animator.SetBool("isMoving", false);
        }

        Vector3 movement = inputDirection * _moveSpeed * Time.deltaTime;
        transform.Translate(movement, Space.World);

        AdjustCam();
        Shoot();
        if (playerStats.IsDoubleShot)
        {
            StartCoroutine(DoubleShot());
        }
        
    }
    public void UpdateDetectionRangeCircle()
    {
        // Calculate points for the detection range circle
        int pointCount = 50; // Number of points to define the circle
        _detectionRangeCircle.positionCount = pointCount;

        for (int i = 0; i < pointCount; i++)
        {
            float angle = (float)i / pointCount * 360f;
            float x = Mathf.Sin(angle * Mathf.Deg2Rad) * _detectionRange;
            float z = Mathf.Cos(angle * Mathf.Deg2Rad) * _detectionRange;
            Vector3 point = transform.position + new Vector3(x, 0.004f, z);
            _detectionRangeCircle.SetPosition(i, point);
        }
    }
    public void DetectEnemy()
    {
        _enemies.Clear();

        float closestDistance = Mathf.Infinity;
        IEnemy closestEnemy = null;

        foreach (IEnemy enemy in IEnemy.EnemyList)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < _detectionRange && distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = enemy;
            }
        }

        if (closestEnemy != null)
        {
            _currentEnemy = closestEnemy.gameObject;
            _enemies.Add(closestEnemy);
        }
        else
        {
            _currentEnemy = null;
        }
    }
    private void Shoot()
    {
        if (_currentEnemy != null)
        {
            if (!_isMoving)
            {
                transform.LookAt(_currentEnemy.transform);
                Weapon.StartShot();
                _animator.SetFloat("AttackSpeed",1);
                _animator.SetBool("isShooting", true);
            }
            else
            {
                Weapon.EndShot();
                _animator.SetBool("isShooting", false);
            }
        }
        else if (_currentEnemy == null)
        {
            if (_isMoving)
            {
                Weapon.EndShot();
                _animator.SetBool("isShooting", false);
            }
            else
            {
                transform.rotation = originalrotation;
                Weapon.EndShot();
                _animator.SetBool("isShooting", false);
            }
        }
    }
    public void TakeMeleeDamage(int damage)
    {
        playerStats.TakeMeleeDamage(damage);
    }
    public void TakeRangedDamage(int damage)
    {
        playerStats.TakeRangeDamage(damage);
    }
    public void RespawnPlayer()
    {
        transform.position = new Vector3(_payloadTransform.position.x - 1f, 0.004f, _payloadTransform.position.z - 2f);
        playerStats.CurrentHP = playerStats.MaxHP;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Coin")
        {
            Coin = other.GetComponent<Coins>();
            playerStats.LevelCoins++;
            playerStats.PlayerLevelXP++;
            playerStats.UpdatePlayerCoins();
            playerStats.UpdatePlayerLevel();
        }
    }
    private void AdjustCam()
    {
        // Update camera position to follow the player
        if (_camTransform != null)
        {
            _camTransform.position = transform.position + CamPosition;
        }
    }
    public IEnumerator DoubleShot()
    {
        new WaitForSecondsRealtime(0.5f);
        Weapon.ReadyToShoot = true;
        Shoot();
        yield return null;
    }
}


