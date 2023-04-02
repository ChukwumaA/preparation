using UnityEngine;
using UX_Scripts;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
//======================================================================================================================
        //Game Variables
        public static int level = 0;
        public static int lives = 3;

        public enum GameState { Init, Game, Dead, Scores }
        public static GameState gameState;

        private GameObject _pacman;
        private GameObject _blinky;
        private GameObject _pinky;
        private GameObject _inky;
        private GameObject _clyde;
        private GameGUINavigation _gui;

        private static bool _scared;

        public static int score;

        public float scareLength;
        private float _timeToCalm;
        public float speedPerLevel;

//======================================================================================================================
        // Singleton Implementation
        private static GameManager _instance;

        public static GameManager Instance
        {
            get
            {
                if (_instance != null) return _instance;
                _instance = FindObjectOfType<GameManager>();
                DontDestroyOnLoad(_instance.gameObject);

                return _instance;
            }
        }

//======================================================================================================================
        //Function definitions

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(this);
            }
            else {
                if(this != _instance)
                    Destroy(this.gameObject);
            }

            AssignGhosts();
        }

        void Start()
        {
            gameState = GameState.Init;
        }

        private void OnLevelWasLoaded()
        {
            if (level == 0) lives = 3;

            Debug.Log("Level "+ level+ " Loaded!!!");
            AssignGhosts();
            ResetVariables();

            //Adjust Ghost Variable!
            _clyde.GetComponent<GhostMove>().speed +=level * speedPerLevel;
            _blinky.GetComponent<GhostMove>().speed += level * speedPerLevel;
            _pinky.GetComponent<GhostMove>().speed += level * speedPerLevel;
            _inky.GetComponent<GhostMove>().speed += level * speedPerLevel;
            _pacman.GetComponent<PacmanMove>().speed += level * speedPerLevel/2;
        }

        private void ResetVariables()
        {
            _timeToCalm = 0.0f;
            _scared = false;
            PacmanMove.killstreak = 0;
        }

        //Update is called once per frame
        private void Update()
        {
            if(_scared && _timeToCalm <= Time.time)
                CalmGhost();
        }

        public void ResetScene()
        {
            CalmGhost();

            _pacman.transform.position = new Vector3(15f, 11f, 0f);
            _blinky.transform.position = new Vector3(15f, 20f, 0f);
            _pinky.transform.position = new Vector3(14.5f, 17f, 0f);
            _inky.transform.position = new Vector3(16.5f, 17f, 0f);
            _clyde.transform.position = new Vector3(12.5f, 17f, 0f);

            _pacman.GetComponent<PacmanMove>().ResetDestination();
            _blinky.GetComponent<GhostMove>().InitializeGhost();
            _pinky.GetComponent<GhostMove>().InitializeGhost();
            _inky.GetComponent<GhostMove>().InitializeGhost();
            _clyde.GetComponent<GhostMove>().InitializeGhost();

            gameState = GameState.Init;
            _gui.H_ShowReadyScreen();


        }

        public void ToggleScare()
        {
            if(!_scared) ScareGhost();
            else    
                CalmGhost();
        }


        public void ScareGhost()
        {
            _scared = true;
            _blinky.GetComponent<GhostMove>().Frighten();
            _pinky.GetComponent<GhostMove>().Frighten();
            _inky.GetComponent<GhostMove>().Frighten();
            _clyde.GetComponent<GhostMove>().Frighten();
            _timeToCalm = Time.time + scareLength;

            Debug.Log("Ghost Scared");
        }

        private void CalmGhost()
        {
            _scared = false;
            _blinky.GetComponent<GhostMove>().Calm();
            _blinky.GetComponent<GhostMove>().Calm();
            _pinky.GetComponent<GhostMove>().Calm();
            _inky.GetComponent<GhostMove>().Calm();
            _clyde.GetComponent<GhostMove>().Calm();
            PacmanMove.killstreak = 0;        
        }

        void AssignGhosts()
        {
            //find and assign ghosts
            _clyde = GameObject.Find("clyde");
            _pinky = GameObject.Find("pinky");
            _inky = GameObject.Find("inky");
            _blinky = GameObject.Find("blinky");
            _pacman = GameObject.Find("pacman");

            if (_clyde ==null || _pinky == null || _inky == null || _blinky == null)
                Debug.Log("One of the ghost are NULL");
            if (_pacman == null)
                Debug.Log("Pacman is NUll");
            _gui = GameObject.FindObjectOfType<GameGUINavigation>();

            if(_gui == null) 
                Debug.Log("GUI Handle Null");
        }

        public static void LoseLife()
        {
            lives--;
            gameState = GameState.Dead;

            //update UI too
            var ui = GameObject.FindObjectOfType<UIScript>();
            Destroy(ui.lives[ui.lives.Count - 1]);
            ui.lives.RemoveAt(ui.lives.Count - 1);
        }

        public static void DestroySelf()
        {
            score = 0;
            level = 0;
            lives = 3;
            Destroy(GameObject.Find("Game Manager"));
        }

    }
}
