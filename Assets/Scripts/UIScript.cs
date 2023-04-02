using System.Collections.Generic;
using Managers;
using UnityEngine;
using UnityEngine.UI;

public class UIScript : MonoBehaviour
{
    public int high, score;
    
    public List<Image> lives = new List<Image>(3);

    Text _textscore, _texthigh, _textlevel;
    private GameObject _gameObject;
    private ScoreManager _scoreManager;

    // Start is called before the first frame update
    private void Awake()
    {
        _gameObject = GameObject.Find("Game Manager");
        _scoreManager = _gameObject.GetComponent<ScoreManager>();
    }

    private void Start()
    {
        _textscore = GetComponentsInChildren<Text>()[1];
        _texthigh = GetComponentsInChildren<Text>()[0];
        _textlevel = GetComponentsInChildren<Text>()[2];

        for ( int i = 0; i < 3 - GameManager.lives; i++)
        {
            Destroy(lives[lives.Count-1]);
            lives.RemoveAt(lives.Count - 1);
        }
    }

    // Update is called once per frame
    private void Update()
    {
        high = _scoreManager.High();

        //update score text
        score = GameManager.score;
        _textscore.text = "Score\n" + score;
        _texthigh.text = "High Score\n" + high;
        _textlevel.text = "Level\n" + (GameManager.level + 1);
    }
}
