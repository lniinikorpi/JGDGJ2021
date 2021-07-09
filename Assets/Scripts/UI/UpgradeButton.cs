using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public enum UpgradeType
{
    GunDamage, GunFireRate, MissileDamage, Speed, Health
}

public class UpgradeButton : MonoBehaviour
{
    int price;
    public TMP_Text priceText;
    public int upgradeLevel = 0;
    GameManager gm;
    public UpgradeType upgradeType;
    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.instance;
        price = gm.baseUpgradeCost;
        UpdatePriceText();
    }

    public void BuyUpgrade()
    {
        if (gm.money - price >= 0)
        {
            gm.money -= price;
            upgradeLevel++;
            gm.player.UpgradeShip(upgradeType);
            if (upgradeLevel >= gm.maxUpgradeLevel)
            {
                GetComponent<Button>().interactable = false;
                priceText.text = "MAXED OUT";
            }
            else
            {
                price = gm.baseUpgradeCost * (upgradeLevel + 1);
                UpdatePriceText(); 
            }
            UIManager.instance.UpdateMoneyText();
        }
    }

    void UpdatePriceText()
    {
        priceText.text = price.ToString() + "$";
    }
}
