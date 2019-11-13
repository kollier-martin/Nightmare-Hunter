using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.EventSystems;

public enum State { PAUSED, PLAYING, GAMEOVER }
public class GameController : MonoBehaviour, IEventSystemHandler
{
    // Handles Pause, Play, Settings, Saving, Loading

    [SerializeField] private GameObject PauseMenu = null;
    [SerializeField] private GameObject DeathScreen = null;
    [SerializeField] private GameObject World;
    [SerializeField] private GameObject Spawn;
    [SerializeField] private GameObject Player;

    private State gameState;

    string[] cheatCode = new string[] { "k", "i", "l", "l" };
    private int index = 0;

    public static GameData currentData;
    public List<Scene> scenes;

    public static event Action DeadPlayer;

    private static GameController _instance = null;
    public static GameController Instance { get { return _instance; } }

    private void Awake()
    { 
        DontDestroyOnLoad(this.gameObject);
        World = GameObject.FindWithTag("World");
        Spawn = GameObject.FindWithTag("Respawn");
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    // Start is called before the first frame update
    void Start()
    {
        if (_instance == null)
        {
            gameState = State.PLAYING;
            ExecuteEvents.Execute<MessageSystem>(Player, null, (x, y) => x.SpawnHere(Spawn));

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
    }
    
    void Update()
    {
        CheatCodeParser();
        Debug.Log(Input.GetAxis("DPadX"));

        if (SceneManager.GetActiveScene().name == "Menu")
        {
            Destroy(this.gameObject);
        }

        switch (gameState)
        {
            case State.PAUSED:
                Cursor.visible = true;

                if (Input.GetButtonDown("Cancel"))
                {
                    PlayGame();
                    gameState = State.PLAYING;
                }
                break;
            case State.PLAYING:
                Cursor.visible = false;

                if (Input.GetButtonDown("Cancel"))
                {
                    PauseGame();
                    gameState = State.PAUSED;
                }

                /*if (DeadPlayer != null) //If player is dead
                {
                    gameState = State.GAMEOVER;
                }*/

                break;
            case State.GAMEOVER:
                // Use some type of System to see when player dies
                Cursor.visible = true;
                GameOver();
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

    private void GameOver()
    {
        // Have the audio switch to the death music, when player dies
        // GetComponent<AudioSource>().clip == DeathAudio;

        Time.timeScale = 0;
        DeathScreen.SetActive(true);
        World.SetActive(false);
    }

    public void LoadNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private void CheatCodeParser()
    {
        // Check if any key is pressed
        if (Input.anyKeyDown)
        {
            // Check if the next key in the code is pressed
            if (Input.GetKeyDown(cheatCode[index]))
            {
                // Add 1 to index to check the next key in the code
                if (index < cheatCode.Length)
                {
                    index++;
                }
                // Wrong key entered, we reset code typing
                else
                {
                    index = 0;
                }
            }
        }

        // If index reaches the length of the cheatCode string, 
        // the entire code was correctly entered
        if (index == cheatCode.Length)
        {
            // Tell Player to die
            ExecuteEvents.Execute<MessageSystem>(Player, null, (x, y) => x.Die());

            // Reset cheat code index
            index = 0;
        }
    }

    void Save()
    {
        // Binary Formatter Serializes
        BinaryFormatter bf = new BinaryFormatter();

        // Filestream creates a file using arguments
        // FileStream file = File.Create a file in the player's game destination
        // bf.Serialize(file)
    }

    void Load()
    {

    }

    [Serializable]
    public class GameData
    {
        public int PlayerHealth { get; set; }
        public Scene scene;
        public GameObject[] GunsOwned;
        public Vector2 playerLocation;

        public GameData()
        {
            PlayerHealth = 0;
            scene = SceneManager.GetActiveScene();
            GunsOwned = null;
            playerLocation = Vector2.zero;
        }
    }
}