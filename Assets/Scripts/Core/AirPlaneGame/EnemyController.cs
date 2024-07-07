using System;
using UnityEngine;

public class EnemyController : MonoBehaviour, IDamagable
{
    [SerializeField] int _maxHealth;
    [SerializeField] int _damage;
    [SerializeField] float _fireRate;

    [SerializeField] LayerMask _layerMask;
    [SerializeField] GameObject _bulletPrefab;
    [SerializeField] Transform _firePoint;
    [SerializeField] Animator _animator;

    private float _fireTime;
    private int _health;
    private bool _canShoot;
    

    public event Action OnDie;

    void Start()
    {
        _animator = GetComponent<Animator>();
        _health = _maxHealth;
        _canShoot = true;
    }

    private void Update()
    {
        Shoot();
    }

    private void Shoot()
    {
        if (_canShoot)
        {
            if (Time.time >= _fireTime)
            {
                _fireTime = Time.time + _fireRate;
                GameObject bulletObj = Instantiate(_bulletPrefab, _firePoint.position, _firePoint.rotation);
                var bullet = bulletObj.GetComponent<Bullet>();
                bullet.InitBullet(4.5f, _damage, _layerMask);
                bulletObj.SetActive(true);
                Destroy(bulletObj, 2f);
            }
        }
    }

    public void TakeDamage(int amount)
    {
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
