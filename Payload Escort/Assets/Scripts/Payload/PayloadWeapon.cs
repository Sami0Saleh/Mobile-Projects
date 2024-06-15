using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PayloadWeapon : MonoBehaviour
{
    [SerializeField] PayloadController _payloadController;
    [SerializeField] private Barrel _barrel;
    [SerializeField] private float _bulletRange;
    public float FireRate;
    [SerializeField] private bool _isAutomatic;
    [SerializeField] private float _bulletSpeed;
    public int BulletDamage;
    private int _ammoLeft = 1;
    private bool _canShoot = false;
    private bool _readyToShoot;
    private RaycastHit _rayHit;
    [SerializeField] List<IEnemy> _enemies = new List<IEnemy>();
    [SerializeField] LineRenderer _detectionRangeCircle;
    [SerializeField] float _detectionRange;
    private GameObject _currentEnemy;


    private void Awake()
    {
        _readyToShoot = true;
    }
    

    void Update()
    {
        UpdateDetectionRangeCircle();
        DetectEnemy();
        WeaponState();
    }
    public void WeaponState()
    {
        if (_currentEnemy != null)
        {
            transform.LookAt(_currentEnemy.transform);
            StartShot();
        }
        else
        {
            transform.rotation = _payloadController.transform.rotation;
            EndShot();
        }

        if (_canShoot && _readyToShoot)
        {
            PerformShot();
        }
    }

    public void StartShot()
    {
        _canShoot = true;
    }
    public void EndShot()
    {
        _canShoot = false;
    }
    private void PerformShot()
    {
        _readyToShoot = false;
        Vector3 direction = transform.forward;

        if (Physics.Raycast(transform.position, direction, out _rayHit, _bulletRange))
        {
            if (_rayHit.collider.gameObject.tag == "enemy")
            {
                _barrel.Shoot(direction, _bulletSpeed, BulletDamage, gameObject);
            }
        }


        if (_ammoLeft >= 0)
        {
            Invoke("ResetShot", FireRate);

            if (!_isAutomatic)
            {
                EndShot();
            }
        }
    }
    private void ResetShot()
    {
        _readyToShoot = true;
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
            Vector3 point = transform.position + new Vector3(x, -0.2f, z);
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
}
