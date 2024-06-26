using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{

    [SerializeField] PlayerController _playerController;
    [SerializeField] private Barrel _barrel;
    [SerializeField] private float _bulletRange;
    public float FireRate;
    [SerializeField] private bool _isAutomatic;
    [SerializeField] private float _bulletSpeed;
    public int BulletDamage;
    private int _ammoLeft = 1;

    private bool _canShoot = false;
    public bool ReadyToShoot;


    private RaycastHit _rayHit;

    private void Awake()
    {
        ReadyToShoot = true;
    }

    void Update()
    {
        if (_canShoot && ReadyToShoot)
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
        ReadyToShoot = false;
        Vector3 direction = _playerController.transform.forward;

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
        ReadyToShoot = true;
    }
}
