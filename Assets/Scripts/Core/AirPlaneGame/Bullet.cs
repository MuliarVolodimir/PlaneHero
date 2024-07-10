using UnityEngine;

public class Bullet : MonoBehaviour, IDamagable
{
    private LayerMask _layerMask;
    private float _speed;
    private int _damage;

    public void InitBullet(float speed, int damage,  LayerMask layer)
    {
        _speed = speed;
        _damage = damage;
        _layerMask = layer; 
    }

    void Update()
    {
        CheckCollisison();
    }

    private void CheckCollisison()
    {
        Vector3 dir = Vector3.up;
        transform.Translate(dir * _speed * Time.deltaTime);

        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, _speed * Time.deltaTime, _layerMask);
        if (hit.collider != null)
        { 
            hit.collider.GetComponent<IDamagable>().TakeDamage(_damage);
            Destroy(gameObject);
            return;
        }
    }

    public void TakeDamage(int amount)
    {
        Destroy(gameObject);
    }
}
