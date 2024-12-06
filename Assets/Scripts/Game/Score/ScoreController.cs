using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class ScoreController : MonoBehaviour
{
    public UnityEvent OnScoreChanged;
    public int Score { get; private set; }

    // Maximum score per round
    private readonly int _scoreThreshold = 500;

    public void AddScore(int amount) {
        Score += amount;
        OnScoreChanged.Invoke();

        // Check if the score meets the threshold for a scene change
        if (Score >= _scoreThreshold) {
            ChangeToNextScene();
        }
    }

    private void ChangeToNextScene() {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        // Check if there's a next scene to load
        if (currentSceneIndex + 1 < SceneManager.sceneCountInBuildSettings) {
            int nextSceneIndex = currentSceneIndex + 1;

            // Load the next scene
            SceneManager.LoadScene(nextSceneIndex);
        }
        else {
            Debug.Log("No more scenes to load or already in Endless.");
        }
    }
}
