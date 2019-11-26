using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class TimelineManager : MonoBehaviour, IEventSystemHandler
{
    [SerializeField] private GameObject CurrentGameController;
    bool finished;

    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private Image image;
    [SerializeField] private Player player;

    private PlayableDirector cutscene;
    private const float fadeTime = 1.0f;

    private void Awake()
    {
        if (CurrentGameController == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
            CurrentGameController = GameObject.FindGameObjectWithTag("GameController");
        }
        
        cutscene = GetComponent<PlayableDirector>();

        if (SceneManager.GetActiveScene().name == "Opening Cutscene")
        {
            StartCoroutine(FadeIn());
        }
    }

    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "Opening Cutscene")
        {
            StartCoroutine(StartCutsceneWithDelay());
        }
    }

    private void Update()
    {
        if (finished && SceneManager.GetActiveScene().name == "Opening Cutscene")
        {
            loadingScreen.SetActive(true);
        }
        else if (finished)
        {
            ExecuteEvents.Execute<GameController>(CurrentGameController.gameObject, null, (x, y) => x.CutsceneDone(finished));
        }
    }

    IEnumerator FadeIn()
    {
        image.CrossFadeAlpha(0.0f, fadeTime, false);

        if (finished && SceneManager.GetActiveScene().name == "Opening Cutscene")
        {
            // Fixes player from looking like she's floating
            foreach (Collider2D collider in player.GetComponents<Collider2D>())
            {
                collider.enabled = false;
            }
        }

        yield return new WaitForSeconds(fadeTime);

        if (finished && SceneManager.GetActiveScene().name == "Opening Cutscene")
        {
            // Reverts the "fix"
            foreach (Collider2D collider in player.GetComponents<Collider2D>())
            {
                collider.enabled = true;
            }
        }
    }

    IEnumerator StartCutsceneWithDelay()
    {
        player.speed = 0;
        player.GetComponent<PlayerController>().jumpForce = 0f;
        yield return new WaitForSeconds(0.0f);
        cutscene.Play();
    }

    // Actions
    void OnEnable()
    {
        cutscene.stopped += OnPlayableDirectorStopped;
    }

    void OnPlayableDirectorStopped(PlayableDirector aDirector)
    {
        if (cutscene == aDirector)
        {
            player.speed = 12;
            player.GetComponent<PlayerController>().jumpForce = 4.5f;
            finished = true;
        }
    }

    void OnDisable()
    {
        cutscene.stopped -= OnPlayableDirectorStopped;
    }
}