using UnityEditor.SearchService;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    public void OnPlayButtonClicked(string sceneName)
    {
        Scenes sceneToLoad = ScenesUtils.GetScene(sceneName);
        SceneLoader.Instance.LoadScene(sceneToLoad);
    }

    public void OnExitButtonClicked()
    {
        Application.Quit();
    }
}
