using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treasure : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("TreasurePlatform"))
        {
            GameManager.instance.player.hasTreasure = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("TreasurePlatform"))
        {
            GameManager.instance.player.hasTreasure = false;
        }
    }
}
