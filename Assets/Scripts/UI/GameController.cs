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

    private void Start()
    {
        try
        {
            SavePath = Application.persistentDataPath + "/SavedGame.sav";
            World = GameObject.FindWithTag("World");
            Spawn = GameObject.FindWithTag("Respawn");
            player = FindObjectOfType<Player>();
            bossCam = GameObject.FindGameObjectWithTag("BossCamera");
            mainCam = GameObject.FindGameObjectWithTag("MainCamera");
            BossHandler = GameObject.FindGameObjectWithTag("Boss Handler");
            Timeline = GameObject.FindGameObjectWithTag("Timeline");

            bossCam.SetActive(false);
            Timeline.SetActive(false);
            
            music = World.GetComponent<AudioSource>();

            CameraSwitch.CurrentCam = mainCam.GetComponent<Camera>();

            CheatDict.Add("kill", KillPlayer);
            CheatDict.Add("skip", LoadSceneNoFade);
            CheatDict.Add("help", Help);

            if (SceneManager.GetActiveScene().buildIndex > 1)
            {
                ExecuteEvents.Execute<MessageSystem>(player.gameObject, null, (x, y) => x.SpawnHere(Spawn));
            }

            DontDestroyOnLoad(gameObject);
        }
        catch
        {

        }
    }

    void OnLoadCallback(Scene scene, LoadSceneMode sceneMode)
    {
        if (scene.name == "Win Screen")
        {
            Destroy(gameObject);
        }
        else
        {
            Start();
        }
    }

    void OnGameStateLoad()
    {
        ExecuteEvents.Execute<Player>(player.gameObject, null, (x, y) => x.UpdateData(currentData.GunsOwned, currentData.ItemInventory));
    }
    
    void Awake()
    {
        SceneManager.sceneLoaded += this.OnLoadCallback;
        LoadFromMenu += this.OnGameStateLoad;

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

    void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex > 1)
        {
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
        World.SetActive(true);
        DeathScreen.SetActive(false);


        gameState = State.PLAYING;

        ExecuteEvents.Execute<MessageSystem>(player.gameObject, null, (x, y) => x.SpawnHere(Spawn));

        player.myHealth = 100;
        Time.timeScale = 1;
    }

    public void InstantiateMarlo()
    {
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

    /// Custom Exception
    public class NullPathException : Exception
    {
        public NullPathException(string message)
        {
            Debug.Log(message);
        }
    }
}