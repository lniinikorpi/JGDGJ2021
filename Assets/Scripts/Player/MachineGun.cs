using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineGun : MonoBehaviour
{
    public float fireRate = 2f;
    public float damage = 1;
    public float bulletDistance = 10;
    public Transform muzzle;
    public LineRenderer bulletTrail;
    public LayerMask layerMask;

    private bool _shooting;
    private float _canShoot;
    private Player _player;
    public AudioSource audioSource;

    private void Start()
    {
        _player = GetComponent<Player>();
    }

    private void Update()
    {
        if (_shooting)
        {
            if (Time.time >= _canShoot)
            {
                StartCoroutine(Shoot());
                _canShoot = Time.time + (1 / fireRate);
            } 
        }
    }

    public void OnShoot()
    {
        if(_shooting)
        {
            _shooting = false;
        }
        else
        {
            _shooting = true;
        }
    }

    IEnumerator Shoot()
    {
        audioSource.Play();
        Vector3[] positions = new Vector3[2];
        positions[0] = muzzle.transform.position;
        float direction = 1;
        if(_player.flipped)
        {
            direction = -1;
        }
        RaycastHit2D hit;
        hit = Physics2D.Raycast(muzzle.position, transform.right * direction, bulletDistance, layerMask);
        if (hit.collider)
        {
            positions[1] = hit.point;
            if(hit.transform.GetComponent<IDamageable>() != null)
            {
                hit.transform.GetComponent<IDamageable>().TakeDamage(damage);
                Instantiate(GameManager.instance.player.enemyHitParticle, hit.point, Quaternion.identity);
            }
        }
        else
        {
            positions[1] = muzzle.position + new Vector3(direction * 200, 0, 0);
        }
        bulletTrail.SetPositions(positions);
        yield return new WaitForSeconds(.05f);
        bulletTrail.SetPositions(new Vector3[2]);
    }
}
