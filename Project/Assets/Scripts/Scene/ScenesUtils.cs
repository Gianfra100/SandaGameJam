using UnityEngine;

public static class ScenesUtils
{
    public static string GetSceneName(Scenes scene)
    {
        switch (scene)
        {
            case Scenes.MainMenu:
                return "MainMenu";
            case Scenes.Level1:
                return "Level1";
            default:
                Debug.LogError("Scene not recognized");
                return string.Empty;
        }
    }

    public static Scenes GetScene(string sceneName)
    {
        return sceneName switch
        {
            "MainMenu" => Scenes.MainMenu,
            "Level1" => Scenes.Level1,
            _ => Scenes.Level1,
        };
    }
}
