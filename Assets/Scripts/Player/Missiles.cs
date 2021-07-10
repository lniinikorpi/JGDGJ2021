using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missiles : MonoBehaviour
{
    public Transform missileSpawn;
    public GameObject missilePrefab;
    public float damage = 5;
    public float speed = 10;
    public float fireRate = .25f;
    public float _canShoot;
    private Player _player;
    public AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        _player = GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnMissile()
    {
        if(Time.time >= _canShoot)
        {
            audioSource.Play();            _canShoot = Time.time + (1 / fireRate);
            GameObject missile = Instantiate(missilePrefab, missileSpawn.position, Quaternion.identity);
            float direction = 1;
            if(_player.flipped)
            {
                missile.transform.localScale = new Vector3(missile.transform.localScale.x * -1, missile.transform.localScale.y, missile.transform.localScale.z);
                direction = -1;
            }
            missile.GetComponent<MissilePrefab>().Init(damage, speed, _player, direction, transform, gameObject.GetComponent<Rigidbody2D>().velocity.x);
        }
    }
}
