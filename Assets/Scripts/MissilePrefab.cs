using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissilePrefab : MonoBehaviour
{
    private Rigidbody2D _rb;
    private float _damage;
    private float _speed;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, 4);
    }

    public void Init(float damage, float speed)
    {
        _damage = damage;
        _speed = speed;
    }

    private void FixedUpdate()
    {
        _rb.AddForce(Vector3.right * _speed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<IDamageable>() != null)
        {
            collision.GetComponent<IDamageable>().TakeDamage(_damage);
        }
        else if(!collision.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
