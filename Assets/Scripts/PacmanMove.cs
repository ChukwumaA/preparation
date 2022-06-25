using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacmanMove : MonoBehaviour
{
    public float speed = 0.4f;
    Vector2 dest = Vector2.zero;
    Vector2 _dir = Vector2.zero;
    Vector2 _nextDir = Vector2.zero;

    [Serializable]
    public class PointSprites
    {
        public GameObject[] pointSprites;
        
    }

    public PointSprites points;

    public static int killstreak = 0;

    //script from Manager folder
    private GameGUINavigation GUINav;
    private GameManager GM;
    private ScoreManager SM;

    private bool _deadPlaying = false;

    // Start is called before the first frame update
    void Start()
    {
        GM = GameObject.Find("Game Manager").GetComponent<GameManager>();
        SM = GameObject.Find("Game Manager").GetComponent<ScoreManager>();
        GUINav = GameObject.Find("UI Manager").GetComponent<GameGUINavigation>();
        dest = transform.position;
    }

    // Update is called once per frame in a fixed time interval
    void FixedUpdate()
    {

        switch (GameManager.gameState)
        {
            case GameManager.GameState.Game:
                ReadInputAndMove();
                Animate();
                break;

            case GameManager.GameState.Dead:
                if (!_deadPlaying)
                    StartCoroutine("PlayDeadAnimation");
                break;
        }
    }

    IEnumerator PlayDeadAnimation()
    {
        _deadPlaying = true;
        GetComponent<Animator>().SetBool("Die", true);
        yield return new WaitForSeconds(1);
        GetComponent<Animator>().SetBool("Die", false);
        _deadPlaying = false;

        if (GameManager.lives <= 0)
        {
            Debug.Log("Threshold for the High Score: " + SM.LowestHigh());
            if (GameManager.score >= SM.LowestHigh())
                GUINav.getScoresMenu();
            else
                GUINav.H_ShowGameOverScreen();
        }

        else 
            GM.ResetScene();
    }

    void Animate()
    {
    //Animation Parameters
        Vector2 dire = dest - (Vector2)transform.position;
        GetComponent<Animator>().SetFloat("DirX", dire.x);
        GetComponent<Animator>().SetFloat("DirY", dire.y);
    }

    bool valid(Vector2 dir) 
    {
        //Cast Line from 'next to Pac-Man' to 'Pac-Man'
        Vector2 pos = transform.position;
        direction += new Vector2(direction.x * 0.45f, direction.y * 0.45f);
        RaycastHit2D hit = Physics2D.Linecast(pos + dir, pos);
        return hit.collider == "pacdot" || (hit.collider == GetComponent<Collider2D>());
    }

    public void ResetDestination()
    {
        dest = new Vector2(15f, 11f);
        GetComponent<Animator>
    }

        // Move closer to Destination
    //     Vector2 p = Vector2.MoveTowards(transform.position, dest, speed);
    //     GetComponent<Rigidbody2D>().MovePosition(p);

    //       // Check for Input if not moving
    //     if ((Vector2)transform.position == dest) 
    //     // Check for Input if not moving
    //     if ((Vector2)transform.position == dest) 
    //     {
    //     if (Input.GetKey(KeyCode.UpArrow) && valid(Vector2.up))
    //         dest = (Vector2)transform.position + Vector2.up;

    //     if (Input.GetKey(KeyCode.RightArrow) && valid(Vector2.right))
    //         dest = (Vector2)transform.position + Vector2.right;

    //     if (Input.GetKey(KeyCode.DownArrow) && valid(-Vector2.up))
    //         dest = (Vector2)transform.position - Vector2.up;

    //     if (Input.GetKey(KeyCode.LeftArrow) && valid(-Vector2.right))
    //         dest = (Vector2)transform.position - Vector2.right;
    //     }

    //     //Animation Parameters
    //     Vector2 dire = dest - (Vector2)transform.position;
    //     GetComponent<Animator>().SetFloat("DirX", dire.x);
    //     GetComponent<Animator>().SetFloat("DirY", dire.y);
    // }

    
}
