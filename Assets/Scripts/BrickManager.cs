using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class BrickManager : MonoBehaviour
{
    #region Singleton
    private static BrickManager _instance;
    public static BrickManager Instance => _instance;
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
    public Material[] mats;
    public Color[] colors;
    public List<int[,]> levels { get; set; }
    public List<Brick> RemainingBricks { get; set; }
    public int initialBrickNo;

    
    private GameObject _bricksContainer;
    private int _maxRows = 8;
    private int _maxCols = 10;
    private float _firstBrickX = -2.8f;
    private float _firstBrickY = 2.75f;
    private float _shiftX = 0.7f;
    private float _shiftY = 0.3f;

    public static event Action LevelLoaded;
    public int currentLevel;
    [SerializeField]
    private Brick _brickPrefab;
   
    private void Start()
    {
        this._bricksContainer = new GameObject("BricksContainer");
        this.levels = this.LoadLevelData();
        
    }
    private List<int[,]> LoadLevelData()
    {
        TextAsset text = Resources.Load("levels") as TextAsset;
        string[] rows = text.text.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
        List<int[,]> levels = new List<int[,]>();
        int[,] currentLevel = new int[_maxRows, _maxCols];

        int currentRow = 0;

        for (int i = 0; i < rows.Length; i++)
        {
            string line = rows[i];
            if (line.IndexOf("--") == -1)
            {
                string[] bricks = line.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                for (int j = 0; j < bricks.Length; j++)
                {
                    currentLevel[currentRow, j] = int.Parse(bricks[j]);
                }
                currentRow++;
            }
            else
            {
                currentRow = 0;
                levels.Add(currentLevel);
                currentLevel = new int[_maxRows, _maxCols];
            }
        }
        return levels;
    }

    public void GenerateBrick()
    {
        this.RemainingBricks = new List<Brick>();
        int[,] currentLevelMatrix = this.levels[this.currentLevel];
        float currentX = _firstBrickX;
        float currentY = _firstBrickY;
        for (int i = 0; i < this._maxRows; i++)
        {
            for (int j = 0; j < this._maxCols; j++)
            {
                int brickType = currentLevelMatrix[i, j];
                if (brickType > 0)
                {
                    Brick newBrick = Instantiate(_brickPrefab, new Vector3(currentX, currentY, -6), Quaternion.identity);
                    newBrick.Init(_bricksContainer.transform, this.mats[brickType - 1], this.colors[brickType -1], brickType);

                    this.RemainingBricks.Add(newBrick);
                }

                currentX += _shiftX;
                if (j + 1 == this._maxCols)
                {
                    currentX = _firstBrickX;
                }
            }

            currentY -= _shiftY;
        }
        this.initialBrickNo = this.RemainingBricks.Count;
        LevelLoaded?.Invoke();

    }

    public void LoadLevel(int level)
    {
        this.currentLevel = level;
        this.ClearAllBricks();
        this.GenerateBrick();
        Player.Instance.InitPlayer();
    }

    public void LoadNextLevel()
    {
        this.currentLevel++;
        if(this.currentLevel >= this.levels.Count)
        {
            GameManager.Instance.finishGame.SetActive(true);
        }
        else
        {
            GameManager.Instance.nextLevel.SetActive(true);
        }
    }

    private void ClearAllBricks()
    {
        foreach(Brick brick in this.RemainingBricks.ToList())
        {
            Destroy(brick.gameObject);
        }
    }
}
