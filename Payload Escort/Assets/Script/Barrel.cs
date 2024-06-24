using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrel : MonoBehaviour
{
    [SerializeField] private GameObject _bulletPrefab;

    public void Shoot(Vector3 direction, float speed, int damage, GameObject shooter)
    {
        GameObject bullet = Instantiate(_bulletPrefab, transform.position, Quaternion.LookRotation(direction));
        Bullet bulletComponent = bullet.GetComponent<Bullet>();
        bulletComponent.Initialize(speed, damage, shooter);
    }
}
