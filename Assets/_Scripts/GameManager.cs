using UnityEngine;
using TMPro; // Import TextMeshPro namespace

public class GameManager : SingletonMonoBehavior<GameManager>
{
    [SerializeField] private int maxLives = 3;
    [SerializeField] private Ball ball;
    [SerializeField] private Transform bricksContainer;
    [SerializeField] private TMP_Text scoreText; // Use TMP_Text instead of Text
    [SerializeField] private TMP_Text livesText; // Use TMP_Text instead of Text
    [SerializeField] private GameObject gameOverPanel;

    private int currentBrickCount;
    private int totalBrickCount;
    private int score = 0;
    private int currentLives;

    private void OnEnable()
    {
        InputHandler.Instance.OnFire.AddListener(FireBall);
        ball.ResetBall();
        totalBrickCount = bricksContainer.childCount;
        currentBrickCount = bricksContainer.childCount;
        currentLives = maxLives;
        UpdateUI();
    }

    private void OnDisable()
    {
        InputHandler.Instance.OnFire.RemoveListener(FireBall);
    }

    private void FireBall()
    {
        ball.FireBall();
    }

    public void OnBrickDestroyed(Vector3 position)
    {
        currentBrickCount--;
        score += 10;
        UpdateUI();
        Debug.Log($"Destroyed Brick at {position}, {currentBrickCount}/{totalBrickCount} remaining");

        if (currentBrickCount == 0)
            SceneHandler.Instance.LoadNextScene();
    }

    public void KillBall()
    {
        currentLives--;
        UpdateUI();

        if (currentLives == 0)
        {
            GameOver();
        }
        else
        {
            ball.ResetBall();
        }
    }

    private void UpdateUI()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + score;
        if (livesText != null)
            livesText.text = "Lives: " + currentLives;
    }

    private void GameOver()
    {
        Time.timeScale = 0; // Pause game
        gameOverPanel.SetActive(true);
        Invoke("ReturnToMenu", 2f);
    }

    private void ReturnToMenu()
    {
        Time.timeScale = 1; // Resume game
        SceneHandler.Instance.LoadMenuScene();
    }
}
