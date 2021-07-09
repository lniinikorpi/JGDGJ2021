using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour, IDamageable
{
    public float maxHealth = 10;
    public float movementSpeed = 10;
    public float damage = 3;
    public GameObject sprite;
    public GameObject powerUpToDrop;
    public EnemySpawn spawn;

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
    
    public void InitEnemy()
    {
        _currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Die()
    {
        if(powerUpToDrop != null)
        {
            Instantiate(powerUpToDrop, transform.position, Quaternion.identity);
            spawn.enemyAlive = false;
        }
        gameObject.SetActive(false);
    }
}
