using System;
using UnityEngine;

public class PlaneController : MonoBehaviour, IDamagable
{
    [SerializeField] int _maxHealth;
    [SerializeField] int _damage;
    [SerializeField] float _speed = 5.0f;
    [SerializeField] LayerMask _layerMask;
    [SerializeField] GameObject _bulletPrefab;
    [SerializeField] Transform _firePoint;
    [SerializeField] float _fireRate = 1f;

    private int _health;
    private Vector3 _targetPosition;
    private bool _isMoving = false;
    private float _nextFireTime = 0f;

    public event Action OnDie;

    private void Start()
    {
        _health = _maxHealth;
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            _targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            _targetPosition.z = 0;
            _isMoving = true;
        }
        else
        {
            _isMoving = false;
        }

        if (_isMoving)
        {
            Shoot(); 
            transform.position = Vector3.MoveTowards(transform.position, _targetPosition, _speed * Time.deltaTime);
        }
    }

    void Shoot()
    {
        if (Time.time >= _nextFireTime)
        {
            _nextFireTime = Time.time + _fireRate;
            GameObject bulletObj = Instantiate(_bulletPrefab, _firePoint.position, _firePoint.rotation);
            var bullet = bulletObj.GetComponent<Bullet>();
            bullet.InitBullet(7.5f, _damage, _layerMask);
            bulletObj.SetActive(true);
            Destroy(bulletObj, 2f);
        }
        
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
    }
}

