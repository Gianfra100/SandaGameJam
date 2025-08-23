using System.Collections;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private LevelData levelData;

    private float totalLevelTime;
    private Coroutine levelTimer;

    private void OnEnable()
    {
        GameEvents.OnGameLose += StopTimer;
        GameEvents.OnGameWin += StopTimer;
    }
    private void OnDisable()
    {
        GameEvents.OnGameLose -= StopTimer;
        GameEvents.OnGameWin -= StopTimer;
    }

    void Start()
    {
        if (levelData == null)
        {
            Debug.LogError("LevelData is not assigned in LevelManager.");
            return;
        }

        levelTimer = StartCoroutine(LevelCountdown());
    }

    public Vector3 GetPlayerSpawnPoint(PlayerType player)
    {
        if (levelData == null)
        {
            Debug.LogError("LevelData is not assigned in LevelManager.");
            return Vector3.zero;
        }

        if (levelData.player1SpawnPoint != null && levelData.player2SpawnPoint != null)
        {
            return player == PlayerType.Player1 ? levelData.player1SpawnPoint.position : levelData.player2SpawnPoint.position;
        }

        Debug.LogError("Player spawn point is not set in LevelData.");

        return Vector3.zero;
    }

    private IEnumerator LevelCountdown()
    {
        totalLevelTime = levelData.minutes * 60 + levelData.seconds;
        float currentTime = totalLevelTime;
        UpdateUITimer(currentTime);

        while (currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
            UpdateUITimer(currentTime);
        }

        GameEvents.GameLose();
    }

    private void UpdateUITimer(float currentTime)
    {
        GameEvents.TimeUpdate(currentTime, totalLevelTime);
    }
    
    private void StopTimer()
    {
        if (levelTimer != null)
        {
            StopCoroutine(levelTimer);
            levelTimer = null;
        }
    }
}
