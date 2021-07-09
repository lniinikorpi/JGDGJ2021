using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MissionButton : MonoBehaviour
{
    public TMP_Text difficultyText;
    public TMP_Text distanceText;
    [HideInInspector]
    public Objective objective;
    
    public void Init(Objective obj)
    {
        objective = obj;
        difficultyText.text = objective.difficulty.ToString();
        distanceText.text = "Distance: " + Vector2.Distance(GameManager.instance.baseSpawn.transform.position, objective.target.transform.position).ToString();
    }

    public void SelectMission()
    {
        GameManager.instance.selectedObjective = objective;
        UIManager.instance.missionPanel.SetActive(false);
        GameManager.instance.SelectMission();
    }
}
