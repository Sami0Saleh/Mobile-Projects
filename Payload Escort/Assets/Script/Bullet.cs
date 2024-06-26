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
    [SerializeField] private MeshRenderer _bulletMesh;
    [SerializeField] Material _playerBulletMaterial;
    [SerializeField] Material _enemyBulletMaterial;
    [SerializeField] Light _bulletLight;
    public GameObject Shooter;

    private void Start()
    {
        if(Shooter.CompareTag("enemy"))
        {
            _bulletMesh.material = _enemyBulletMaterial;
            _bulletLight.color = Color.red;
        }
        else
        {
            _bulletMesh.material = _playerBulletMaterial;
            _bulletLight.color = Color.green;
        }
        _bulletSound.Play();
        StartCoroutine(BulletDestroy());
    }

    public void Initialize(float speed, int damage, GameObject shooter)
    {
        _speed = speed;
        _damage = damage;
        Shooter = shooter;
    }

    void Update()
    {
        transform.Translate(Vector3.forward * _speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (Shooter != null)
        {
            if (other.gameObject != Shooter)
            {
                if (((Shooter.CompareTag("Player") || Shooter.CompareTag("payload")) && other.CompareTag("enemy")) ||
                    (Shooter.CompareTag("enemy") && (other.CompareTag("Player") || other.CompareTag("payload"))))
                {
                    var damageable = other.GetComponent<IDamageable>();
                    if (damageable != null)
                    {
                        if (Shooter.CompareTag("Player"))
                        {
                            if (PlayerStats.IsInstantKill)
                            {
                                int ran = Random.Range(1, 4);
                                if (ran == 1)
                                {
                                    Destroy(other.gameObject);
                                }
                                else
                                {
                                    damageable.TakeRangedDamage(_damage);
                                }
                            }
                            else
                            {
                                damageable.TakeRangedDamage(_damage);
                            }
                        }
                        else
                        {
                            damageable.TakeRangedDamage(_damage);
                        }
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
    private IEnumerator BulletDestroy()
    {
        yield return new WaitForSecondsRealtime(1);
        Destroy(gameObject);
    }
}
