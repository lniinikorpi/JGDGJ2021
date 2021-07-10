using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Difficulty
{
    Easy, Medium, Hard
}

public class Objective
{
    public GameObject target;
    public Difficulty difficulty;
    public float multiplier;
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    public List<Transform> treasureSpawns;
    public List<Transform> enemySpawns;
    public Objective selectedObjective;
    public Player player;
    public GameObject baseSpawn;
    List<Objective> missions = new List<Objective>();
    public GameObject treasurePrefab;
    public int baseMissionMoney = 200;
    public int treasureMoney = 1000;
    public float easyMultiplier = 1f;
    public float mediumMultiplier = 1.5f;
    public float hardMultiplier = 2.0f;
    public int money = 0;
    public int baseUpgradeCost = 200;

    public int maxUpgradeLevel = 10;

    public float gunDamageUpgrade = 1;
    public float gunFireRateUpgrade = .2f;
    public float missileDamageUpgrade = 2;
    public float speedUpgrade = 100;
    public float healthUpgrade = 10;

    public float minEnemyRespawnTime = 10;
    public float maxEnemyRespawnTime = 20;

    public float wallDamage = 1;

    [HideInInspector]
    private int treasureCount = 0;
    public int treasureLimit = 5;

    public BoxCollider2D hangarCollider;
    public float missionMinTime = 30;
    private float _currentMissionTime = 0;

    [HideInInspector]
    public bool isMuted;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else if(instance != this)
        {
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        InitMissions();
        Time.timeScale = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(_currentMissionTime < missionMinTime)
        {
            if (Time.timeScale > 0)
            {
                _currentMissionTime += Time.deltaTime; 
            }
        }
        else
        {
            hangarCollider.enabled = true;
        }
    }

    public void StartMission()
    {
        player.Respawn();
        player.transform.position = baseSpawn.transform.position;
        Time.timeScale = 1;
        _currentMissionTime = 0;
        hangarCollider.enabled = false;
    }

    void InitMissions()
    {
        foreach (Transform trans in treasureSpawns)
        {
            Difficulty diff;
            float distance = Vector2.Distance(baseSpawn.transform.position, trans.position);
            if (distance < 75)
            {
                diff = Difficulty.Easy;
            }
            else if (distance >= 75 && distance < 150)
            {
                diff = Difficulty.Medium;
            }
            else
            {
                diff = Difficulty.Hard;
            }
            float multiplier = 1;
            switch (diff)
            {
                case Difficulty.Easy:
                    multiplier = easyMultiplier;
                    break;
                case Difficulty.Medium:
                    multiplier = mediumMultiplier;
                    break;
                case Difficulty.Hard:
                    multiplier = hardMultiplier;
                    break;
                default:
                    break;
            }
            Objective objective = new Objective { target = trans.gameObject, difficulty = diff, multiplier = multiplier };
            missions.Add(objective);
        }
        GetNewMissions();
    }

    public void GetNewMissions()
    {
        for (int i = 0; i < 5; i++)
        {
            UIManager.instance.SpawnMissionButton(missions[Random.Range(0, missions.Count - 1)]);
        }
    }

    public void SelectMission()
    {
        GameObject treasure = Instantiate(treasurePrefab, selectedObjective.target.transform.position, Quaternion.identity);
        player.NewObjective(treasure);
        SpawnEnemies();
        StartMission();
    }

    public void EndMission()
    {
        if(player.alive)
        {
            float prizeMoney = baseMissionMoney;
            if (player.hasTreasure)
            {
                prizeMoney += treasureMoney;
                treasureCount++;
                if(treasureCount == treasureLimit)
                {
                    EndGame();
                }
            }
            switch (selectedObjective.difficulty)
            {
                case Difficulty.Easy:
                    prizeMoney *= easyMultiplier;
                    break;
                case Difficulty.Medium:
                    prizeMoney *= mediumMultiplier;
                    break;
                case Difficulty.Hard:
                    prizeMoney *= hardMultiplier;
                    break;
                default:
                    break;
            }
            money += (int)prizeMoney;
            UIManager.instance.UpdateCalculationsText((int)prizeMoney);
            UIManager.instance.prizePanel.SetActive(true);
            UIManager.instance.UpdateMoneyText();
        }
        else
        {
            UIManager.instance.mainPanel.SetActive(true);
        }
        Destroy(player.currentObjective);
        player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        Time.timeScale = 0;
    }

    void SpawnEnemies()
    {
        for (int i = 0; i < enemySpawns.Count; i++)
        {
            enemySpawns[i].gameObject.SetActive(false);
        }

        int enemyCount = 0;
        float multiplier = 0;
        switch (selectedObjective.difficulty)
        {
            case Difficulty.Easy:
                multiplier = .4f;
                break;
            case Difficulty.Medium:
                multiplier = .6f;
                break;
            case Difficulty.Hard:
                multiplier = .8f;
                break;
            default:
                break;
        }
        enemyCount = (int)((float)enemySpawns.Count * multiplier);

        List<Transform> spawnClone = new List<Transform>(enemySpawns);

        for (int i = 0; i < enemyCount; i++)
        {
            int index = Random.Range(0, spawnClone.Count - 1);
            GameObject activeSpawn = spawnClone[index].gameObject;
            activeSpawn.SetActive(true);
            activeSpawn.GetComponent<EnemySpawn>().SpawnEnemy();
            spawnClone.RemoveAt(index);
        }
    }

    private void OnDrawGizmos()
    {
        if (treasureSpawns.Count > 0)
        {
            for (int i = 0; i < treasureSpawns.Count; i++)
            {
                if (treasureSpawns[i] != null)
                {
                    Gizmos.color = Color.green;
                    Gizmos.DrawWireSphere(treasureSpawns[i].position, .6f); 
                }
            } 
        }
        if (enemySpawns.Count > 0)
        {
            for (int i = 0; i < enemySpawns.Count; i++)
            {
                if (enemySpawns[i] != null)
                {
                    Gizmos.color = Color.red;
                    Gizmos.DrawWireSphere(enemySpawns[i].position, .6f);
                }
            }
        }
    }

    void EndGame()
    {
        UIManager.instance.gameEndPanel.SetActive(true);
    }
}
