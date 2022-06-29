using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacDot : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D co) 
    {
        if (co.name == "pacman")
        {
            GameManager.score += 10;
            GameObject[] pacdots = GameObject.FindGameObjectWithTag("pacdot");
            Destroy(gameObject);

            if (pacdots.Length == 1)
            {
                GameObject.FindObjectOfType<GameGUINavigation>().LoadLevel();
            }
        }
    }
}
