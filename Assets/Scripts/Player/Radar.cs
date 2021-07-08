using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radar : MonoBehaviour
{
    public float maxRadarTime = 10f;
    float _currentRadarTime;
    public GameObject radar;
    public GameObject radarCursor;
    private Player _player;
    private Vector2 _target;
    bool radarActive = false;
    // Start is called before the first frame update
    void Start()
    {
        _player = GetComponent<Player>();
        _currentRadarTime = maxRadarTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (_player.currentObjective != null)
        {
            RotateRadar(); 
        }

        if(radarActive)
        {
            ReduceRadarTime();
        }
    }

    private void ReduceRadarTime()
    {
        _currentRadarTime -= Time.deltaTime;
        if (_currentRadarTime <= 0)
        {
            _currentRadarTime = 0;
            radarActive = false;
            radar.SetActive(false);
        }
    }

    public void SetRadarTarget(Vector2 position)
    {
        _target = position;
    }

    void RotateRadar()
    {
        Vector2 direction =  _target - (Vector2)transform.position;
        direction = direction.normalized;

        float angle = Vector2.SignedAngle(Vector2.up, direction);
        Vector3 directionVector = new Vector3(0, 0, angle);
        radarCursor.transform.localEulerAngles = directionVector;
    }

    public void OnRadar()
    {
        radarActive = !radarActive;
        radar.SetActive(radarActive);
    }

    public void AddRadarTime(float value)
    {
        _currentRadarTime += value;
        if(_currentRadarTime > maxRadarTime)
        {
            _currentRadarTime = maxRadarTime;
        }
    }
}
