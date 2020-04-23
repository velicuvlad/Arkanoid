using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    #region Singleton
    private static UIManager _instance;
    public static UIManager Instance => _instance;
    void Awake()
    {
        Brick.brickDestroy += BrickDestroyed;
        BrickManager.LevelLoaded += OnLevelLoad;
        GameManager.LiveLost += OnLiveLost;
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
    public Text TargetsText;
    public Text ScoreText;
    public Text LivesText;

    public int score;

    private void Start()
    {
        OnLiveLost(GameManager.Instance.totalLives);
    }

    private void OnLiveLost(int remainingLives)
    {
        LivesText.text = $"Lives:{remainingLives}";
    }
    private void OnLevelLoad()
    {
        UpdateTargetsText();
        UpdateScoreText(0);

    }

    private void UpdateScoreText(int current)
    {
        this.score += current;
        string scoreString = this.score.ToString().PadLeft(4, '0');
        ScoreText.text = $"Score: {scoreString}";
    }

    private void BrickDestroyed(Brick obj)
    {
        UpdateTargetsText();
        UpdateScoreText(1);
    }

    private void UpdateTargetsText()
    {
        TargetsText.text = $"Targets: {BrickManager.Instance.RemainingBricks.Count}/{BrickManager.Instance.initialBrickNo}";
    }
    
    private void OnDisable()
    {
        Brick.brickDestroy -= BrickDestroyed;
        BrickManager.LevelLoaded -= OnLevelLoad;
        GameManager.LiveLost -= OnLiveLost;
    }
}

