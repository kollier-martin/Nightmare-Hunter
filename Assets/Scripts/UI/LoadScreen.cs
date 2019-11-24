using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadScreen : MonoBehaviour
{
    public Slider slider;
    public Text progressText;

    // Start is called before the first frame update
    void Start()
    {
        LoadScene();
    }

    // Update is called once per frame
    void LoadScene()
    {
        StartCoroutine(LoadQuietly(SceneManager.GetActiveScene().buildIndex + 1));
    }

    IEnumerator LoadQuietly(int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);

            slider.value = progress;
            progressText.text = progress * 100f + "%";
            yield return null;
        }
    }
}
