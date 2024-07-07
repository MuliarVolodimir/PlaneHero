using Codice.Client.Common.FsNodeReaders;
using Codice.CM.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Bullet : MonoBehaviour
{
    [SerializeField] LayerMask _layerMask;
    [SerializeField] bool moveUp = true;

    private float _speed;
    private int _damage;

    public void InitBullet(float speed, int damage)
    {
        _speed = speed;
        _damage = damage;
    }

    void Update()
    {
        CheckCollisison();
    }

    private void CheckCollisison()
    {
        Vector3 dir = Vector3.forward;
        if (moveUp)
        {
            transform.Translate(Vector3.up * _speed * Time.deltaTime);
            dir = Vector3.up;
        }
        else
        {
            transform.Translate(Vector3.down * _speed * Time.deltaTime);
            dir = Vector3.down;
        }

        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, _speed * Time.deltaTime, _layerMask);
        if (hit.collider != null)
        { 
            hit.collider.GetComponent<EnemyController>().TakeDamage(_damage);
            Destroy(gameObject);
            return;
        }

        Debug.DrawRay(transform.position, dir, Color.red);
    }
}
