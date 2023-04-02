using Managers;
using UnityEngine;
using UX_Scripts;

public class PacDot : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.name == "pacman")
        {
            GameManager.score += 10;
            GameObject[] pacdots = GameObject.FindGameObjectsWithTag("pacdot");
            Destroy(gameObject);

            if (pacdots.Length == 1)
            {
                GameObject.FindObjectOfType<GameGUINavigation>().LoadLevel();
            }
        }
    }
}
