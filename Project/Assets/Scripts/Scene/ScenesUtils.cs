using UnityEngine;
using UnityEngine.SceneManagement;

public static class ScenesUtils
{
    public static string GetSceneName(Scenes scene)
    {
        switch (scene)
        {
            case Scenes.Menu:
                return "Menu";
            case Scenes.Level1:
                return "Level1";
            case Scenes.level2:
                return "Level2";
            case Scenes.level3:
                return "Level3";
            default:
                Debug.LogError("Scene not recognized");
                return string.Empty;
        }
    }

    public static Scenes GetScene(string sceneName)
    {
        return sceneName switch
        {
            "Menu" => Scenes.Menu,
            "Level1" => Scenes.Level1,
            "Level2" => Scenes.level2,
            "Level3" => Scenes.level3,
            _ => Scenes.Level1,
        };
    }

    public static Scenes GetCurrentScene()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        return sceneName switch
        {
            "Menu" => Scenes.Menu,
            "Level1" => Scenes.Level1,
            "Level2" => Scenes.level2,
            "Level3" => Scenes.level3,
            _ => Scenes.Menu,
        };
    }

    public static Scenes GetNextScene(Scenes currentScene)
    {
        return currentScene switch
        {
            Scenes.Level1 => Scenes.level2,
            Scenes.level2 => Scenes.level3,
            Scenes.level3 => Scenes.Menu,
            _ => Scenes.Menu,
        };
    }

    public static Music GetMusicEnum()
    {
        Scenes currentScene = GetCurrentScene();
        return currentScene switch
        {
            Scenes.Menu => Music.Menu,
            Scenes.Level1 => Music.Level,
            _ => Music.Menu,
        };
    }
}
