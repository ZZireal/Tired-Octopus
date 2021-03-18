using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuControls : MonoBehaviour
{
    public GameObject loadingCanvas; 

    public void PlayPressed() 
    {
        Debug.Log("Play pressed!");
        gameObject.SetActive(false);
        loadingCanvas.SetActive(true);
        SceneManager.LoadSceneAsync("Game");
    }

    public void ExitPressed()
    {
        Debug.Log("Exit pressed!");
        Application.Quit();
    }
}
