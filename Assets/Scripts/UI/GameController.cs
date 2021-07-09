using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Linq;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Playables;

public enum State { PAUSED, PLAYING, GAMEOVER, CONSOLE, MENU }
public class GameController : MonoBehaviour, IEventSystemHandler
{
    // Handles Pause, Play, Settings, Saving, Loading

    // Scene Loading Values
    [SerializeField] private AudioSource music;
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private Slider slider;
    [SerializeField] private Text progressText;
    [SerializeField] private Image image;
    [SerializeField] public AudioSource BossMusic;
    [SerializeField] public AudioSource AfterBossMusic;
    [SerializeField] public GameObject Timeline;

    // Rendering Cameras
    [SerializeField]
    private GameObject mainCam, bossCam;

    // World Values
    [SerializeField] private GameObject PauseMenu = null;
    [SerializeField] private GameObject BossHandler = null;
    [SerializeField] private GameObject DeathScreen = null;
    [SerializeField] private GameObject World = null;
    [SerializeField] private GameObject Spawn = null;
    [SerializeField] private Player player = null;

    private List<GameObject> SpawnPoints = new List<GameObject>();
    private List<Camera> CamerasInScene = new List<Camera>();

    public State gameState;
    private const float fadeTime = 1.0f;

    // Cheat Dictionary and Input
    [SerializeField] private GameObject inputFieldHolder = null;
    [SerializeField] private InputField inputField = null;
    [SerializeField] private Text logText = null; 
    IDictionary<string, Action> CheatDict = new Dictionary<string, Action>();

    // Game Data and Scene Count
    public static GameData currentData;
    private List<Scene> scenes;

    // Game Controller Instance
    private static GameController _instance = null;
    public bool cutsceneDone;

    Action LoadFromMenu;
    private string SavePath;

    public static GameController Instance { get { return _instance; } }

