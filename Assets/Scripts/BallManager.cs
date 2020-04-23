using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class BallManager : MonoBehaviour
{
    #region Singleton
    private static BallManager _instance;
    public static BallManager Instance => _instance;
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
    [SerializeField]
    private Ball _ballPrefab;
    [SerializeField]
    public static float _initialBallSpeed = 200f;

    private Ball _initialBall;
    public Rigidbody _initialRb;
    
    public List<Ball> Balls { get; set; }
    
    private void Start()
    {
        InitBall();
    }

    private void Update()
    {
            if (!GameManager.Instance.isGameStarted)
            {
                Vector3 playerPosition = Player.Instance.gameObject.transform.position;
                _initialBall.transform.position = new Vector3(playerPosition.x, playerPosition.y + .11f, -6);
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    _initialRb.isKinematic = false;
                    _initialRb.AddForce(new Vector3(0, _initialBallSpeed, 0));
                    GameManager.Instance.isGameStarted = true;
                }

            }
    }

    public void ResetBalls()
    {
        foreach (Ball ball in this.Balls.ToList())
        {
            Destroy(ball.gameObject);
        }
        InitBall();
    }

    public void InitBall()
    {
        Vector3 playerPosition = Player.Instance.gameObject.transform.position;
        Vector3 startPosition = new Vector3(playerPosition.x, playerPosition.y + .11f, -6);
        _initialBall = Instantiate(_ballPrefab, startPosition, Quaternion.identity);
        _initialRb = _initialBall.GetComponent<Rigidbody>();

        this.Balls = new List<Ball>
        {
            _initialBall
        };
    }

    public void SpawnBalls(Vector3 position, int count, bool isFireball)
    {
        for(int i = 0; i < count; i++)
        {
            Ball ball = Instantiate(_ballPrefab, position, Quaternion.identity) as Ball;
            if (isFireball)
            {
                ball.StartLightningBall();
            }
            Rigidbody ballRb = ball.GetComponent<Rigidbody>();
            ballRb.isKinematic = false;
            ballRb.AddForce(new Vector3(0, _initialBallSpeed, 0));
            this.Balls.Add(ball);
        }
    }
}
