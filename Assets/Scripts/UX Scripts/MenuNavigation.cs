using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuNavigation : MonoBehaviour
{
    public void MainMenu()
    {
        Application.LoadLevel("menu");
    }

    public void Quit()
    {
        Application.Quit();
    }
    public void Play()
    {
        Application.LoadLevel("game");
    }
    public void HighScores()
    {
        Application.LoadLevel("scores");
    }
    public void Credits()
    {
        Application.LoadLevel("credits");
    }
    public void SourceCode()
    {
        Application.OpenURL("https://github.com/ChukwumaA/preparation");
    }
}
