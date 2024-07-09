using System;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour, IDamagable
{
    [SerializeField] int _maxHealth;
    [SerializeField] int _damage;
    [SerializeField] float _fireRate = 1f;

    [SerializeField] LayerMask _layerMask;
    [SerializeField] GameObject _bulletPrefab;
    [SerializeField] List<Transform> _firePoint;

    [SerializeField] Animator _animator;
    [SerializeField] AudioClip _dieClip;

    private int _health;
    private bool _canShoot = false;
    private bool _damagable = false;
    private float _nextFireTime;

    public event Action OnDie;

    private void Start()
    {
        _health = _maxHealth;
        _animator = GetComponent<Animator>();
        _canShoot = false;
        _damagable = true;
    }

    private void Update()
    {
        Shoot();
    }

    public void CanShoot()
    {
        _canShoot = true;
    }

    private void Shoot()
    {
        if (_canShoot)
        {
            if (Time.time >= _nextFireTime)
            {
                _nextFireTime = Time.time + _fireRate;
                for (int i = 0; i < _firePoint.Count; i++)
                {
                    GameObject bulletObj = Instantiate(_bulletPrefab, _firePoint[i].position, _firePoint[i].rotation);
                    var bullet = bulletObj.GetComponent<Bullet>();
                    bullet.InitBullet(4.5f, _damage, _layerMask);
                    bulletObj.SetActive(true);
                    Destroy(bulletObj, 2f);
                }
            }
        }
    }

    public void TakeDamage(int amount)
    {
        if (!_damagable) return;
        _animator.ResetTrigger("Damage");
        _animator.SetTrigger("Damage");

        _health -= amount;
        if (_health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        _canShoot = false;
        OnDie?.Invoke();
        Destroy(gameObject);
    }
}
