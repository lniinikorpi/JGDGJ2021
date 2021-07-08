using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissilePrefab : MonoBehaviour
{
    private Rigidbody2D _rb;
    private float _damage;
    private float _speed;
    private Transform _followTransform;
    private Player _player;
    float _direction = 1;
    bool followY = true;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, 4);
    }

    public void Init(float damage, float speed, Player player, float direction, Transform followTransform)
    {
        _damage = damage;
        _speed = speed;
        _player = player;
        _direction = direction;
        _followTransform = followTransform;
    }

    private void FixedUpdate()
    {
        _rb.AddForce(transform.right * _speed * _direction);
    }

    private void Update()
    {
        if(followY)
        {
            transform.position = new Vector3(transform.position.x, _followTransform.position.y);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<IDamageable>() != null)
        {
            collision.GetComponent<IDamageable>().TakeDamage(_damage);
        }
        if (!collision.CompareTag("Player") && !collision.CompareTag("MissileSilo"))
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("MissileSilo"))
        {
            _rb.gravityScale = 0.02f;
            followY = false;
            _rb.velocity = new Vector2(_rb.velocity.x, _player.gameObject.GetComponent<Rigidbody2D>().velocity.y);
        }
    }
}
