using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour, IDamageable
{
    public float maxHealth = 10;
    public float movementSpeed = 10;
    public GameObject sprite;

    private float _currentHealth;

    public void TakeDamage(float value)
    {
        _currentHealth -= value;
        if(_currentHealth <= 0)
        {
            _currentHealth = 0;
            Die();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
