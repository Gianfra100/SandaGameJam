using DG.Tweening;
using UnityEngine;

public class MenuPanel : MonoBehaviour
{
    private void Awake()
    {
        transform.localScale = Vector3.zero;
    }

    public void ShowPanel()
    {
        gameObject.SetActive(true);
        gameObject.PlayScaleAnimation(1f, 0.5f, Ease.OutBack);
    }

    public void HidePanel()
    {
        gameObject.PlayScaleAnimation(0f, 0.5f, Ease.InBack, true, () => this.gameObject.SetActive(false));
    }
}
