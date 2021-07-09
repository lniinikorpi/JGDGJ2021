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
    public GameObject currentObjective;
    private Radar _radar;
    [HideInInspector]
    public bool hasTreasure = false;
    public MachineGun machineGun;
    public Missiles missiles;
    public PlayerMovement playerMovement;

    public void TakeDamage(float value)
    {
        if (immortal)
            return;

        _currentHealth -= value;
        UIManager.instance.UpdateHealthText(_currentHealth);
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
        _radar = GetComponent<Radar>();
        machineGun = GetComponent<MachineGun>();
        missiles = GetComponent<Missiles>();
        playerMovement = GetComponent<PlayerMovement>();
        if (currentObjective != null)
        {
            NewObjective(currentObjective); 
        }
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

    public void NewObjective(GameObject objective)
    {
        currentObjective = objective;
        _radar.SetRadarTarget(objective);
    }

    public void UpgradeShip(UpgradeType type)
    {
        GameManager gm = GameManager.instance;
        switch (type)
        {
            case UpgradeType.GunDamage:
                machineGun.damage += gm.gunDamageUpgrade;
                break;
            case UpgradeType.GunFireRate:
                machineGun.fireRate += gm.gunFireRateUpgrade;
                break;
            case UpgradeType.MissileDamage:
                missiles.damage += gm.missileDamageUpgrade;
                break;
            case UpgradeType.Speed:
                playerMovement.accelerationForce += gm.speedUpgrade;
                playerMovement.maxSpeed += gm.speedUpgrade / 100;
                break;
            case UpgradeType.Health:
                maxHealth += gm.healthUpgrade;
                _currentHealth = maxHealth;
                UIManager.instance.UpdateHealthText(_currentHealth);
                break;
            default:
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Hangar"))
        {
            GameManager.instance.EndMission();
        }
    }
}
