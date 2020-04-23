using System.Linq;
using UnityEngine;

public class Multiball : Powerup
{
    protected override void ApplyEfect()
    {
        foreach(Ball ball in BallManager.Instance.Balls.ToList())
        {
            BallManager.Instance.SpawnBalls(new Vector3(ball.gameObject.transform.position.x + 0.1f, ball.gameObject.transform.position.y+0.1f, ball.gameObject.transform.position.z), 2, ball.isLightning);
        }
    }
}
