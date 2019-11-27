using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public string PlayerName;
    public int powerUpType;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (PlayerName.Equals(col.name))
        {
            PlayerMovement player = col.gameObject.GetComponent<PlayerMovement>();
            switch(powerUpType)
            {
                case 0://speed
                    player.StartCoroutine(player.SpeedPowerUp(5));
                    break;
                case 1://time
                    player.PowerUpAddTime(20);
                    break;
                case 2://score
                    player.PowerUpAddPoints(50);
                    break;
            }
            Destroy(this.gameObject);
        }
    }

    private void Update()
    {
        if (Constants.GameOver)
        {
            Destroy(this.gameObject);
        }
    }

}
