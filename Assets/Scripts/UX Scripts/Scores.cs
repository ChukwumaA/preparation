using System.Collections.Generic;
using Managers;
using UnityEngine;
using UnityEngine.UI;

namespace UX_Scripts
{
    public class Scores : MonoBehaviour
    {
        public ScoreManager scoreManager;

        private Text _scoresText;

        public void UpdateGUIText(List<ScoreManager.Score> scoreList)
        {
            _scoresText = GetComponent<Text>();
            Debug.Log("Updating GUIText: Score-list count=" + scoreList.Count);
            var currentScore = "";
            foreach (var scores in scoreList)
            {
                if (scores.score < 1000)
                    currentScore += scores.score + "\t\t\t" + scores.name + "\n";
                else    
                    currentScore += scores.score + "\t\t" + scores.name + "\n";
            }

            _scoresText.text = currentScore;
        }
    }
}