    void Awake()
    {
        // Event handlers for specific scenes when they load
        SceneManager.sceneLoaded += this.OnLoadCallback;
        LoadFromMenu += this.OnGameStateLoad;

        // if creates a GameController instance and populates the GameController 'scenes' variable with all valid scenes and it loads up a new GameData (used for saves)
        // else indicates that the GameController be destroyed if there is another already in the scene
        if (_instance == null)
        {
            gameState = State.PLAYING;

            scenes = new List<Scene>();

            for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
            {
                scenes.Add(SceneManager.GetSceneByBuildIndex(i));
            }

            currentData = new GameData();

            DontDestroyOnLoad(this.gameObject);
            _instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        try
        {
            // populates the 'Spawns' variable with all possible spawn points in the scene 
            var Spawns = GameObject.FindGameObjectsWithTag("SpawnPoint");
            foreach (GameObject spawn in Spawns)
            {
                SpawnPoints.Add(spawn);
            }

            // populates the 'Cameras' variable with all cameras in the scene
            var Cameras = FindObjectsOfType<Camera>();
            
            // ERROR HERE MULTIPLE MAIN CAMERAS POPULATING. FIXME
            //Debug.Log(Cameras.);
            foreach (Camera cam in Cameras)
            {
                CamerasInScene.Add(cam);
            }

            // indicates where a game save will be located
            SavePath = Application.persistentDataPath + "/SavedGame.sav";

            // get world variable for current world
            World = GameObject.FindWithTag("World");

            // get player spawn point
            Spawn = GameObject.FindWithTag("Respawn");

            // get player's object data
            player = FindObjectOfType<Player>();

            // for all cameras in the scene, set them to their correct values
            for (int i = 0; i < CamerasInScene.Count; i++)
            {
                if (CamerasInScene[i].tag == "BossCamera")
                {
                    bossCam = CamerasInScene[i].gameObject;
                    break;
                }

                if (CamerasInScene[i].tag == "MainCamera")
                {
                    mainCam = CamerasInScene[i].gameObject;
                    break;
                }
            }

            // get object values for Timeline and BossHandler
            BossHandler = GameObject.FindGameObjectWithTag("Boss Handler");
            Timeline = GameObject.FindGameObjectWithTag("Timeline");

            // set bossCam and Timeline inactive, for use later
            bossCam.SetActive(false);
            Timeline.SetActive(false);
            
            // get current music from the world
            music = World.GetComponent<AudioSource>();

            // CameraSwitch is a class that handles which camera is current
            CameraSwitch.CurrentCam = mainCam.GetComponent<Camera>();

            // commands testing, incomplete
            CheatDict.Add("kill", KillPlayer);
            CheatDict.Add("skip", LoadSceneNoFade);
            CheatDict.Add("help", Help);

            // if the Scene is not the main menu, spawn the player on the set spawn point
            if (SceneManager.GetActiveScene().buildIndex > 1)
            {
                ExecuteEvents.Execute<MessageSystem>(player.gameObject, null, (x, y) => x.SpawnHere(Spawn));
            }

            DontDestroyOnLoad(gameObject);
        }
        catch
        {
            // Unhandled exception catch || FIXME
        }
    }

    void OnLoadCallback(Scene scene, LoadSceneMode sceneMode)
    {
        // OnLoadCallback is ran when a scene is switched to see if that scene matches the name
        // if Win Screen is loaded, delete Game Controller ; else clear all Cameras and SpawnPoints then run start again to redo all variable populations
        if (scene.name == "Win Screen")
        {
            Destroy(gameObject);
        }
        else
        {
            CamerasInScene.Clear();
            SpawnPoints.Clear();
            Start();
        }
    }

    // Used in the load game function. populates the player with the saved player data
    // incomplete function
    void OnGameStateLoad()
    {
        ExecuteEvents.Execute<Player>(player.gameObject, null, (x, y) => x.UpdateData(currentData.GunsOwned, currentData.ItemInventory));
    }
    
    void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex > 1)
        {
            // if the boss cutscene is done, then start fight music and set cutscene done to false again
            if (cutsceneDone == true)
            {
                music.Stop();
                BossMusic.Play();
                cutsceneDone = false;
            }

            // Game State Handling
            switch (gameState)
            {
                case State.MENU:
                    break;

                case State.PAUSED:
                    if (Input.GetButtonDown("Cancel"))
                    {
                        PlayGame();
                        
                    }
                    break;
                case State.PLAYING:
                    // Player presses start or escape
                    if (Input.GetButtonDown("Cancel"))
                    {
                        PauseGame();
                    }

                    // If player is dead
                    if (player.tag == "Dead")
                    {
                        gameState = State.GAMEOVER;
                    }

                    // For console integration.. eventually
                    if (Input.GetKeyDown(KeyCode.BackQuote))
                    {
                        Time.timeScale = 0;
                        inputFieldHolder.SetActive(true);
                        gameState = State.CONSOLE;
                    }

                    break;
                case State.GAMEOVER:
                    // Use some type of System to see when player dies
                    GameOver();
                    break;
                case State.CONSOLE:
                    // Close console and resume game
                    if (Input.GetKeyDown(KeyCode.BackQuote))
                    {
                        Time.timeScale = 1;
                        inputFieldHolder.SetActive(false);
                        gameState = State.PLAYING;
                    }
                    break;
            }
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        PauseMenu.SetActive(true);
        gameState = State.PAUSED;
    }

    public void PlayGame()
    {
        Time.timeScale = 1;
        PauseMenu.SetActive(false);
        gameState = State.PLAYING;
    }

    public void GameOver()
    {
        // Have the audio switch to the death music, when player dies

        Time.timeScale = 0;
        DeathScreen.SetActive(true);
        World.SetActive(false);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
        gameState = State.MENU;
    }

    public void ContinueGame()
    {
        // On death, pressing continue runs this function
        // I'm sure this function does not reset the scene, so that will be fixed
        World.SetActive(true);
        DeathScreen.SetActive(false);


        gameState = State.PLAYING;

        ExecuteEvents.Execute<MessageSystem>(player.gameObject, null, (x, y) => x.SpawnHere(Spawn));

        player.myHealth = 100;
        Time.timeScale = 1;
    }

    public void InstantiateMarlo()
    {
        // Spawn Boss. I want a better way to spawn bosses, but this is just a test run for now
        player.speed = 0;
        player.GetComponent<PlayerController>().jumpForce = 0f;
        ExecuteEvents.Execute<Cave>(World.gameObject, null, (x, y) => x.TurnDownMusic());

        mainCam.SetActive(false);
        bossCam.SetActive(true);
        CameraSwitch.CurrentCam = bossCam.GetComponent<Camera>();

        Timeline.SetActive(true);
        Timeline.GetComponent<PlayableDirector>().Play();
    }

