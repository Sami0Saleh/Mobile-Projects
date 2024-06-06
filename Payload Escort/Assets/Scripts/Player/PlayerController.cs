using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System.Linq;

public class PlayerController : MonoBehaviour ,IDamageable
{
    public PlayerStats PlayerStats;
    public Vector3 CamPosition;
    [SerializeField] List<IEnemy> _enemies = new List<IEnemy>();
    [SerializeField] Transform _playerTransform;
    [SerializeField] Transform _payloadTransform;
    [SerializeField] Transform _camTransform;
    [SerializeField] Animator _anim;
    [SerializeField] GameObject _pistol;
    [SerializeField] GameObject _assaultRifle;
    public PlayerWeapon Weapon;
    [SerializeField] FloatingJoystick _joystick;
    //[SerializeField] UpgradeSpawner _upgradeSpawner;
    [SerializeField] LayerMask _enemyLayer;
    [SerializeField] LineRenderer _detectionRangeCircle;
    [SerializeField] float _detectionRange;
    [SerializeField] float _moveSpeed;
    [SerializeField] float _rotationSpeed;
    [SerializeField] bool _isMoving = false;

    private GameObject _currentEnemy;
    public Coins Coin;

    private Vector3 _moveDirection;
    private Quaternion originalrotation;

    private int _weaponIndex;
    public int Level = 0;
    public static int EnemyCount = 3;
    public static bool IsplayerDead;

    private void Awake()
    {
        Time.timeScale = 1.0f;
    }
    private void Start()
    {
        originalrotation = transform.rotation;
        _weaponIndex = 0;
        StartCoroutine(SwitchWeapons());
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
            _anim.SetBool("isFWD", true);
        }
        else
        {
            _isMoving = false;
            _anim.SetBool("isFWD", false);
        }

        Vector3 movement = inputDirection * _moveSpeed * Time.deltaTime;
        transform.Translate(movement, Space.World);
        // Update camera position to follow the player
        if (_camTransform != null)
        {
            _camTransform.position = transform.position + CamPosition;
        }
        if (_currentEnemy != null)
        {
            if (!_isMoving)
            {
                transform.LookAt(_currentEnemy.transform);
                Weapon.StartShot();
                _anim.SetBool("isShooting", true);
            }
            else
            {
                Weapon.EndShot();
                _anim.SetBool("isShooting", false);
            }
        }
        else if (_currentEnemy == null)
        {
            if (_isMoving)
            {
                Weapon.EndShot();
                _anim.SetBool("isShooting", false);
            }
            else
            {
                transform.rotation = originalrotation;
                Weapon.EndShot();
                _anim.SetBool("isShooting", false);
            }
        }

        if (Input.GetKey(KeyCode.Alpha1))
        {
            _weaponIndex = 0;
            _anim.SetBool("isPistol", true);
            _anim.SetBool("isAssaultRifle", false);
            StartCoroutine(SwitchWeapons());
        }
        if (Input.GetKey(KeyCode.Alpha2))
        {
            _weaponIndex = 1;
            _anim.SetBool("isAssaultRifle", true);
            _anim.SetBool("isPistol", false);
            StartCoroutine(SwitchWeapons());
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
    public void TakeMeleeDamage(int damage)
    {
        PlayerStats.TakeMeleeDamage(damage);
    }
    public void TakeRangedDamage(int damage)
    {
        PlayerStats.TakeRangeDamage(damage);
    }
    public void RespawnPlayer()
    {
        transform.position = new Vector3(_payloadTransform.position.x - 1f, 0.004f, _payloadTransform.position.z - 2f);
        PlayerStats.CurrentHP = PlayerStats.MaxHP;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Coin")
        {
            Coin = other.GetComponent<Coins>();
            PlayerStats.LevelCoins++;
            PlayerStats.PlayerLevelXP++;
            PlayerStats.UpdatePlayerCoins();
            PlayerStats.UpdatePlayerLevel();
        }
    }

    IEnumerator SwitchWeapons()
    {
        yield return new WaitForSeconds(0.1f);

        if (_weaponIndex == 0)
        {
            _pistol.SetActive(true);
            _assaultRifle.SetActive(false);
        }
        if (_weaponIndex == 1)
        {
            _pistol.SetActive(false);
            _assaultRifle.SetActive(true);
        }
    }
}


