using System;
using System.Collections;
using System.Linq;
using System.Text.RegularExpressions;
using Managers;
using UnityEngine;
using UnityEngine.UI;

namespace UX_Scripts
{
	public class GameGUINavigation : MonoBehaviour
	{
//======================================================================================================================
		//Variable Declaration
		private bool _paused;
		private bool _quit;

		public float initialDelay;

		//Canvas present in the User Experience Panels
		public Canvas pauseCanvas;
		public Canvas quitCanvas;
		public Canvas readyCanvas;
		public Canvas scoreCanvas;
		public Canvas errorCanvas;
		public Canvas gameOverCanvas;

		//Buttons for the players
		public Button menuButton;


//======================================================================================================================
		//Function Declaration

		//We will use this for initialization
		[Obsolete("Obsolete")]
		private void Start()
		{
			TogglePause();
			Menu();
			StartCoroutine(nameof(ShowReadyScreen), initialDelay);
		}

		private void Update()
		{
			if (!Input.GetKeyDown(KeyCode.Escape)) return;
			//go back to main menu, while in the game or playing the game
			if(GameManager.gameState == GameManager.GameState.Scores)
			{
			}

			//If in the game, toggle pause or quit dialogue
			else
			{
				if(_quit)
					ToggleQuit();
			}
		}


		//Public handle to show ready screen coroutine call
		public void H_ShowReadyScreen()
		{
			StartCoroutine(nameof(ShowReadyScreen), initialDelay);
		}

		[Obsolete("Obsolete")]
		public void H_ShowGameOverScreen()
		{
			StartCoroutine(nameof(ShowGameOverScreen));
		}

		private IEnumerator ShowReadyScreen(float seconds)
		{
			GameManager.gameState = GameManager.GameState.Init;
			readyCanvas.enabled = true;
			yield return new WaitForSeconds(seconds);
			GameManager.gameState = GameManager.GameState.Game;
		}

		[Obsolete("Obsolete")]
		private IEnumerator ShowGameOverScreen()
		{
			Debug.Log("Showing GAME OVER Screen");
			gameOverCanvas.enabled = true;
			yield return new WaitForSeconds(2);
			Menu();
		}

		public void GetScoresMenu()
		{
			Time.timeScale = 0f; //Stops the animation
			GameManager.gameState = GameManager.GameState.Scores;
			menuButton.enabled = false;
			scoreCanvas.enabled = true;
		}


//===========================================================================
		//Buttons functions
		private void TogglePause()
		{
			//if ESc key is not pressed, dont pause the game
			if(_paused)
			{
				Time.timeScale = 1;
				pauseCanvas.enabled = false;
				_paused = false;
				menuButton.enabled = true;
			}

			//if escape key is pressed, pause the game
			else
			{
				Time.timeScale = 0;
				pauseCanvas.enabled = true;
				_paused = true;
				menuButton.enabled = false;
			}

			Debug.Log("PauseCanvas enabled: " + pauseCanvas.enabled);
		}

		private void ToggleQuit()
		{
			if(_quit)
			{
				pauseCanvas.enabled = true;
				quitCanvas.enabled = false;
				_quit = false;
			}

			else
			{
				quitCanvas.enabled = true;
				pauseCanvas.enabled = false;
				_quit = true;
			}
		}

		[Obsolete("Obsolete")]
		private static void Menu()
		{
			Application.LoadLevel("Menu");
			Time.timeScale = 1.0f;

			//take care of game manager
			GameManager.DestroySelf();
		}

		[Obsolete("Obsolete")]
		private IEnumerator AddScore(string playerName, int score)
		{
			const string privateKey = "pKey";
			const string addScoreURL = "http://ilbeyli.byethost18.com/addscore.php?";
			var hash = MD5Sum(playerName + score + privateKey);
			
			Debug.Log("Name: " + name + " Escape: " + WWW.EscapeURL(name));
			WWW scorePost = new WWW(addScoreURL + "name=" + WWW.EscapeURL(name) + "&score=" + score + "&hash=" + hash );
			yield return scorePost;

			if (scorePost.error == null)
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
				Debug.Log("Error posting results: " + scorePost.error);
			}

			yield return new WaitForSeconds(2);

		}

		private string MD5Sum(string strToEncrypt)
		{
			var ue = new System.Text.UTF8Encoding();
			var bytes = ue.GetBytes(strToEncrypt);

			// encrypt bytes
			var md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
			var hashBytes = md5.ComputeHash(bytes);
			
			// Convert the encrypted bytes back to a string (base 16)
			var hashString = hashBytes.Aggregate("", (current, b) => current + Convert.ToString(b, 16).PadLeft(2, '0'));

			return hashString.PadLeft(32, '0');
		}

		[Obsolete("Obsolete")]
		public void SubmitScores()
		{
			//Check username, post to database if its good to go
			var highScore = GameManager.score;
			var userName = scoreCanvas.GetComponentInChildren<InputField>().GetComponentsInChildren<Text>()[1].text;
			var regex = new Regex("^[a-zA-Z0-9]*$");

			if (userName == "")						 ToggleErrorMsg("Username cannot be empty");
			else if(!regex.IsMatch(userName))      	 ToggleErrorMsg("Username can only consist alpha-numeric characters");
			else if(userName.Length > 10)            ToggleErrorMsg("Username cannot be longer than 10 characters");
			else 									StartCoroutine(AddScore(userName, highScore));

		}

		[Obsolete("Obsolete")]
		public void LoadLevel()
		{
			GameManager.level++;
			Application.LoadLevel("game");
		}

		private void ToggleErrorMsg(string toggleErrorMsg)
		{
			if (errorCanvas.enabled)
			{
				scoreCanvas.enabled = true;
				errorCanvas.enabled = false;
			}
			else
			{
				scoreCanvas.enabled = false;
				errorCanvas.enabled = true;
				errorCanvas.GetComponentsInChildren<Text>()[1].text = toggleErrorMsg;
			}
		}



	}
}