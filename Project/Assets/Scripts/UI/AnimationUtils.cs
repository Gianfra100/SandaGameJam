using System;
using DG.Tweening;
using UnityEngine;

public static class AnimationUtils
{
    public static void PlayScaleAnimation(this GameObject gameObject, float scale, float duration, Ease easing = Ease.Linear, bool setUpdate = true, Action callback = null)
    {
         if (gameObject == null || !gameObject.activeInHierarchy) return;
        gameObject.transform
        .DOScale(scale, duration)
        .SetEase(easing)
        .SetUpdate(setUpdate)
        .OnComplete(() => callback?.Invoke());
    }
}
