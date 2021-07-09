using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadarPowerUp : MonoBehaviour
{
    public float minRadarTime = 2;
    public float maxRadarTime = 5;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            collision.GetComponentInParent<Radar>().AddRadarTime(Random.Range(minRadarTime, maxRadarTime));
            Destroy(gameObject);
        }
    }
}
