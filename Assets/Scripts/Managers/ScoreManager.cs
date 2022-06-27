using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private string TopScoresURL = "https://ilbeyli.byethost18.com/pacman/topscores.php";
    private string username;
    private int _highscore;
    private int _lowestHigh;
    private bool _scoresRead;
    private bool _isTableFound;

    public class Score
    {
        public string name { get; set; }
        public int score { get; set; }

        public Score(string n, int s)
        {
            name = n;
            score = s;
        }

        public Score(string n, string s)
        {
            name = n;
            score = Int32.Parse(s);
        }
    }

    List<Score> scoreList = new List<Score>(10);

    void OnLevelWasLoaded(int level)
    {
        StartCoroutine("ReadScoresFromDB"); //This was commented out

        if (level == 2)
            StartCoroutine("UpdateGUIText"); //If scores is loaded
        if (level == 1)
            _lowestHigh = _highscore = 99999;
        //the code below was comented out
        if (level == 1) StartCoroutine("GetHigherScore"); //If game is loaded
    }

    IEnumerator GetHighScore()
    {
        Debug.Log("GETTING HIGHEST SCORE");
        //wait until scores are pulled from database
        float timeOut = timeOut.time + 4;
        while (!_scoresRead)
        {
            yield return new WaitForSeconds(0.01f);
            if (Time.time > timeOut)
            {
                Debug.Log("Timed out");
                scoreList.Clear(); //this was commented out
                scoreList.Add(new Score("GetHighestScore:: DATABASE CONNNECTION TIMED OUT", -1)); //this was commented out
                break;
            }
        }

        _highscore = scoreList[0].score;
        _lowestHigh = scoreList[scoreList.Count - 1].score;
    }

    IEnumerator UpdateGUIText()
    {
        scoreList.Clear();
        scoreList.Add(new Score("DATABASE TEMPORARILY UNAVAILABLE", 999999));
        
        GameObject.FindGameObjectWithTag("ScoresText").GetComponent<Scores>().UpdateGUIText(scoreList);
        yield return new WaitForSeconds(0f);
    }

    IEnumerator ReadScoresFromDB()
    {
        WWW GetScoresAttempt = new WWW(TopScoresURL);
        yield return GetScoresAttempt;

        if (GetScoresAttempt.error != null)
        {
            Debug.Log(string.Format("ERROR GETTING SCORES: {0}", GetScoresAttempt,error));
            scoreList.Add(new Score(GetScoresAttempt,error, 1234));
            StartCoroutine(UpdateGUIText());
        }
        else
        {
            //ATTENTION: assumes query will find field

            string[] textlist = GetScoresAttempt.text.Split(new string[] { "\n", "\t" },
                StringSplitOptions.RemoveEmptyEntries);

            if (textlist.Length == 1)
            {
                //The below code was commented out
                Debug.Log("== 1");

                scoreList.Clear();
                scoreList.Add(new Score(textlist[0], -123));
                yield return null;
            }
        
            else
            {
                string[] Names = new string[Mathf.FloorToInt(textlist.Length/2)];
                string[] Scores = new string[Names.Length];

                //The below code was commented out
                Debug.Log("Textlist length: " + textlist.Length + " DATA: " + textlist[0]);


                for (int i = 0; i < textlist.Length; i++)
                {
                    if (i%2 == 0)
                    {
                        Names[Mathf.FloorToInt(i/2)] = textlist[i];
                    }
                    else Scores[Mathf.FloorToInt(i/2)] = textlist[i];
                }

                for (int i = 0; i < Names.Length; i++)
                {
                    scoreList.Add(new Score(Names[i], Scores[i]));
                }
                    _scoresRead = true;
            }
        }

    }

    public int High()
    {
        return _highscore;
    }

    public int LowestHigh()
    {
        return _lowestHigh;
    }

}
