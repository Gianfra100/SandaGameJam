using UnityEngine;

public class MenuControllerGeorge : MonoBehaviour
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
