using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Playables;

public enum State { PAUSED, PLAYING, GAMEOVER, CONSOLE }
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
    [SerializeField] private GameObject Timeline;

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

    private State gameState;
    private const float fadeTime = 1.0f;

    // Cheat Dictionary and Input
    [SerializeField] private GameObject inputFieldHolder = null;
    [SerializeField] private InputField inputField = null;
    [SerializeField] private Text logText = null; 
    IDictionary<string, Action> CheatDict;

    // Game Data and Scene Count
    public static GameData currentData;
    protected List<Scene> scenes;

    // Binary Formatter Serializes the Game Data
    BinaryFormatter bf;

    // Game Controller Instance
    private static GameController _instance = null;
    public static GameController Instance { get { return _instance; } }

    private void Awake()
    {
        CheatDict = new Dictionary<string, Action>();
        bf = new BinaryFormatter();
        World = GameObject.FindWithTag("World");
        Spawn = GameObject.FindWithTag("Respawn");
        player = FindObjectOfType<Player>();
        BossHandler = World.transform.GetChild(6).gameObject;
        music = World.GetComponent<AudioSource>();
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        if (_instance == null)
        {
            gameState = State.PLAYING;
            ExecuteEvents.Execute<MessageSystem>(player.gameObject, null, (x, y) => x.SpawnHere(Spawn));

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
            Destroy(gameObject);
        }

        CameraSwitch.CurrentCam = mainCam.GetComponent<Camera>();

        CheatDict.Add("kill", KillPlayer);
        CheatDict.Add("skip scene", LoadNextScene);
        CheatDict.Add("help", Help);
    }

    void Update()
    {
        // Game Controller Does Not Need To Exist In Menu
        if (SceneManager.GetActiveScene().name == "Menu")
        {
            Destroy(this.gameObject);
        }

        // Game State Handling
        switch (gameState)
        {
            case State.PAUSED:
                if (Input.GetButtonDown("Cancel"))
                {
                    PlayGame();
                    gameState = State.PLAYING;
                }
                break;
            case State.PLAYING:
                // Player presses start or escape
                if (Input.GetButtonDown("Cancel"))
                {
                    PauseGame();
                    gameState = State.PAUSED;
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

    public void PauseGame()
    {
        Time.timeScale = 0;
        PauseMenu.SetActive(true);
        World.SetActive(false);
    }

    public void PlayGame()
    {
        Time.timeScale = 1;
        PauseMenu.SetActive(false);
        World.SetActive(true);
    }

    public void GameOver()
    {
        // Have the audio switch to the death music, when player dies
        // GetComponent<AudioSource>().clip == DeathAudio;

        Time.timeScale = 0;
        DeathScreen.SetActive(true);
        World.SetActive(false);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void ContinueGame()
    {
        World.SetActive(true);
        DeathScreen.SetActive(false);


        gameState = State.PLAYING;

        ExecuteEvents.Execute<MessageSystem>(player.gameObject, null, (x, y) => x.SpawnHere(Spawn));

        Player.myHealth = 100;
        Time.timeScale = 1;
    }

    public void InstantiateMarlo()
    {
        music.Stop();

        Timeline.SetActive(true);

        mainCam.SetActive(false);
        CameraSwitch.CurrentCam = bossCam.GetComponent<Camera>();
        bossCam.SetActive(true);

        //ExecuteEvents.Execute<SpawnBoss>(BossHandler, null, (x, y) => x.PlaceMarlo());
    }

    public void BossIsDead()
    {
        player.bossIsDead = true;
    }

    private void KillPlayer()
    {
        ExecuteEvents.Execute<MessageSystem>(player.gameObject, null, (x, y) => x.Die());
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

            slider.value = progress;
            progressText.text = progress * 100f + "%";
            yield return null;
        }
    }

    public void FadeIn()
    {
        for (float i = 0; i <= 1;)
        {
            music.volume -= 0.1f;
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
                case ("skip scene"):
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

    public void ProcessPlayerData(Vector3 location, List<GameObject> GunsOwned, List<GameObject> Inventory, Scene currentScene)
    {
        currentData.playerLocation = location;
        currentData.GunsOwned = GunsOwned;
        currentData.ItemInventory = Inventory;
        currentData.scene = currentScene;
        currentData.CurrentWorldData = World;
    }

    void Save()
    {
        // Filestream creates a file using arguments
        // FileStream file = File.Create a file in the player's game destination
        FileStream file = File.Create(Application.persistentDataPath + "/SavedGame.sav");

        // Encode the data
        bf.Serialize(file, currentData);

        // Close Stream
        file.Close();
    }

    void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/SavedGame.sav"))
        {
            FileStream file = File.Open(Application.persistentDataPath + "/SavedGame.sav", FileMode.Open);

            // Populates current game data
            currentData = (GameData)bf.Deserialize(file);

            // Close Stream
            file.Close();
        }
    }

    [Serializable]
    public class GameData
    {
        public float playerHealth;
        public GameObject CurrentWorldData;
        public Scene scene;
        public List<GameObject> GunsOwned;
        public List<GameObject> ItemInventory;
        public Vector3 playerLocation;

        public GameData()
        {
            playerHealth = 100.0f;
            CurrentWorldData = null;
            scene = SceneManager.GetActiveScene();
            GunsOwned = null;
            ItemInventory = null;
            playerLocation = Vector2.zero;
        }
    }
}