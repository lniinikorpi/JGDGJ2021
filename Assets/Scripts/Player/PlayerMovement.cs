using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float accelerationForce = 10f;
    public float maxSpeed = .2f;
    public GameObject ship;
    private Vector2 _movement;
    private Rigidbody2D _rb;
    private Player _player;
    
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _player = GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (_movement != Vector2.zero)
            Move();
    }

    public void OnMove(InputValue value)
    {
        _movement = value.Get<Vector2>();
    }

    void Move()
    {
        _rb.AddForce(_movement * accelerationForce);
        float xVel = Mathf.Clamp(_rb.velocity.x, -maxSpeed, maxSpeed);
        float yVel = Mathf.Clamp(_rb.velocity.y, -maxSpeed, maxSpeed);
        _rb.velocity = new Vector2(xVel, yVel);
        if(_movement.x < 0)
        {
            if(!_player.flipped)
            {
                _player.flipped = true;
                ship.transform.localScale = new Vector3(-1, 1, 1);
            }
        }
        else if( _movement.x > 0)
        {
            if(_player.flipped)
            {
                _player.flipped = false;
                ship.transform.localScale = new Vector3(1, 1, 1);
            }
        }
    }
}
