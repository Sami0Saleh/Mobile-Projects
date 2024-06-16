using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private int _damage;
    [SerializeField] private AudioSource _bulletSound;
    private GameObject _shooter;

    private void Start()
    {
        _bulletSound.Play();
    }

    public void Initialize(float speed, int damage, GameObject shooter)
    {
        _speed = speed;
        _damage = damage;
        _shooter = shooter;
    }

    void Update()
    {
        transform.Translate(Vector3.forward * _speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_shooter != null)
        {
            if (other.gameObject != _shooter)
            {
                if (((_shooter.CompareTag("Player") || _shooter.CompareTag("payload")) && other.CompareTag("enemy")) ||
                    (_shooter.CompareTag("enemy") && (other.CompareTag("Player") || other.CompareTag("payload"))))
                {
                    var damageable = other.GetComponent<IDamageable>();
                    if (damageable != null)
                    {
                        damageable.TakeRangedDamage(_damage);
                    }
                    Destroy(gameObject);
                }
                else if (other.tag != null)
                {
                    Destroy(gameObject);
                }
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
