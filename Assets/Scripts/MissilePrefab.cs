using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissilePrefab : MonoBehaviour
{
    private Rigidbody2D _rb;
    private float _damage;
    private float _speed;
    private Player _player;
    float _direction = 1;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, 4);
    }

    public void Init(float damage, float speed, Player player, float direction)
    {
        _damage = damage;
        _speed = speed;
        _player = player;
        _direction = direction;
    }

    private void FixedUpdate()
    {
        _rb.AddForce(transform.right * _speed * _direction);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<IDamageable>() != null)
        {
            collision.GetComponent<IDamageable>().TakeDamage(_damage);
        }
        if(!collision.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
