using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissilePrefab : MonoBehaviour
{
    public Rigidbody2D rb;
    private float _damage;
    private float _speed;
    private Transform _followTransform;
    private Player _player;
    float _direction = 1;
    bool followY = true;
    public GameObject particles;

    private void Start()
    {
        Destroy(gameObject, 4);
    }

    public void Init(float damage, float speed, Player player, float direction, Transform followTransform, float xVelocity)
    {
        _damage = damage;
        _speed = speed;
        _player = player;
        _direction = direction;
        _followTransform = followTransform;
        rb.velocity = new Vector2(xVelocity, rb.velocity.y);
    }

    private void FixedUpdate()
    {
        rb.AddForce(transform.right * _speed * _direction);
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
        if(collision.GetComponentInParent<IDamageable>() != null)
        {
            if (!collision.CompareTag("Player") && !collision.CompareTag("MissileSilo"))
            {
                collision.gameObject.GetComponentInParent<IDamageable>().TakeDamage(_damage);
                Instantiate(GameManager.instance.player.enemyHitParticle, collision.gameObject.GetComponent<Collider2D>().ClosestPoint(transform.position), Quaternion.identity);
            }
        }
        if (!collision.CompareTag("Player") && !collision.CompareTag("MissileSilo"))
        {
            Instantiate(particles, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("MissileSilo"))
        {
            rb.gravityScale = 0.02f;
            followY = false;
            rb.velocity = new Vector2(rb.velocity.x, _player.gameObject.GetComponent<Rigidbody2D>().velocity.y);
        }
    }
}
