using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance { get; private set; }

    [SerializeField] private GameObject loadingScreen;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        PlayMusic();
    }

    public void LoadScene(Scenes sceneToLoad)
    {
        string sceneName = ScenesUtils.GetSceneName(sceneToLoad);
        StartCoroutine(LoadSceneAsync(sceneName));
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {

        loadingScreen.SetActive(true);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        yield return new WaitForSeconds(5f);

        loadingScreen.SetActive(false);

        PlayMusic();

        yield return null;
    }

    private void PlayMusic()
    {
        if (!AudioManager.Instance.IsMusicPlaying(ScenesUtils.GetMusicEnum()))
        { 
            AudioManager.Instance?.PlayMusic(ScenesUtils.GetMusicEnum());
        }
    }
}
