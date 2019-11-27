using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class MainMenu : MonoBehaviour
{
    private const float fadeTime = 2.0f;
    public TextMeshProUGUI[] MainMenuText;

    public GameObject loadingScreen;
    public Slider slider;
    public Text progressText;
    public AudioSource music;
    GameController controller;

    [SerializeField] private Image image = null;

    private void Awake()
    {
        controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();

        controller.gameState = State.MENU;
    }

    public void NewGame()
    {
        StartCoroutine(Fade());
        controller.gameState = State.PLAYING;
    }

    public void Continue()
    {
        controller.Load();
        controller.gameState = State.PLAYING;
    }

    public void Quit()
    {
        Application.Quit();
    }

    IEnumerator Fade()
    {
        for (float i = 0; i <= 1;)
        {
            GetComponent<AudioSource>().volume -= 0.1f;
            i += 0.1f;
        }

        image.CrossFadeAlpha(0.0f, fadeTime, false);

        foreach (TextMeshProUGUI element in MainMenuText)
        {
            element.text = "";
        }

        yield return new WaitForSeconds(fadeTime);

        loadingScreen.SetActive(true);
    }

}
