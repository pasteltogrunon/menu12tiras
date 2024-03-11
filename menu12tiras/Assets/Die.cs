using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Die
{
    public static int throwDie(Player player)
    {
        int number = Random.Range(1, 6);

        switch(number)
        {
            case 1:
                player.Cervez(10);
                break;
            case 2:
                player.USpeed *= 0.4f;
                break;
            case 3:
                GameManager.instance.Energy += 5;
                break;
            case 4:
                player.USpeed *= 1.3f;
                break;
            default:
                break;
        }

        return number;
    }
}
