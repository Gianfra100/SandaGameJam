using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject player1Prefab;
    [SerializeField] private GameObject player2Prefab;

    private LevelManager levelManager;

    private BasePlayer player1;
    private BasePlayer player2;

    private void Start()
    {
        InitilizeLevel();
    }

    private void InitilizeLevel()
    {
        levelManager = FindFirstObjectByType<LevelManager>();

        if (levelManager == null)
        {
            Debug.LogError("LevelManager not found in the scene.");
            return;
        }

        InitializePlayers();
        PlayMusic();
    }
    private void InitializePlayers()
    {
        if (player1Prefab == null || player2Prefab == null)
        {
            Debug.LogError("Player prefabs are not assigned in the GameManager.");
            return;
        }

        var p1 = PlayerInput.Instantiate(player1Prefab, controlScheme: "KeyboardLeft", pairWithDevice: Keyboard.current);
        var p2 = PlayerInput.Instantiate(player2Prefab, controlScheme: "KeyboardRight", pairWithDevice: Keyboard.current);

        player1 = p1.GetComponent<BasePlayer>();
        player2 = p2.GetComponent<BasePlayer>();

        if (player1 == null || player2 == null)
        {
            Debug.LogError("BasePlayer component missing on one of the player prefabs.");
            return;
        }

        player1.transform.position = levelManager.GetPlayerSpawnPoint(PlayerType.Player1);
        player2.transform.position = levelManager.GetPlayerSpawnPoint(PlayerType.Player2);
    }

    private void PlayMusic()
    { 
        AudioManager.Instance?.PlayMusic(ScenesUtils.GetMusicEnum());
    }
}
