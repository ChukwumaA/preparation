using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UX_Scripts;

namespace Managers
{
    public class ScoreManager : MonoBehaviour
    {
        private readonly string TopScoresURL = "https://ilbeyli.byethost18.com/pacman/topscores.php";
        private string _username;
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

        private readonly List<Score> _scoreList = new List<Score>(10);

        [Obsolete("Obsolete")]
        private void OnLevelWasLoaded(int level)
        {
            StartCoroutine(nameof(ReadScoresFromDB)); //This was commented out

            switch (level)
            {
                case 2:
                    StartCoroutine(nameof(UpdateGUIText)); //If scores is loaded
                    break;
                case 1:
                    _lowestHigh = _highscore = 99999;
                    break;
            }
        }

        private IEnumerator GetHighScore()
        {
            Debug.Log("GETTING HIGHEST SCORE");
            //wait until scores are pulled from database
            var timeOut = Time.time + 4;
            while (!_scoresRead)
            {
                yield return new WaitForSeconds(0.01f);
                if (!(Time.time > timeOut)) continue;
                Debug.Log("Timed out");
                break;
            }

            _highscore = _scoreList[0].score;
            _lowestHigh = _scoreList[_scoreList.Count - 1].score;
        }

        IEnumerator UpdateGUIText()
        {
            _scoreList.Clear();
            _scoreList.Add(new Score("DATABASE TEMPORARILY UNAVAILABLE", 999999));
        
            GameObject.FindGameObjectWithTag("ScoresText").GetComponent<Scores>().UpdateGUIText(_scoreList);
            yield return new WaitForSeconds(0f);
        }

        [Obsolete("Obsolete")]
        private IEnumerator ReadScoresFromDB()
        {
            WWW getScoresAttempt = new WWW(TopScoresURL);
            yield return getScoresAttempt;

            if (getScoresAttempt.error != null)
            {
                Debug.Log($"ERROR GETTING SCORES: {getScoresAttempt.error}");
                _scoreList.Add(new Score(getScoresAttempt.error, 1234));
                StartCoroutine(UpdateGUIText());
            }
            else
            {
                //ATTENTION: assumes query will find field
                var textlist = getScoresAttempt.text.Split(new string[] { "\n", "\t" },
                    StringSplitOptions.RemoveEmptyEntries);

                if (textlist.Length == 1)
                {
                    _scoreList.Clear();
                    _scoreList.Add(new Score(textlist[0], -123));
                    yield return null;
                }
        
                else
                {
                    var names = new string[Mathf.FloorToInt(textlist.Length/2)];
                    var scores = new string[names.Length];

                    //The below code was commented out
                    Debug.Log("Textlist length: " + textlist.Length + " DATA: " + textlist[0]);
                    
                    for (var i = 0; i < textlist.Length; i++)
                    {
                        if (i%2 == 0)
                        {
                            names[Mathf.FloorToInt(i/2)] = textlist[i];
                        }
                        else scores[Mathf.FloorToInt(i/2)] = textlist[i];
                    }

                    for (var i = 0; i < names.Length; i++)
                    {
                        _scoreList.Add(new Score(names[i], scores[i]));
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
}
