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
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    public List<Transform> treasureSpawns;
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
    [HideInInspector]
    public int money;
    public int baseUpgradeCost = 200;

    public int maxUpgradeLevel = 10;

    public float gunDamageUpgrade = 1;
    public float gunFireRateUpgrade = .2f;
    public float missileDamageUpgrade = 2;
    public float speedUpgrade = 100;
    public float healthUpgrade = 10;

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
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        player.currentObjective = selectedObjective.target;
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
            Objective objective = new Objective { target = trans.gameObject, difficulty = diff };
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
    }

    public void EndMission()
    {
        float prizeMoney = baseMissionMoney;
        if (player.hasTreasure)
        {
            prizeMoney += treasureMoney;
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
    }

    private void OnDrawGizmos()
    {
        if (treasureSpawns.Count > 0)
        {
            for (int i = 0; i < treasureSpawns.Count; i++)
            {
                if (treasureSpawns[i] != null)
                {
                    Gizmos.color = Color.red;
                    Gizmos.DrawWireSphere(treasureSpawns[i].position, .6f); 
                }
            } 
        }
    }
}
