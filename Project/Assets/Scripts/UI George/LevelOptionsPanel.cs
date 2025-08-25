using UnityEngine;
using UnityEngine.UI;

public class LevelOptionsPanel : MonoBehaviour
{
    [SerializeField] private Button menuButton;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button nextLevelButton;

    private void OnEnable()
    {
        SetPanel();  
    }

    private void SetPanel()
    { 
        Scenes nextLevel = ScenesUtils.GetNextScene(ScenesUtils.GetCurrentScene());
        if (nextLevel == Scenes.Menu)
        {
            nextLevelButton?.gameObject.SetActive(false);
        }
        else
        {
            nextLevelButton?.gameObject.SetActive(true);
        }

        menuButton?.onClick.AddListener(() => SceneLoader.Instance?.LoadScene(Scenes.Menu));
        restartButton?.onClick.AddListener(() => SceneLoader.Instance?.LoadScene(ScenesUtils.GetCurrentScene()));
        nextLevelButton?.onClick.AddListener(() => SceneLoader.Instance?.LoadScene(nextLevel));
    }
}
