using UnityEngine;

[System.Serializable]
public class LevelData
{
    [Header("Level Settings")]
    public Transform player1SpawnPoint;
    public Transform player2SpawnPoint;

    [Header("Level Time")]
    public int minutes;
    public int seconds;
}
