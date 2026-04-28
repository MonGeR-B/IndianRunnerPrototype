using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public float gameSpeed = 10f;
    public float speedIncreaseRate = 0.1f;
    public float maxSpeed = 25f;

    public TMP_Text scoreText;

    private float score = 0f;
    private bool isGameOver = false;

    void Awake()
    {
        instance = this;
    }

    void Update()
    {
        if (isGameOver)
            return;

        // Increase speed over time
        gameSpeed += speedIncreaseRate * Time.deltaTime;
        gameSpeed = Mathf.Clamp(gameSpeed, 10f, maxSpeed);

        // Increase score
        score += gameSpeed * Time.deltaTime;

        if (scoreText != null)
            scoreText.text = "Score: " + Mathf.FloorToInt(score);
    }

    public void GameOver()
    {
        isGameOver = true;
    }
}