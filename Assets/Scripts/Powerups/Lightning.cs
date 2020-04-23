using System.Linq;
using UnityEngine;

public class Lightning : Powerup
{
    protected override void ApplyEfect()
    {
        foreach (Ball ball in BallManager.Instance.Balls.ToList())
        {
            ball.StartLightningBall();
        }
    }
} 
