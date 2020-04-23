using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Singleton
    private static GameManager _instance;
    public static GameManager Instance => _instance; 
    void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
    }
    #endregion
    public GameObject gameOver;
    public GameObject nextLevel;
    public GameObject finishGame;
    public GameObject menu;
    [SerializeField]
    public int totalLives = 3;
    public int lives { get; set; }
    public int scoreKeeper;
    public static event Action<int> LiveLost;
    public bool isGameStarted { get; set; }

    private void Start()
    {
        this.lives = totalLives;
        Ball.onDeath += OnDeath;
        Brick.brickDestroy += OnBricksDestroy;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnBricksDestroy(Brick obj)
    {
        if(BrickManager.Instance.RemainingBricks.Count <= 0)
        {
            BallManager.Instance.ResetBalls();
            isGameStarted = false;
            BrickManager.Instance.LoadNextLevel();
        }
    }

    private void OnDeath(Ball obj)
    {
        if(BallManager.Instance.Balls.Count <= 0)
        {
            UIManager.Instance.score = scoreKeeper;
            this.lives--;
            if(this.lives < 1)
            {
                gameOver.SetActive(true);
            }
            else
            {
                LiveLost?.Invoke(this.lives);
                BallManager.Instance.ResetBalls();
                isGameStarted = false;
                BrickManager.Instance.LoadLevel(BrickManager.Instance.currentLevel);
            }
        }
    }

    public void Startlevel()
    {
        nextLevel.SetActive(false);
    }

    public void ShowNextLevel()
    {   
        // Bonus points 
        int score = UIManager.Instance.score;
        int livesRemaining = lives;
        UIManager.Instance.score += lives * 5;

        scoreKeeper = UIManager.Instance.score; 

        nextLevel.SetActive(false);
        BrickManager.Instance.LoadLevel(BrickManager.Instance.currentLevel);
    }

    public void ShowVictoryScreen()
    {
        finishGame.SetActive(true);
    }

    public void BackToMenu()
    {
        finishGame.SetActive(false);
        menu.SetActive(true);
    }
    public void QuitGame()
    {
        Application.Quit();
    }

    public void StartGame()
    {
        BrickManager.Instance.GenerateBrick();
        menu.SetActive(false);
    }

    private void OnDisable()
    {
        Ball.onDeath -= OnDeath;
        Brick.brickDestroy -= OnBricksDestroy;
    }
}
