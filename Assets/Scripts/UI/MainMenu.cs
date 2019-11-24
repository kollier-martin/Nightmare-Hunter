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

    [SerializeField] private Image image = null;

    public void NewGame()
    {
        StartCoroutine(Fade());
    }

    public void Continue()
    {
        /* If there is load data saved on the user's drive this button will be enabled
         * 
         * StartCoroutine(LoadQuietly(the scene with their data saved));
         * 
         */

        throw new NotImplementedException("Coming Soon :)");
    }

    public void Quit()
    {
        Application.Quit();
    }

    IEnumerator Fade()
    {
        for (float i = 0; i <= 1;)
        {
            music.volume -= 0.1f;
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
