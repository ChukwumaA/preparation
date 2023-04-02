using Managers;
using UnityEngine;

public class Energizer : MonoBehaviour
{
    private GameManager gm;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.Find("Game Manager"). GetComponent<GameManager>();
        if(gm == null )  
            Debug.Log("Energizer did not find Game Manager");
    }

    // Update is called once per frame
    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.name == "pacman")
        {
            gm.ScareGhost();
            Destroy(gameObject);
        }
    }
}
