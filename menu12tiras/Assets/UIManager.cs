using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Energy")]
    [SerializeField] private Image energyBar;
    [SerializeField] private Image energyBarFollower;
    [SerializeField] private TMP_Text energyBarText;
    private float currentEnergy;

    [Header("Timer")]
    [SerializeField] private TMP_Text DistanceCounter;
    [SerializeField] private TMP_Text Timer;
    [SerializeField] private Color[] LevelColors = new Color[4];
    [SerializeField] private float[] LevelSizes = new float[4];

    [Header("Velocimeter")]
    [SerializeField] private TMP_Text Velocimeter;
    [SerializeField] private Transform VelocimeterParent;
    private float rotationSpeed;
    private Vector3 startVelocimeterPosition;
    private float oscilationAmount;

    [Header("Coins")]
    [SerializeField] private TMP_Text Coins;
    [SerializeField] private Animator coinParentAnimator;
    [SerializeField] private Image die;

    private void Start()
    {
        Player.onCoinsChanged += updateCoinsUI;
        updateCoinsUI(0);
        Player.onSpeedChanged += updateSpeedUI;

        startVelocimeterPosition = Velocimeter.transform.localPosition;
    }

    private void Update()
    {
        currentEnergy = Mathf.Clamp(GameManager.instance.Energy, 0, 100);
        energyBar.fillAmount = currentEnergy / 100;
        energyBarFollower.fillAmount = Mathf.Lerp(energyBarFollower.fillAmount, energyBar.fillAmount, Mathf.Clamp01(0.3f * Time.deltaTime));
        energyBarText.text = Mathf.FloorToInt(currentEnergy).ToString();

        updateTimerUI();

        VelocimeterParent.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
        Velocimeter.transform.localPosition = startVelocimeterPosition + oscilationAmount * new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), 0);
    }

    private void OnDestroy()
    {
        Player.onCoinsChanged -= updateCoinsUI;
        Player.onSpeedChanged -= updateSpeedUI;
    }

    void updateCoinsUI(int coins)
    {
        Coins.text = coins.ToString();
        die.fillAmount = Mathf.Clamp01((float) coins / 10);
        coinParentAnimator.Play("CoinUI");
    }

    void updateSpeedUI(float speed)
    {
        this.rotationSpeed = Mathf.Pow(speed, 2);
        this.oscilationAmount = Mathf.Pow(speed * 0.05f, 2);
        Velocimeter.text = Mathf.FloorToInt(speed * 4).ToString();
        Velocimeter.transform.localScale = (0.75f + speed / 40) * Vector3.one;
    }

    void updateTimerUI()
    {
        float t = Mathf.FloorToInt(GameManager.instance.TotalTime);

        float s = t % 60;
        float m = Mathf.Clamp((t - 30) / 60, 0, 99999);

        Timer.text = string.Format("{0:000}:{1:00}.{2:000}", m, s, (GameManager.instance.TotalTime - t) * 1000);
        DistanceCounter.text = Mathf.FloorToInt(GameManager.instance.TotalDistance).ToString() + "m";

        DistanceCounter.color = LevelColors[ObstacleGenerator.CurrentLevel - 1];
        DistanceCounter.transform.localScale = LevelSizes[ObstacleGenerator.CurrentLevel - 1] * Vector3.one;
    }

}
