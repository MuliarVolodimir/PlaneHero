using System;
using UnityEngine;

public class PlaneController : MonoBehaviour, IDamagable
{
    [SerializeField] int _maxHealth;
    [SerializeField] int _damage;
    [SerializeField] float _speed = 5.0f;
    [SerializeField] float _fireRate = 1f;

    [SerializeField] SpriteRenderer _spriteRenderer;
    [SerializeField] GameObject _particle;
    [SerializeField] LayerMask _layerMask;
    [SerializeField] GameObject _bulletPrefab;
    [SerializeField] Transform _firePoint;

    [SerializeField] AudioClip _shootClip;
    [SerializeField] AudioClip _dieClip;

    private int _health;
    private Vector3 _targetPosition;
    private bool _isMoving = false;
    private float _nextFireTime = 0f;
    private ApplicationData _appData;

    public event Action OnDie;

    private void Start()
    {
        _health = _maxHealth;

        _appData = ApplicationData.Instance;
        var planeIndex = _appData.GetPlane();
        var sprite = _appData.GetPlanes().Find(plane => plane.Name == planeIndex).Sprite;
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _spriteRenderer.sprite = sprite;
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
            AudioManager.Instance.PlayOneShotSound(_shootClip);
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
        AudioManager.Instance.PlayOneShotSound(_dieClip);
        GameObject particle = Instantiate(_particle, transform.position, transform.rotation);
        Destroy(particle, _dieClip.length);
        OnDie?.Invoke();
    }
}

