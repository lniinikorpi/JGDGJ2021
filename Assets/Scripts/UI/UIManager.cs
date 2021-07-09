using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance = null;
    public TMP_Text totalMoneyText;
    public TMP_Text healthText;
    public Scrollbar radarScroll;
    public GameObject missionButton;
    public Transform missionButtonGrid;
    public GameObject missionPanel;
    public GameObject mainPanel;
    public GameObject prizePanel;
    public TMP_Text calculationText;
    GameManager gm;

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
        UpdateMoneyText();
        UpdateHealthText(GameManager.instance.player.maxHealth);
        missionPanel.SetActive(false);
        mainPanel.SetActive(true);
        prizePanel.SetActive(false);
        gm = GameManager.instance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnMissionButton(Objective objective)
    {
        GameObject obj = Instantiate(missionButton, missionButtonGrid);
        MissionButton mb = obj.GetComponent<MissionButton>();
        mb.Init(objective);
    }

    public void UpdateHealthText(float value)
    {
        healthText.text = value.ToString() + "/" + GameManager.instance.player.maxHealth.ToString();
    }

    public void UpdateMoneyText()
    {
        totalMoneyText.text = GameManager.instance.money.ToString();
    }

    public void UpdateRadarScroll(int value)
    {
        radarScroll.value = value;
    }

    public void UpdateCalculationsText(int totalMoney)
    {
        if(gm.player.hasTreasure)
        {
            calculationText.text = "Mission: " + gm.baseMissionMoney + "$" + "\n" + "Treasure: " + gm.treasureMoney + "$" + "\n" + "Multiplier: " + gm.selectedObjective.multiplier + "\n" + "Total: " + totalMoney + "$";
        }
        else
        {
            calculationText.text = "Mission: " + gm.baseMissionMoney + "$" + "\n" + "Multiplier: " + gm.selectedObjective.multiplier + "\n" + "Total: " + totalMoney + "$";
        }
    }
}
