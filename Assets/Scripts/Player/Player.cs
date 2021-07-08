using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IDamageable
{
    [HideInInspector]
    public bool flipped;
    public float maxHealth = 20;
    float _currentHealth;
    public float immortalTime = 2;
    public bool immortal = false;

    public void TakeDamage(float value)
    {
        if (immortal)
            return;

        _currentHealth -= value;
        if(_currentHealth <= 0)
        {
            _currentHealth = 0;
            Die();
        }
        else
        {
            immortal = true;
            StartCoroutine(Immortal());
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
        print("die");
    }

    IEnumerator Immortal()
    {
        yield return new WaitForSeconds(immortalTime);
        immortal = false;

    }
}
