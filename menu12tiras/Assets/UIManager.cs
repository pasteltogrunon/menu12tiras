using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    [SerializeField] private Image energyBar;
    [SerializeField] private TMP_Text energyBarText;

    [SerializeField] private TMP_Text Timer;

    [SerializeField] private TMP_Text Velocimeter;

    private void Update()
    {
        energyBar.fillAmount = GameManager.instance.EnergyLevel / 100;
        energyBarText.text = Mathf.FloorToInt(GameManager.instance.EnergyLevel).ToString();


        float t = Mathf.FloorToInt(GameManager.instance.TotalTime);

        float s = t % 60;
        float m = t / 60;

        Timer.text = string.Format("{0:000}:{1:00}.{2:000}", m, s, (GameManager.instance.TotalTime - t) * 1000);

        Velocimeter.text = Mathf.FloorToInt(GameManager.instance.Player.USpeed * 4).ToString();
    }

}
