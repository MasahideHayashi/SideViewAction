using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject gameOverText;
    [SerializeField] GameObject gameClearText;
    [SerializeField] Text scoreText;

    int score = 0;
    static int MAX_SCORE = 9999;

    private void Start()
    {
        scoreText.text = score.ToString();
    }

    public void AddScore(int val)
    {
        score += val;
        if (score >= MAX_SCORE)
        {
            score = MAX_SCORE;
        }
        scoreText.text = score.ToString();
    }

    public void GameOver()
    {
        gameOverText.SetActive(true);
        Invoke("RestartScene", 1.5f);
    }
    public void GameClear()
    {
        gameClearText.SetActive(true);
        Invoke("RestartScene", 1.5f);
    }

    void RestartScene()
    {
        Scene thisScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(thisScene.name);
    }
}
