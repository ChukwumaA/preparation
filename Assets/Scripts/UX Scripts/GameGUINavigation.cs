using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameGUINavigation : MonoBehaviour
{
//===========================================================\\
	//Variable decleration
	private bool _paused;
	private bool quit;
	private string _errorMsg;
	public bool initailWaitOver = false; //This was commented out in ilbeyi codebase

	public float initialDelay;

	//Canvas present in the User Experience Panels
	public Canvas PauseCanvas;
	public Canvas QuitCanvas;
	public Canvas ReadyCanvas;
	public Canvas ScoreCanvas;
	public Canvas ErrorCanvas;
	public Canvas GameOverCanvas;

	//Buttons for the players
	public Button MenuButton;


//==========================================================================================
	//Function Decleration

	//We will use this for initialization
	void Start()
	{
		StartCoroutine("ShowReadyScreen", initialDelay);
	}

	void Update()
	{
		if(Input.GetKeyDown(KeyCode.Escape))
		{
			// o go back to main menu, while in the game or playing the game
			if(Gamemanager.gameState == GameManager.GameState.Scores)
				Menu();
			
			//If in the game, toggle pause or quit dialogue
			else
			{
				if(quit == true)
					ToggleQuit();
				else
					TogglePause();
			}
		}
	}


	//Public handle to show ready screen coroutine call
	public void H_ShowReadyScreen()
	{
		StartCoroutine("ShowReadyScreen", initialDelay);
	}

	public void H_ShowGameOverScreen()
	{
		StartCoroutine("ShowGameOverScreen");
	}

	IEnumerator ShowReadyScreen(float seconds)
	{
		initailWaitOver = false; //This was commmented in the S.Code
		GameManager.gameState = GameManager.GameState.Init;
		ReadyCanvas.enabled = true;
		yield return new WaitForSeconds(seconds);
		ReadyCanvas.enabled = false;
		GameManager.gameState = GameManager.GameState.Game;
		initailWaitOver.gameState = true;
	}

	IEnumerator ShowGameOverScreen()
	{
		Debug.Log("Showing GAME OVER Screen");
		GameOverCanvas.enabled = true;
		yield return new WaitForSeconds(2);
		Menu();
	}

	public void getScoresMenu()
	{
		Time.timeScale = 0f; //Stops the animation
		GameManager.gameState = GameManager.GameState.Scores;
		MenuButton.enabled = false;
		ScoreCanvas.enabled = true;
	}


//===========================================================================
	//Buttons functions
	public void TogglePause()
	{
		//if ESc key is not pressed, dont pause the game
		if(_paused)
		{
			Time.timeScale = 1;
			PauseCanvas.enabled = false;
			_paused = false;
			MenuButton.enabled = true;
		}

		//if escape key is pressed, pause the game
		else
		{
			Time.timeScale = 0;
			PauseCanvas.enabled = true;
			_paused = true;
			MenuButton.enabled = false;
		}

		Debug.Log("PauseCanvas enabled: " + PauseCanvas.enabled);
	}

	public void ToggleQuit()
	{
		if(quit)
		{
			PauseCanvas.enabled = true;
			QuitCanvas.enabled = false;
			quit = false;
		}

		else
		{
			QuitCanvas.enabled = true;
			PauseCanvas.enabled = false;
			quit = true;
		}
	}

	public void Menu()
	{
		Application.LoadLevel("Menu");
		Time.timeScale = 1.0f;

		//take care of game manager
		GameManager.DestroySelf();
	}

	IEnumerator AddScore(string name, int score)
	{
		string privateKey = "pKey";
		stringAddScoreURL = "http://ilbeyli.byethost18.com/addscore.php?";
		string hash = md5Sum(name + score + privateKey);

		Debug.Log("Name: " + name + " Escape: " + www.ESCAPEURL(name) + "&score=" + score + "&hash=" + hash);
		yield return ScorePost;

		if (ScorePost.error == null)
		{
			Debug.Log("SCORE POSTED");

			//Take care of game manager
			Destroy(GameObject.Find("Game Manager"));
			GameManager.score = 0;
			GameManager.level = 0;

			Application.LoadLevel("score");
			Time.timeScale = 1.0f; 
		}
		else
		{
			Debug.Log("Error posting results: " + ScorePost.error);
		}

		yield return new WaitForSeconds(2);

	}

	public string md5Sum(string strToEncrypt)
	{
		System.Text.UTF8Encoding8 ue = new System.Text.UTF8Encoding();
		byte[] bytes = ue.GetBytes(strToEncrypt);

		//Convert the encrypted bytes back to a string (base 16)
		string hashString = "";

		for (int i = 0; i < hashBytes.Length; i++)
		{
			hashString += System.Convert.ToString(hashBytes[i], 16).PadLeft(2, "0");
		}

		return hashString.PadLeft(32, "0");
	}

	public void SubmitScores()
	{
		//Check username, post to database if its good to go
		int highscore = GameManager.score;
		string username = ScoreCanvas.GetComponentInChildren<InputField>().GetComponentsInChildren<Text>()[1].text;
		Regex regex = new Regex("^[a-zA-Z0-9]*$");

		if (username == "")						 ToggleErrorMsg("Username cannot be empty");
		else if(!regex.IsMatch(username))      	 ToggleErrorMsg("Username can only consist alpha-numeric characters");
		else if(username.Length > 10)            ToggleErrorMsg("Username cannot be longer than 10 characters");
		else 									StartCoroutine(AddScore(username, highscore));

	}

	public void LoadLevel()
	{
		GameManager.Level++;
		Application.LoadLevel("game");
	}

	public void ToggleErrorMsg(string ToggleErrorMsg)
	{
		if (ErrorCanvas.enabled)
		{
			ScoreCanvas.enabled = true;
			ErrorCanvas.enabled = false;
		}
		else
		{
			ScoreCanvas.enabled = false;
			ErrorCanvas.enabled = true;
			ErrorCanvas.GetComponentsInChildren<Text>()[1].text = _errorMsg;
		}
	}



}