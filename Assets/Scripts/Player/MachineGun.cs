using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineGun : MonoBehaviour
{
    public float fireRate = .5f;
    public float damage = 1;
    public float bulletDistance = 10;
    public Transform muzzle;
    public LineRenderer bulletTrail;

    private bool _shooting;
    private float _canShoot;

    private void Update()
    {
        if (_shooting)
        {
            if (Time.time >= _canShoot)
            {
                StartCoroutine(Shoot());
                _canShoot = Time.time + fireRate;
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
        Vector3[] positions = new Vector3[2];
        positions[0] = muzzle.transform.position;
        RaycastHit2D hit = Physics2D.Raycast(muzzle.position, Vector2.right, bulletDistance);
        if(hit.collider)
        {
            positions[1] = hit.point;
        }
        else
        {
            positions[1] = muzzle.position + new Vector3(bulletDistance, 0, 0);
        }
        bulletTrail.SetPositions(positions);
        yield return new WaitForSeconds(.05f);
        bulletTrail.SetPositions(new Vector3[2]);
    }
}
