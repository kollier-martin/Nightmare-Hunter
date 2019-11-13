using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ToMenu : MonoBehaviour
{
    // Start is called before the first frame update

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
