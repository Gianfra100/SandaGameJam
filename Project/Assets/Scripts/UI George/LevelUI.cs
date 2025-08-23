using System.Runtime.CompilerServices;
using UnityEngine;

public class LevelUI : MonoBehaviour
{
    [SerializeField] private GameObject LevelCompletePanel;
    [SerializeField] private GameObject LevelFailedPanel;

    private void OnEnable()
    {
        GameEvents.OnGameWin += DisplayLevelComplete;
        GameEvents.OnGameLose += DiplayLevelFailed;
    }

    private void OnDisable()
    {
        GameEvents.OnGameWin -= DisplayLevelComplete;
        GameEvents.OnGameLose -= DiplayLevelFailed;
    }
    
    private void DisplayLevelComplete()
    {

        LevelCompletePanel.SetActive(true);
    }

    private void DiplayLevelFailed()
    {
        LevelFailedPanel.SetActive(true);
    }
}
