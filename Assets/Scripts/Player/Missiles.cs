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
    // Start is called before the first frame update
    void Start()
    {
        
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
            missile.GetComponent<MissilePrefab>().Init(damage, speed);
        }
    }
}
