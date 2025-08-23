using UnityEngine;
using UnityEngine.UI;

public class LevelTimerUI : MonoBehaviour
{
    [SerializeField] private Image timerFillImage;
    private void OnEnable()
    {
        GameEvents.OnTimeUpdate += UpdateTimer;
    }

    private void OnDisable()
    {
        GameEvents.OnTimeUpdate -= UpdateTimer;
    }

    private void UpdateTimer(float currentTime, float totaTime)
    {
        timerFillImage.fillAmount = (float)currentTime / totaTime;
    }
}
