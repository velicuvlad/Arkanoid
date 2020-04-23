using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Death : MonoBehaviour
{
    private void OnTriggerEnter(Collider coll)
    {
        if(coll.tag == "Ball")
        {
            Ball ball = coll.GetComponent<Ball>();
            BallManager.Instance.Balls.Remove(ball);
            ball.Die();

        }
    }
}
