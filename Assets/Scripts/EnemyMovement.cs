using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public int damage = 1;
    float movementSpeed;
    public bool backAndForth = false;
    public bool waitBetweenTargets = false;
    public float waitTime = 0;
    [Tooltip("Time after enemy goes to patrol")]
    public float waitToPatrol = 2;
    public float chaceSpeedMultiplier = 1.5f;
    public float waitTimeAfterAttack = 1;
    public float visionRange = 4;
    private bool chasing = false;
    bool moving = false;
    private bool waiting = false;
    private bool waitStarted;
    private bool _outOfRange;
    private float _patrolWaitCounter;
    private int _targetIndex = 0;
    private int direction = 1;
    public List<Transform> _patrolTargets = new List<Transform>();
    private List<Vector2> _patrolPositions = new List<Vector2>();
    EnemyBase _enemyBase;
    float spriteStartXScale;
    GameObject _player;
    EnemyShark enemyShark;

    void Start()
    {
        for (int i = 0; i < _patrolTargets.Count; i++)
        {
            _patrolPositions.Add(_patrolTargets[i].position);
        }
        _enemyBase = GetComponent<EnemyBase>();
        movementSpeed = _enemyBase.movementSpeed;
        spriteStartXScale = _enemyBase.sprite.transform.localScale.x;
        _player = GameObject.FindGameObjectWithTag("Player");
        enemyShark = GetComponent<EnemyShark>();
    }

    void Update()
    {
        if (!waiting)
        {
            if (!moving && !chasing)
            {
                StartCoroutine(MovePosition());
            }

            if (CheckIfChaseDistance())
            {
                chasing = true;
                _outOfRange = false;
                _patrolWaitCounter = 0;
            }
            else
            {
                _outOfRange = true;
            }

            if (chasing)
            {
                Chase(_player.transform.position);
                if(_outOfRange)
                {
                    _patrolWaitCounter += Time.deltaTime;
                    if(_patrolWaitCounter >= waitToPatrol)
                    {
                        chasing = false;
                    }
                }
            } 
        }
        else
        {
            if(!waitStarted)
            {
                StartCoroutine(Wait());
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, visionRange);
        foreach (Transform t in _patrolTargets)
        {
            Gizmos.DrawWireSphere(t.position, .5f);
        }
    }

    IEnumerator MovePosition()
    {
        moving = true;
        if(transform.position.x > _patrolPositions[_targetIndex].x)
        {
            _enemyBase.sprite.transform.localScale = new Vector3(-spriteStartXScale, _enemyBase.sprite.transform.localScale.y, _enemyBase.sprite.transform.localScale.z);
        }
        else
        {
            _enemyBase.sprite.transform.localScale = new Vector3(spriteStartXScale, _enemyBase.sprite.transform.localScale.y, _enemyBase.sprite.transform.localScale.z);
        }
        while (transform.position != (Vector3)_patrolPositions[_targetIndex] && !chasing)
        {
            float step = movementSpeed * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, _patrolPositions[_targetIndex], step);
            yield return new WaitForSeconds(.001f);
        }

        if (!chasing)
        {
            if (waitBetweenTargets)
                yield return new WaitForSeconds(waitTime); 
        }

        if (backAndForth)
        {
            if (_targetIndex + direction >= _patrolPositions.Count || _targetIndex + direction < 0)
            {
                direction *= -1;
            }
            _targetIndex += direction;
        }
        else
        {
            if (_targetIndex + 1 < _patrolPositions.Count)
            {
                _targetIndex++;
            }
            else
            {
                _targetIndex = 0;
            }
        }
        moving = false;
    }

    void Chase(Vector3 target)
    {
        if (chasing)
        {
            transform.position = Vector2.MoveTowards(transform.position, target, movementSpeed * Time.deltaTime * chaceSpeedMultiplier); 
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            if(chasing)
            {
                waiting = true;
                chasing = false;
            }
            _player.GetComponentInParent<IDamageable>().TakeDamage(enemyShark.damage);
        }
    }

    bool CheckIfChaseDistance()
    {
        if(Vector2.Distance(transform.position, _player.transform.position) <= visionRange)
        {
            if((_player.transform.position.x >= transform.position.x && _enemyBase.sprite.transform.localScale.x > 0) || (_player.transform.position.x <= transform.position.x && _enemyBase.sprite.transform.localScale.x < 0))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    IEnumerator Wait()
    {
        waitStarted = true;
        yield return new WaitForSeconds(waitTimeAfterAttack);
        waitStarted = false;
        waiting = false;
    }

    private void OnDisable()
    {
        moving = false;
    }

}
