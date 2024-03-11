using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Die
{
    public static int throwDie(Player player)
    {
        int number = Random.Range(1, 7);

        switch(number)
        {
            /*case 1:
                player.Cervez(10);
                break;
            case 2:
                player.USpeed *= 0.6f;
                break;
            case 3:
                GameManager.instance.Energy += 10;
                break;
            case 4:
                player.Coins += 5;
                break;
            case 5:
                player.USpeed *= 1.4f;
                break;
            case 6:
                player.Invinivilize(12);
                break;
                */
            default:
                player.USpeed *= 1.4f;break;
        }

        return number;
    }
}
