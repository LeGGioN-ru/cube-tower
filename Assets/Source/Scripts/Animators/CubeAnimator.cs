using System;
using DG.Tweening;
using UnityEngine;

public static class CubeAnimator
{
    public static Tween PlayJump(Transform cubeTransform, Vector3 endPos)
    {
        return cubeTransform.DOJump(endPos, 4, 1, 0.5f);
    }

    public static Tween PlayFail(GameCubeView view, Action onBeforeDestroy = null)
    {
        return view.SpriteRenderer
            .DOFade(0f, 0.5f)
            .OnComplete(() =>
            {
                onBeforeDestroy?.Invoke();
                UnityEngine.Object.Destroy(view.gameObject);
            });
    }

    public static void PlayRemove(GameCubeView view)
    {
        view.transform.DOKill();
        UnityEngine.Object.Destroy(view.gameObject);
    }

    public static Tween PlayShift(Transform cubeTransform, float targetY)
    {
        cubeTransform.DOKill();
        return cubeTransform.DOMoveY(targetY, 0.4f).SetEase(Ease.OutBounce);
    }
}
