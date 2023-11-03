using DG.Tweening;
using UnityEngine;

public class AnimationBall : MonoBehaviour
{
    public float duration1;
    public float duration2;
    public float multiplyValue;
    public Vector3 originScale;
    public Ease ease1;
    public Ease ease2;
    [Button]
    public void SetScale()
    {
        transform.DOScale(originScale * multiplyValue, duration1).SetEase(ease1);
        Invoke(nameof(ScaleDecrese), duration1);
    }

    public void ScaleDecrese()
    {
        transform.DOScale(originScale, duration2).SetEase(ease2);
    }
    private void OnDisable()
    {
        transform.DOKill();
    }
}
