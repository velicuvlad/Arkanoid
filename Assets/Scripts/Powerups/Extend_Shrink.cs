using UnityEngine;

public class Extend_Shrink : Powerup
{
    
    public float extend ;
   
    protected override void ApplyEfect()
    {
        if(Player.Instance != null && !Player.Instance.playerTransforming)
        {
            Player.Instance.StartWidthAnimation(extend);
        }
    }
}
