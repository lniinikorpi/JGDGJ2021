using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missiles : MonoBehaviour
{
    public Transform missileSpawn;
    public GameObject missilePrefab;
    public float damage;
    public float speed;
    public float fireRate;
    public float _canShoot;
    private Player _player;
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
            _canShoot = Time.time + fireRate;
            GameObject missile = Instantiate(missilePrefab, missileSpawn.position, Quaternion.identity);
            float direction = 1;
            if(_player.flipped)
            {
                missile.transform.localScale = new Vector3(missile.transform.localScale.x * -1, missile.transform.localScale.y, missile.transform.localScale.z);
                direction = -1;
            }
            missile.GetComponent<MissilePrefab>().Init(damage, speed, _player, direction);
        }
    }
}
