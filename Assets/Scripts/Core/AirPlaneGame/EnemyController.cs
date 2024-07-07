using System;
using UnityEngine;

public class EnemyController : MonoBehaviour, IDamagable
{
    [SerializeField] int _maxHealth;
    [SerializeField] float _fireRate;

    private float _fireTime;
    private int _health;
    private bool _canShoot;

    public event Action OnDie;

    void Start()
    {
        _health = _maxHealth;
        
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
            }
        }
    }

    public void CanShoot(bool canShoot)
    {
        _canShoot = canShoot;
    }

    public void TakeDamage(int amount)
    {
        _health -= amount;
        if (_health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        OnDie?.Invoke();
        Destroy(gameObject);
    }
}
