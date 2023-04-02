using System;
using UnityEngine;

namespace UX_Scripts
{
    public class MenuNavigation : MonoBehaviour
    {
        [Obsolete("Obsolete")]
        public void MainMenu()
        {
            Application.LoadLevel("menu");
        }

        public void Quit()
        {
            Application.Quit();
        }
        [Obsolete("Obsolete")]
        public void Play()
        {
            Application.LoadLevel("game");
        }
        [Obsolete("Obsolete")]
        public void HighScores()
        {
            Application.LoadLevel("scores");
        }
        [Obsolete("Obsolete")]
        public void Credits()
        {
            Application.LoadLevel("credits");
        }
        public void SourceCode()
        {
            Application.OpenURL("https://github.com/ChukwumaA/preparation");
        }
    }
}
