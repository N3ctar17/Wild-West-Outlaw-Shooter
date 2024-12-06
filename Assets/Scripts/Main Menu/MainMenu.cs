using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private SceneController _sceneController;
    public void Play() {
        _sceneController.LoadScene("Game");
    }

    public void Exit() {
        Application.Quit();
    }

    public void Back() {
        _sceneController.LoadScene("Main Menu");
    }

    public void Controls() {
        _sceneController.LoadScene("Controls");
    }
}
