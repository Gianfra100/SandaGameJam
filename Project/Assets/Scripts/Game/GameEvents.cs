using System;

public static class GameEvents
{
    public static event Action OnGameWin;
    public static event Action OnGameLose;
    public static event Action OnGamePause;
    public static event Action OnGameResume;
    public static event Action OnGameReload;
    public static event Action OnGameStart;
    public static event Action OnGameEnd;
    public static event Action OnSceneLoaded;
    public static event Action<float, float> OnTimeUpdate;


    public static void GameWin() => OnGameWin?.Invoke();
    public static void GameLose() => OnGameLose?.Invoke();
    public static void GamePause() => OnGamePause?.Invoke();
    public static void GameResume() => OnGameResume?.Invoke();
    public static void GameReload() => OnGameReload?.Invoke();
    public static void StartGame() => OnGameStart?.Invoke();
    public static void GameEnd() => OnGameEnd?.Invoke();
    public static void SceneLoaded() => OnSceneLoaded?.Invoke();
    public static void TimeUpdate(float currentTime,  float totaTime) => OnTimeUpdate?.Invoke(currentTime, totaTime);
}
