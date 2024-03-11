using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    [SerializeField] private Image energyBar;
    [SerializeField] private TMP_Text energyBarText;
    private float currentEnergy;

    [SerializeField] private TMP_Text Timer;

    [SerializeField] private TMP_Text Velocimeter;
    [SerializeField] private TMP_Text Coins;
    [SerializeField] private Image die;

    private void Update()
    {
        currentEnergy = Mathf.Clamp(GameManager.instance.Energy, 0, 100);
        energyBar.fillAmount = currentEnergy / 100;
        energyBarText.text = Mathf.FloorToInt(currentEnergy).ToString();


        float t = Mathf.FloorToInt(GameManager.instance.TotalTime);

        float s = t % 60;
        float m = Mathf.Clamp((t-30) / 60, 0, 99999);

        Timer.text = string.Format("{0:000}:{1:00}.{2:000}", m, s, (GameManager.instance.TotalTime - t) * 1000);

        Velocimeter.text = Mathf.FloorToInt(GameManager.instance.player.USpeed * 4).ToString();
        Coins.text = GameManager.instance.player.Coins.ToString();

        die.fillAmount = Mathf.Clamp01((float)GameManager.instance.player.Coins/10);
    }

}
