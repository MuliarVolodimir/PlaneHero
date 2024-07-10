using System;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class EnemyController : MonoBehaviour, IDamagable
{
    [SerializeField] int _maxHealth;
    [SerializeField] int _damage;
    [SerializeField] float _bulletSpeed;
    [SerializeField] float _fireRate;

    [SerializeField] LayerMask _layerMask;
    [SerializeField] GameObject _particle;
    [SerializeField] GameObject _bulletPrefab;
    [SerializeField] Transform _firePoint;

    [SerializeField] Animator _animator;
    [SerializeField] AudioClip _dieClip;

    private float _fireTime;
    private int _health;
    private bool _canShoot;
    

    public event Action OnDie;

    void Start()
    {
        _animator = GetComponent<Animator>();
        _health = _maxHealth;
        _canShoot = false;
    }

    private void Update()
    {
        Shoot();
    }

    public void CanShoot()
    {
        _canShoot = !_canShoot;
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
                bullet.InitBullet(_bulletSpeed, _damage, _layerMask);
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
        AudioManager.Instance.PlayOneShotSound(_dieClip);
        GameObject particle = Instantiate(_particle, transform.position, transform.rotation);
        Destroy(particle, _dieClip.length);
        _canShoot = false;
        OnDie?.Invoke();
        Destroy(gameObject);
    }
}
