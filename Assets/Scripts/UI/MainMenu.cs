using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class MainMenu : MonoBehaviour
{
    private const float fadeTime = 1.0f;
    public TextMeshProUGUI[] MainMenuText;

    public GameObject loadingScreen;
    public Slider slider;
    public Text progressText;
    public AudioSource music;

    [SerializeField] private Image image = null;

    public void NewGame()
    {
        StartCoroutine(LoadQuietly(SceneManager.GetActiveScene().buildIndex + 1));
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

    IEnumerator LoadQuietly(int sceneIndex) 
    {
        Fade();
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

    public void Fade()
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
    }
}
