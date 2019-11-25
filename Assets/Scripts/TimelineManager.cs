using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class TimelineManager : MonoBehaviour
{
    GameController CurrentGameController;

    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private Image image;
    [SerializeField] private Player player;

    private PlayableDirector cutscene;
    private const float fadeTime = 1.0f;

    private void Awake()
    {
        try
        {
            CurrentGameController = FindObjectOfType<GameController>();
        }
        catch
        {

        }
        
        cutscene = GetComponent<PlayableDirector>();
        StartCoroutine(FadeIn());
    }

    private void Start()
    {
        StartCoroutine(StartCutsceneWithDelay());
    }

    private void Update()
    {
        if (cutscene.time == cutscene.duration && SceneManager.GetActiveScene().name == "Opening Cutscene")
        {
            Destroy(player);
            loadingScreen.SetActive(true);
        }
        else if (cutscene.time == cutscene.duration)
        {
            CurrentGameController.BossMusic.Play();
            this.gameObject.SetActive(false);
        }
    }

    IEnumerator FadeIn()
    {
        image.CrossFadeAlpha(0.0f, fadeTime, false);

        // Fixes player from looking like she's floating
        foreach (Collider2D collider in player.GetComponents<Collider2D>())
        {
            collider.enabled = false;
        }

        yield return new WaitForSeconds(4.0f);

        // Reverts the "fix"
        foreach (Collider2D collider in player.GetComponents<Collider2D>())
        {
            collider.enabled = true;
        }
    }

    IEnumerator StartCutsceneWithDelay()
    {
        yield return new WaitForSeconds(2.0f);
        cutscene.Play();
    }
}