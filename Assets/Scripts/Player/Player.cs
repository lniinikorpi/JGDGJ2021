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
    public bool alive;
    public GameObject sprite;
    public GameObject chain;
    public GameObject enemyHitParticle;
    public GameObject dieParticles;
    public GameObject wallHitParticle;
    public AudioSource audioSource;

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
            audioSource.Play();
            immortal = true;
            StartCoroutine(Immortal());
        }
        UIManager.instance.UpdateHealthText(_currentHealth);
    }

    // Start is called before the first frame update
    void Start()
    {
        _currentHealth = maxHealth;
        _radar = GetComponent<Radar>();
        machineGun = GetComponent<MachineGun>();
        missiles = GetComponent<Missiles>();
        playerMovement = GetComponent<PlayerMovement>();
        alive = true;
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
        alive = false;
        sprite.SetActive(false);
        chain.SetActive(false);
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        GetComponent<LineRenderer>().enabled = false;
        Instantiate(dieParticles, transform.position, Quaternion.identity);
        StartCoroutine(WaitAfterDie());
    }

    public void Respawn()
    {
        alive = true;
        sprite.SetActive(true);
        chain.SetActive(true);
        GetComponent<LineRenderer>().enabled = true;
        _currentHealth = maxHealth;
        UIManager.instance.UpdateHealthText(maxHealth);
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
        Radar[] radars = GetComponents<Radar>();
        foreach (Radar radar in radars)
        {
            radar.AddRadarTime(radar.maxRadarTime);
        }
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            if (!immortal)
            {
                TakeDamage(GameManager.instance.wallDamage);
                Instantiate(wallHitParticle, collision.contacts[0].point, Quaternion.identity); 
            }
        }
    }

    IEnumerator WaitAfterDie()
    {
        yield return new WaitForSeconds(1f);
        GameManager.instance.EndMission();
    }

    public void OnMute()
    {
        UIManager.instance.Mute();
    }

    public void OnQuit()
    {
        UIManager.instance.QuitGame();
    }
}
