using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    private const int TARGET_FRAME_RATE = 60;
    
    private void Awake()
    {
        Application.targetFrameRate = TARGET_FRAME_RATE;
        transform.localScale = Vector3.zero;
    }

    private void Start()
    {
        gameObject.PlayScaleAnimation(1f, 1f, Ease.OutBack, true);
    }

    public void PlayButton()
    {
        gameObject.PlayScaleAnimation(0f, 0.5f, Ease.InBack, true, () =>
        {
            SceneManager.LoadScene("Test");
        });
    }

    public void ExitButton()
    {
        gameObject.PlayScaleAnimation(0f, 0.5f, Ease.InBack, true, () =>
        {
            Application.Quit();
        });
    }
}