    public void CutsceneDone(bool val)
    {
        if (val == true)
        {
            cutsceneDone = true;
            Timeline.SetActive(false);
        }
        else
        {
            cutsceneDone = false;
        }
    }

    public void BossIsDead()
    {
        ExecuteEvents.Execute<Cave>(World.gameObject, null, (x, y) => x.TurnOffMusic());
        BossMusic.Stop();
        AfterBossMusic.Play();
        player.bossIsDead = true;
    }

    private void KillPlayer()
    {
        ExecuteEvents.Execute<MessageSystem>(player.gameObject, null, (x, y) => x.Die());
    }

    public void LoadSceneNoFade()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void LoadNextScene()
    {
        StartCoroutine(LoadQuietly(SceneManager.GetActiveScene().buildIndex + 1));
    }

    IEnumerator LoadQuietly(int sceneIndex)
    {
        // Loading Screen
        FadeIn();
        yield return new WaitForSeconds(fadeTime);

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        loadingScreen.SetActive(true);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);

            slider.value = (int) progress;
            progressText.text = (int) progress * 100f + "%";
            yield return null;
        }
    }

    public void FadeIn()
    {
        for (float i = 0; i <= 1;)
        {
            World.GetComponent<AudioSource>().volume -= 0.1f;
            i += 0.1f;
        }

        image.CrossFadeAlpha(1.0f, fadeTime, false);
    }

    #region Commands Console
    // Uses a dictionary to attach commands to a text input within the command console
    public void Help()
    {
        logText.text = "Commands:\n";

        foreach (string key in CheatDict.Keys)
        {
            switch(key)
            {
                case ("kill"):
                    logText.text += key.ToString() + " : Kills the player\n";
                    break;
                case ("skip"):
                    logText.text += key.ToString() + " : Loads the next scene\n";
                    break;
                case ("help"):
                    logText.text += key.ToString() + " : Prints every avaialble command";
                    break;
            }
            
        }
    }

    public void CheatCodeParser()
    {
        CheckInput(inputField.text);
    }

    void CheckInput(string str)
    {
        if (CheatDict.ContainsKey(str) == false)
        {
            return;
        }

        CheatDict[str]();
    }
    #endregion

    public void ProcessPlayerData(List<GameObject> GunsOwned, List<GameObject> Inventory, int currentScene)
    {
        currentData.GunsOwned = GunsOwned;
        currentData.ItemInventory = Inventory;
        currentData.scene = currentScene;
        currentData.CurrentWorldData = World;
    }
    

    public void ExitGame()
    {
        Application.Quit();
    }

    public void Save()
    {
        // Using Json Rather than Binary Formatter
        string json = JsonUtility.ToJson(currentData, true);
        File.WriteAllText(SavePath, json);
    }

    public GameData Load()
    {
        // Looks functional. Scared to test if it works lol
        if (File.Exists(SavePath))
        {
            string contents = File.ReadAllText(SavePath);

            if (string.IsNullOrEmpty(contents))
            {
                Debug.Log("This save file is empty.");
            }

            // Populates current game data
            return JsonUtility.FromJson<GameData>(contents);
        }
        else if (!File.Exists(SavePath))
        {
            throw new NullPathException("Game data does not exist.");
        }

        return new GameData();
    }

    [Serializable]
    public class GameData
    {
        public float playerHealth;
        public GameObject CurrentWorldData;
        public int scene;
        public List<GameObject> GunsOwned;
        public List<GameObject> ItemInventory;

        public GameData()
        {
            playerHealth = 100.0f;
            CurrentWorldData = null;
            scene = SceneManager.GetActiveScene().buildIndex;
            GunsOwned = new List<GameObject>();
            ItemInventory = new List<GameObject>();
        }
    }

    /// Custom Exception for Loading and all that Jazz.
    public class NullPathException : Exception
    {
        public NullPathException(string message)
        {
            Debug.Log(message);
        }
    }
}